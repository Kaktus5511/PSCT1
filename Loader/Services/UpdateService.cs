using Ionic.Zip;
using Loader.Model;
using Loader.Model.Config;
using Loader.Model.Message;
using Loader.Services.Model;
using Loader.ViewModels;
using Loader.ViewModels.Model;
using log4net;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.VisualStudio.Threading;
using PlaySharp.Service.Messages;
using PlaySharp.Service.Package.Model;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Loader.Service.GameLocator;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Messages;
using PlaySharp.Toolkit.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Loader.Services
{
	[Export(typeof(IUpdateService))]
	public class UpdateService : IUpdateService, IActivatable, IStateful, IDeactivatable, IHandleWithTask<OnPackageInstalled>, IHandle, IHandle<OnPackageUninstalling>
	{
		private readonly static ILog Log;

		[Import(typeof(PlaySharpConfig), AllowRecomposition=true)]
		public Lazy<PlaySharpConfig> Config
		{
			get;
			set;
		}

		[Import(typeof(IEventAggregator))]
		public Lazy<IEventAggregator> EventAggregator
		{
			get;
			set;
		}

		[ImportMany(typeof(IGameLocator))]
		public IEnumerable<Lazy<IGameLocator>> GameLocators
		{
			get;
			set;
		}

		public bool IsActive
		{
			get
			{
				return JustDecompileGenerated_get_IsActive();
			}
			set
			{
				JustDecompileGenerated_set_IsActive(value);
			}
		}

		private bool JustDecompileGenerated_IsActive_k__BackingField;

		public bool JustDecompileGenerated_get_IsActive()
		{
			return this.JustDecompileGenerated_IsActive_k__BackingField;
		}

		private void JustDecompileGenerated_set_IsActive(bool value)
		{
			this.JustDecompileGenerated_IsActive_k__BackingField = value;
		}

		public bool IsPreparing
		{
			get;
			set;
		}

		private bool LastCoreUpdateResult
		{
			get;
			set;
		}

		[Import(typeof(IBusyOverlayView))]
		public IBusyOverlayView Overlay
		{
			get;
			set;
		}

		[Import(typeof(IPackageClient))]
		public Lazy<IPackageClient> PackageClient
		{
			get;
			set;
		}

		[Import(typeof(IWebServiceClient))]
		public Lazy<IWebServiceClient> ServiceClient
		{
			get;
			set;
		}

		[Import(typeof(IShellView))]
		public Lazy<IShellView> Shell
		{
			get;
			set;
		}

		private AsyncLock SnycRoot { get; } = new AsyncLock();

		private EasyCache UpdateCache { get; } = new EasyCache("UpdateService", null);

		static UpdateService()
		{
			UpdateService.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public UpdateService()
		{
		}

		public void Activate()
		{
			if (this.IsActive)
			{
				return;
			}
			this.IsActive = true;
			this.EventAggregator.Value.Subscribe(this);
			Thread thread = new Thread(new ThreadStart(this.UrlHandlerThread))
			{
				Name = "Url Handler",
				IsBackground = false
			};
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
		}

		public void Deactivate()
		{
			if (!this.IsActive)
			{
				return;
			}
			this.IsActive = false;
			this.EventAggregator.Value.Unsubscribe(this);
		}

		public void Handle(OnPackageUninstalling message)
		{
			PackageAssembly packageAssembly = new PackageAssembly(message.Package);
			PlaySharp.Toolkit.Helper.ThreadHelper.JoinableTaskFactory.Run(async () => await packageAssembly.DeleteAsync());
		}

		public async Task Handle(OnPackageInstalled message)
		{
			IProfile orCreate;
			PackageAssembly packageAssembly = new PackageAssembly(message.Package);
			if (message.Package.AssemblyEntry.Type == AssemblyType.Library)
			{
				orCreate = this.Config.Value.ServiceSettings.Profiles.GetOrCreate<LibrariesProfile>(null, true);
				if (!this.IsPreparing)
				{
					await packageAssembly.CompileAsync();
				}
				else if (orCreate.Add(packageAssembly))
				{
					await packageAssembly.CompileAsync();
				}
				await this.SatisfyAsync(packageAssembly.AssemblyEntry);
				EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnProfileChanged(orCreate));
			}
			orCreate = null;
		}

		public async Task HandleUrl(string url)
		{
			AssemblyData assemblyDatum;
			IProfile orCreate;
			AssemblyEntry assemblyEntry;
			try
			{
				if (url.StartsWith("ps://"))
				{
					string[] strArrays = url.Split(new char[] { '/' });
					string str = strArrays[2];
					if (str == "profile")
					{
						string str1 = HttpUtility.UrlDecode(strArrays[3]);
						string str2 = strArrays[4];
						char[] chrArray = new char[] { ',' };
						IEnumerable<int> nums = str2.Split(chrArray).Select<string, int>(new Func<string, int>(int.Parse));
						if (!this.Config.Value.ServiceSettings.Profiles.All<IProfile>((IProfile p) => p.Name != str1))
						{
							await this.Shell.Value.ShowMessageAsync("Profile Import", string.Format("Profile with the same name already exists '{0}'", str1), 0);
						}
						else
						{
							orCreate = this.Config.Value.ServiceSettings.Profiles.GetOrCreate<Profile>(str1, true);
							assemblyDatum = await this.ServiceClient.Value.AssembliesAsync();
							List<AssemblyEntry> list = assemblyDatum.Assemblies.Where<AssemblyEntry>((AssemblyEntry a) => {
								if (!a.IsValid)
								{
									return false;
								}
								return nums.Contains<int>(a.Id);
							}).ToList<AssemblyEntry>();
							await this.InstallAsync(list, orCreate);
							EventAggregatorExtensions.PublishOnUiThread(this.EventAggregator.Value, new OnProfilesChanged());
							await this.Shell.Value.ShowMessageAsync("Profile Import", string.Format("Successfully imported '{0}'", str1), 0);
						}
					}
					else if (str == "assembly")
					{
						int num = int.Parse(strArrays[3]);
						orCreate = this.Config.Value.ServiceSettings.Profiles.FirstOrDefault<IProfile>();
						assemblyDatum = await this.ServiceClient.Value.AssembliesAsync();
						assemblyEntry = assemblyDatum.Assemblies.FirstOrDefault<AssemblyEntry>((AssemblyEntry a) => {
							if (!a.IsValid)
							{
								return false;
							}
							return a.Id == num;
						});
						await this.InstallAsync(assemblyEntry, orCreate);
						EventAggregatorExtensions.PublishOnUiThread(this.EventAggregator.Value, new OnProfilesChanged());
						await this.Shell.Value.ShowMessageAsync("Assembly Import", string.Format("Successfully imported '{0}' into '{1}'", assemblyEntry.Name, orCreate.Name), 0);
					}
					assemblyEntry = null;
					strArrays = null;
					orCreate = null;
				}
			}
			catch (Exception exception)
			{
				UpdateService.Log.Warn(exception);
			}
		}

		public async Task<bool> InstallAsync(IList<AssemblyEntry> assemblies, IProfile profile)
		{
			bool flag = false;
			await this.Overlay.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
				model.CanAbort = true;
				foreach (AssemblyEntry assembly in assemblies)
				{
					try
					{
						model.Maximum = (double)assemblies.Count;
						model.Text = string.Format("Install [{0}] {1}", assembly.Id, assembly.Name);
						if (await this.PackageClient.Value.InstallAsync(assembly, true))
						{
							PackageAssembly packageAssembly = new PackageAssembly(assembly);
							await packageAssembly.CompileAsync();
							profile.Add(packageAssembly);
							flag = true;
							await this.SatisfyAsync(packageAssembly.AssemblyEntry);
							packageAssembly = null;
						}
						token.ThrowIfCancellationRequested();
					}
					catch (TaskCanceledException taskCanceledException)
					{
						UpdateService.Log.Debug(string.Format("{0} Canceled", "InstallAsync"));
					}
					catch (Exception exception)
					{
						UpdateService.Log.Warn(exception);
					}
				}
			});
			this.Config.Value.Save(false);
			return flag;
		}

		public async Task<bool> InstallAsync(AssemblyEntry assembly, IProfile profile)
		{
			bool flag = false;
			await this.Overlay.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
				model.CanAbort = true;
				model.Maximum = 1;
				model.Text = string.Format("Install [{0}] {1}", assembly.Id, assembly.Name);
				if (await this.PackageClient.Value.InstallAsync(assembly, true))
				{
					PackageAssembly packageAssembly = new PackageAssembly(assembly);
					await packageAssembly.CompileAsync();
					profile.Add(packageAssembly);
					flag = true;
					await this.SatisfyAsync(packageAssembly.AssemblyEntry);
					packageAssembly = null;
				}
			});
			this.Config.Value.Save(false);
			return flag;
		}

		public async Task<bool> IsCoreSupportedAsync()
		{
			bool flag;
			try
			{
				CoreData coreDatum = await this.ServiceClient.Value.CoreAsync(HashFactory.MD5.File(this.Config.Value.ServiceSettings.GameFilePath));
				CoreData coreDatum1 = coreDatum;
				if (coreDatum1 != null && coreDatum1.Core.HashCore == HashFactory.MD5.File(Files.Core.FullName))
				{
					flag = true;
					return flag;
				}
			}
			catch (Exception exception)
			{
				UpdateService.Log.Error(exception);
			}
			flag = false;
			return flag;
		}

		public async Task PrepareAssembliesAsync(AssemblyUpdate type, bool forceCompile = false)
		{
			UpdateService.<PrepareAssembliesAsync>d__55 variable = new UpdateService.<PrepareAssembliesAsync>d__55();
			variable.<>4__this = this;
			variable.type = type;
			variable.forceCompile = forceCompile;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<UpdateService.<PrepareAssembliesAsync>d__55>(ref variable);
			return variable.<>t__builder.Task;
		}

		private async Task SatisfyAsync(AssemblyEntry entry)
		{
			try
			{
				AssemblyData assemblyDatum = await this.ServiceClient.Value.AssembliesAsync();
				IProfile orCreate = this.Config.Value.ServiceSettings.Profiles.GetOrCreate<LibrariesProfile>(null, true);
				foreach (AssemblyEntry assemblyEntry in assemblyDatum.GetDependencies(entry.LatestVersion))
				{
					if (assemblyEntry.IsValidVersion)
					{
						PackageAssembly packageAssembly = new PackageAssembly(assemblyEntry);
						if (orCreate.Add(packageAssembly) && !packageAssembly.IsCached)
						{
							await packageAssembly.CompileAsync();
						}
						await this.SatisfyAsync(packageAssembly.AssemblyEntry);
						packageAssembly = null;
					}
					else
					{
						EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage("Dependency Installer", string.Format("Failed to install {0}", assemblyEntry)));
					}
				}
				orCreate = null;
			}
			catch (Exception exception)
			{
				UpdateService.Log.Error(exception);
			}
		}

		public async Task<bool> UpdateCoreAsync(string module, CancellationToken token = null)
		{
			bool item;
			if (!this.Config.Value.Settings.BypassCoreUpdate)
			{
				using (await this.SnycRoot.LockAsync())
				{
					bool flag = false;
					string str = HashFactory.MD5.File(module);
					string str1 = HashFactory.MD5.File(Files.Core.FullName);
					string str2 = HashFactory.MD5.File(Files.CoreBridge.FullName);
					CoreData coreDatum = null;
					if (!this.UpdateCache.Contains(str, null))
					{
						coreDatum = await this.ServiceClient.Value.CoreAsync(str);
						if (coreDatum != null)
						{
							CoreEntry core = coreDatum.Core;
							if (core.HashCore != str1)
							{
								UpdateService.Log.Debug(string.Format("{0} Update Core because of {1} mismatch", this, "coreHash"));
								flag = true;
							}
							if (core.HashCoreBridge != str2)
							{
								UpdateService.Log.Debug(string.Format("{0} Update Core because of {1} mismatch", this, "coreBridgeHash"));
								flag = true;
							}
							if (flag)
							{
								try
								{
									try
									{
										Singleton<Eudyptula>.Instance.Start();
										using (WebClient webClient = new WebClient())
										{
											webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler((object sender, DownloadProgressChangedEventArgs args) => EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnDownloadProgressChanged(string.Format("Core {0}", core.Id), args)));
											UpdateService.Log.Debug(string.Format("Download {0}", core.DataUrl));
											byte[] numArray = await webClient.DownloadDataTaskAsync(core.DataUrl);
											using (MemoryStream memoryStream = new MemoryStream(numArray))
											{
												using (ZipFile zipFiles = ZipFile.Read(memoryStream))
												{
													using (FileStream fileStream1 = new FileStream(Files.Core.FullName, FileMode.Create))
													{
														ZipFile zipFiles1 = zipFiles;
														zipFiles1.First<ZipEntry>((ZipEntry e) => e.FileName.EndsWith(Files.Core.Name)).Extract(fileStream1);
													}
													using (FileStream fileStream = new FileStream(Files.CoreBridge.FullName, FileMode.Create))
													{
														ZipFile zipFiles2 = zipFiles;
														zipFiles2.First<ZipEntry>((ZipEntry e) => e.FileName.EndsWith(Files.CoreBridge.Name)).Extract(fileStream);
													}
												}
											}
										}
										webClient = null;
										await PathRandomizer.CopyFilesAsync();
									}
									catch (Exception exception)
									{
										UpdateService.Log.Error(exception);
										item = false;
										return item;
									}
								}
								finally
								{
									Singleton<Eudyptula>.Instance.Stop();
								}
								this.UpdateCache.Add(str, true, TimeSpan.FromMinutes(10));
								item = true;
							}
							else
							{
								this.UpdateCache.Add(str, true, TimeSpan.FromMinutes(10));
								item = true;
							}
						}
						else
						{
							this.UpdateCache.Add(str, false, TimeSpan.FromMinutes(10));
							Singleton<Eudyptula>.Instance.Start();
							EventAggregatorExtensions.PublishOnUiThread(this.EventAggregator.Value, new OnCloseApplication(string.Format("PlaySharp is outdated for your Game!\nVersion: {0}\n\nGame and Loader will exit now.", str)));
							item = false;
						}
					}
					else
					{
						UpdateService.Log.Debug(string.Format("Using Cache result for {0} - {1}", str, this.UpdateCache[str]));
						item = (bool)this.UpdateCache[str];
					}
				}
			}
			else
			{
				UpdateService.Log.Warn("I-WANT-TO-GET-BANNED");
				item = true;
			}
			return item;
		}

		public async Task<bool> UpdateCoreAsync()
		{
			UpdateService.<UpdateCoreAsync>d__57 variable = new UpdateService.<UpdateCoreAsync>d__57();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<UpdateService.<UpdateCoreAsync>d__57>(ref variable);
			return variable.<>t__builder.Task;
		}

		public async Task UpdateEnsageTextures()
		{
			string fullName;
			if (this.Config.Value.Service == ServiceType.Dota)
			{
				if (!this.Config.Value.ServiceSettings.GameFilePath.IsNullOrEmpty())
				{
					DirectoryInfo parent = (new DirectoryInfo(this.Config.Value.ServiceSettings.GameFilePath)).Parent;
					if (parent != null)
					{
						DirectoryInfo directoryInfo = parent.Parent;
						if (directoryInfo != null)
						{
							DirectoryInfo parent1 = directoryInfo.Parent;
							if (parent1 != null)
							{
								fullName = parent1.FullName;
							}
							else
							{
								fullName = null;
							}
						}
						else
						{
							fullName = null;
						}
					}
					else
					{
						fullName = null;
					}
					string str = fullName;
					if (str != null)
					{
						string str1 = Path.Combine(str, "dota", "materials", "ensage_ui");
						await this.Overlay.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
							model.CanAbort = true;
							model.Maximum = 2;
							model.Text = "Updating Ensage Texture Pack";
							await GitUpdater.UpdateAsync("https://github.com/h3h3/EnsageTexturePack", str1);
							model.Text = "Updating Ensage Texture Pack Completed.";
						});
					}
					else
					{
						UpdateService.Log.Warn("Steam/dota 2 beta/game not found.");
					}
				}
				else
				{
					UpdateService.Log.Warn("Steam not found.");
				}
			}
		}

		public bool UpdateGamePath()
		{
			bool flag;
			try
			{
				if (!File.Exists(this.Config.Value.ServiceSettings.GameFilePath))
				{
					Lazy<IGameLocator> locator = this.GameLocators.FirstOrDefault<Lazy<IGameLocator>>((Lazy<IGameLocator> l) => l.Value.Service == this.Config.Value.Service);
					if (locator != null && locator.Value.Search())
					{
						this.Config.Value.ServiceSettings.GameFilePath = locator.Value.Location;
					}
					flag = !string.IsNullOrEmpty(this.Config.Value.ServiceSettings.GameFilePath);
				}
				else
				{
					flag = true;
				}
			}
			catch (Exception exception)
			{
				UpdateService.Log.Error(exception);
				return false;
			}
			return flag;
		}

		public async Task UpdateLoaderAsync(bool confirm = true)
		{
			try
			{
				try
				{
					UpdateService.Log.Debug("Checking for Loader Updates.");
					string str = Path.Combine(Directories.Cache, "update.exe");
					LoaderVersionData loaderVersionDatum = null;
					Version version = Assembly.GetExecutingAssembly().GetName().Version;
					str.DeleteFile();
					LoaderVersionData loaderVersionDatum1 = await this.ServiceClient.Value.LoaderVersionAsync(this.Config.Value.Settings.LoaderUpdateChannel);
					loaderVersionDatum = loaderVersionDatum1;
					if (loaderVersionDatum == null)
					{
						UpdateService.Log.Warn("Failed to retrieve Loader Version.");
						return;
					}
					else if (version != loaderVersionDatum.Version)
					{
						Singleton<Eudyptula>.Instance.Start();
						if (confirm)
						{
							MessageDialogResult messageDialogResult = await this.Shell.Value.ShowMessageAsync("Loader Update", string.Format("New Loader update available {0} -> {1}\nUpdate now?", version, loaderVersionDatum.Version), 1);
							if (messageDialogResult == null)
							{
								return;
							}
						}
						UpdateService.Log.Debug(string.Format("Update Loader to [{0}] {1}", this.Config.Value.Settings.LoaderUpdateChannel, loaderVersionDatum.Version));
						await this.Overlay.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
							UpdateService.<>c__DisplayClass60_0.<<UpdateLoaderAsync>b__0>d _ = new UpdateService.<>c__DisplayClass60_0.<<UpdateLoaderAsync>b__0>d();
							_.<>4__this = this;
							_.model = model;
							_.<>t__builder = AsyncTaskMethodBuilder.Create();
							_.<>1__state = -1;
							_.<>t__builder.Start<UpdateService.<>c__DisplayClass60_0.<<UpdateLoaderAsync>b__0>d>(ref _);
							return _.<>t__builder.Task;
						});
						if (File.Exists(str))
						{
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.Append("/CLOSEAPPLICATIONS ");
							stringBuilder.Append("/SILENT ");
							stringBuilder.Append(string.Concat("/DIR=\"", Directories.Current, "\" "));
							stringBuilder.Append(string.Concat("/LOG=\"", Path.Combine(Directories.Logs, string.Format("setup-{0}.log", loaderVersionDatum.Version)), "\" "));
							ServiceType service = this.Config.Value.Service;
							if (service == ServiceType.LoL)
							{
								stringBuilder.Append("/TYPE=LeagueSharp ");
							}
							else if (service == ServiceType.Dota)
							{
								stringBuilder.Append("/TYPE=EnsageSharp ");
							}
							Process process = new Process();
							process.StartInfo.FileName = str;
							process.StartInfo.Arguments = stringBuilder.ToString();
							process.Start();
							this.Config.Value.Save(false);
							Environment.Exit(0);
							version = null;
						}
						else
						{
							UpdateService.Log.Warn(string.Format("{0} not found.", str));
							return;
						}
					}
					else
					{
						UpdateService.Log.Debug(string.Format("No update required [{0}] {1}|{2}", this.Config.Value.Settings.LoaderUpdateChannel, version, loaderVersionDatum.Version));
						return;
					}
				}
				catch (Exception exception)
				{
					UpdateService.Log.Error(exception);
				}
			}
			finally
			{
				Singleton<Eudyptula>.Instance.Stop();
			}
		}

		public async void UrlHandlerThread()
		{
			UpdateService.<UrlHandlerThread>d__61 variable = new UpdateService.<UrlHandlerThread>d__61();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<UpdateService.<UrlHandlerThread>d__61>(ref variable);
		}
	}
}