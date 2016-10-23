using Caliburn.Micro;
using Ionic.Zip;
using Ionic.Zlib;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Properties;
using Loader.Services.Model;
using Loader.ViewModels.Model;
using log4net;
using Microsoft.Win32;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Messages;
using PlaySharp.Toolkit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Loader.ViewModels
{
	[Export(typeof(IDashboardView))]
	public class DashboardViewModel : PlaySharpScreen, IDashboardView, IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		private string accountColor = "#000000";

		private string accountInfo = "...";

		private string accountName = "...";

		private string avatar;

		private bool canUploadLogs = true;

		private BindableCollection<ChangelogEntry> coreChangelog = new BindableCollection<ChangelogEntry>();

		private string coreVersion = "...";

		private string gamelimit = "...";

		private string gameVersion = "...";

		private BindableCollection<ChangelogEntry> loaderChangelog = new BindableCollection<ChangelogEntry>();

		private IServiceSettings serviceSettings;

		private ISettings settings;

		public string AccountColor
		{
			get
			{
				return this.accountColor;
			}
			set
			{
				if (value == this.accountColor)
				{
					return;
				}
				this.accountColor = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_AccountColor").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string AccountInfo
		{
			get
			{
				return this.accountInfo;
			}
			set
			{
				if (value == this.accountInfo)
				{
					return;
				}
				this.accountInfo = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_AccountInfo").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string AccountName
		{
			get
			{
				return this.accountName;
			}
			set
			{
				if (value == this.accountName)
				{
					return;
				}
				this.accountName = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_AccountName").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Avatar
		{
			get
			{
				return this.avatar;
			}
			set
			{
				if (value == this.avatar)
				{
					return;
				}
				this.avatar = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_Avatar").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool CanImportConfig { get; set; } = true;

		public bool CanUploadLogs
		{
			get
			{
				return this.canUploadLogs;
			}
			set
			{
				if (value == this.canUploadLogs)
				{
					return;
				}
				this.canUploadLogs = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_CanUploadLogs").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public BindableCollection<ChangelogEntry> CoreChangelog
		{
			get
			{
				return this.coreChangelog;
			}
			set
			{
				if (object.Equals(value, this.coreChangelog))
				{
					return;
				}
				this.coreChangelog = value;
				base.NotifyOfPropertyChange<BindableCollection<ChangelogEntry>>(System.Linq.Expressions.Expression.Lambda<Func<BindableCollection<ChangelogEntry>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_CoreChangelog").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string CoreVersion
		{
			get
			{
				return this.coreVersion;
			}
			set
			{
				if (value == this.coreVersion)
				{
					return;
				}
				this.coreVersion = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_CoreVersion").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Gamelimit
		{
			get
			{
				return this.gamelimit;
			}
			set
			{
				if (value == this.gamelimit)
				{
					return;
				}
				this.gamelimit = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_Gamelimit").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string GameVersion
		{
			get
			{
				return this.gameVersion;
			}
			set
			{
				if (value == this.gameVersion)
				{
					return;
				}
				this.gameVersion = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_GameVersion").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public BindableCollection<ChangelogEntry> LoaderChangelog
		{
			get
			{
				return this.loaderChangelog;
			}
			set
			{
				if (object.Equals(value, this.loaderChangelog))
				{
					return;
				}
				this.loaderChangelog = value;
				base.NotifyOfPropertyChange<BindableCollection<ChangelogEntry>>(System.Linq.Expressions.Expression.Lambda<Func<BindableCollection<ChangelogEntry>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_LoaderChangelog").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[Import(typeof(IBusyOverlayView))]
		public IBusyOverlayView Overlay
		{
			get;
			set;
		}

		public IServiceSettings ServiceSettings
		{
			get
			{
				return this.serviceSettings;
			}
			set
			{
				if (object.Equals(value, this.serviceSettings))
				{
					return;
				}
				this.serviceSettings = value;
				base.NotifyOfPropertyChange<IServiceSettings>(System.Linq.Expressions.Expression.Lambda<Func<IServiceSettings>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_ServiceSettings").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public ISettings Settings
		{
			get
			{
				return this.settings;
			}
			set
			{
				if (object.Equals(value, this.settings))
				{
					return;
				}
				this.settings = value;
				base.NotifyOfPropertyChange<ISettings>(System.Linq.Expressions.Expression.Lambda<Func<ISettings>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(DashboardViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DashboardViewModel).GetMethod("get_Settings").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public DashboardViewModel()
		{
		}

		public string CreateBugreport()
		{
			string str;
			try
			{
				string store = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VirtualStore");
				if (Directory.Exists(store))
				{
					try
					{
						Extensions.SetFullAccessControl(store);
					}
					catch (Exception exception)
					{
						PlaySharpScreen.Log.Warn(exception);
					}
					foreach (FileInfo file in 
						from f in Directory.EnumerateFiles(store, "*.log", SearchOption.AllDirectories)
						select new FileInfo(f))
					{
						try
						{
							if (file.Name.Contains("Sandbox"))
							{
								string dest = Path.Combine(Directories.Logs, file.Name);
								file.MoveTo(dest);
								file.Attributes = FileAttributes.Normal;
							}
						}
						catch (Exception exception1)
						{
							PlaySharpScreen.Log.Warn(exception1);
						}
					}
				}
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("=== Config");
				sb.AppendLine(string.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version));
				sb.AppendLine(string.Format("Service {0}", base.Config.Value.Service));
				sb.AppendLine(string.Format("Language {0}", base.Config.Value.Settings.Language));
				sb.AppendLine(string.Format("AssemblyDebug {0}", base.Config.Value.Settings.AssemblyDebug));
				sb.AppendLine(string.Format("TosAccepted {0}", base.Config.Value.Settings.TosAccepted));
				sb.AppendLine(string.Format("UseSandbox {0}", base.Config.Value.Settings.UseSandbox));
				sb.AppendLine(string.Format("Workers {0}", base.Config.Value.Settings.Workers));
				sb.AppendLine(string.Format("GameFilePath {0}", base.Config.Value.ServiceSettings.GameFilePath));
				sb.AppendLine();
				sb.AppendLine("=== Assemblies");
				foreach (IPlaySharpAssembly assembly in base.Config.Value.ServiceSettings.Assemblies)
				{
					sb.AppendLine(string.Format("[{0}-{1}] {2}", assembly.Id, assembly.VersionString, assembly.Name));
				}
				sb.AppendLine();
				sb.AppendLine("=== Profiles");
				foreach (IProfile profile in base.Config.Value.ServiceSettings.Profiles)
				{
					sb.AppendLine(string.Format("[{0}] {1}", profile.Name, profile.Inject));
					foreach (IProfileAssembly assembly in profile.Assemblies)
					{
						try
						{
							IPlaySharpAssembly realAssembly = assembly.GetAssembly();
							sb.AppendLine(string.Format("[{0}-{1}] {2} {3}", new object[] { realAssembly.Id, realAssembly.VersionString, realAssembly.Name, assembly.Inject }));
						}
						catch (Exception exception2)
						{
							sb.AppendLine(exception2.Message);
						}
					}
					sb.AppendLine();
				}
				sb.AppendLine("=== Hotkeys");
				foreach (IHotkeyEntry hotkey in base.Config.Value.ServiceSettings.Hotkeys)
				{
					sb.AppendLine(string.Format("{0} {1}", hotkey.Name, hotkey.HotkeyString));
				}
				sb.AppendLine();
				sb.AppendLine("=== Game Settings");
				foreach (IGameSettingEntry setting in base.Config.Value.ServiceSettings.GameSettings)
				{
					sb.AppendLine(string.Format("{0} {1}", setting.Name, setting.SelectedValue));
				}
				sb.AppendLine();
				sb.AppendLine(string.Format("=== Files {0}", Directories.Core));
				foreach (FileInfo file in 
					from f in Directory.EnumerateFiles(Directories.Core)
					select new FileInfo(f))
				{
					sb.Append(string.Format("{0}", file.Name));
					if (file.Extension == ".dll" || file.Extension == ".exe")
					{
						try
						{
							AssemblyName assembly = AssemblyName.GetAssemblyName(file.FullName);
							sb.Append(string.Format(" {0}", assembly.FullName));
						}
						catch
						{
						}
					}
					sb.AppendLine();
				}
				sb.AppendLine();
				sb.AppendLine(string.Format("=== Files {0}", Directories.References));
				foreach (FileInfo file in 
					from f in Directory.EnumerateFiles(Directories.References)
					select new FileInfo(f))
				{
					sb.Append(string.Format("{0}", file.Name));
					if (file.Extension == ".dll" || file.Extension == ".exe")
					{
						try
						{
							AssemblyName assembly = AssemblyName.GetAssemblyName(file.FullName);
							sb.Append(string.Format(" {0}", assembly.FullName));
						}
						catch
						{
						}
					}
					sb.AppendLine();
				}
				sb.AppendLine();
				sb.AppendLine(string.Format("=== Files {0}", Directories.System));
				foreach (FileInfo file in 
					from f in Directory.EnumerateFiles(Directories.System)
					select new FileInfo(f))
				{
					sb.Append(string.Format("{0}", file.Name));
					if (file.Extension == ".dll" || file.Extension == ".exe")
					{
						try
						{
							AssemblyName assembly = AssemblyName.GetAssemblyName(file.FullName);
							sb.Append(string.Format(" {0}", assembly.FullName));
						}
						catch
						{
						}
					}
					sb.AppendLine();
				}
				sb.AppendLine();
				sb.AppendLine(string.Format("=== Files {0}", Directories.Assemblies));
				foreach (FileInfo file in 
					from f in Directory.EnumerateFiles(Directories.Assemblies)
					select new FileInfo(f))
				{
					sb.Append(string.Format("{0}", file.Name));
					if (file.Extension == ".dll" || file.Extension == ".exe")
					{
						try
						{
							AssemblyName assembly = AssemblyName.GetAssemblyName(file.FullName);
							sb.Append(string.Format(" {0}", assembly.FullName));
						}
						catch
						{
						}
					}
					sb.AppendLine();
				}
				sb.AppendLine();
				str = sb.ToString();
			}
			catch (Exception exception3)
			{
				PlaySharpScreen.Log.Error(exception3);
				str = null;
			}
			return str;
		}

		public async Task ImportConfig()
		{
			try
			{
				try
				{
					this.CanImportConfig = false;
					OpenFileDialog openFileDialog = new OpenFileDialog()
					{
						Filter = "LeagueSharp Config (config.xml)|config.xml"
					};
					bool? nullable = openFileDialog.ShowDialog();
					if ((nullable.GetValueOrDefault() ? nullable.HasValue : false))
					{
						await this.Config.Value.Import(openFileDialog.FileName);
					}
					else
					{
						return;
					}
				}
				catch (Exception exception)
				{
					PlaySharpScreen.Log.Error(exception);
				}
			}
			finally
			{
				this.CanImportConfig = true;
			}
		}

		private async Task LegacyConfigImport()
		{
			try
			{
				string str = Path.Combine(Directories.AppData, "config.xml");
				if (File.Exists(str))
				{
					await this.Config.Value.Import(str);
					str.DeleteFile();
				}
				str = null;
			}
			catch (Exception exception)
			{
				PlaySharpScreen.Log.Error(exception);
			}
		}

		protected override async void OnActivate()
		{
			this.<>n__0();
			await this.LegacyConfigImport();
			this.UpdateService.Value.UpdateGamePath();
			await this.UpdateChangelogAsync();
			await this.UpdateAccountInfoAsync();
			await this.UpdateGameInfoAsync();
			await this.UpdateCoreInfoAsync();
			this.Settings = this.Config.Value.Settings;
			this.ServiceSettings = this.Config.Value.ServiceSettings;
		}

		public async Task UpdateAccountInfoAsync()
		{
			try
			{
				Account account = await this.ServiceClient.Value.AccountAsync().Account;
				string str = "Normal";
				if (account.IsSubscriber)
				{
					str = (account.SubscriptionEnd <= DateTime.UtcNow ? "Subscriber" : string.Format("Subscriber {0}", account.SubscriptionEnd));
				}
				if (account.IsBotter)
				{
					str = (account.SubscriptionEnd <= DateTime.UtcNow ? "Botter" : string.Format("Botter {0}", account.SubscriptionEnd));
				}
				if (account.IsDev)
				{
					str = "Developer";
				}
				if (account.Id == 129)
				{
					str = "Special Member";
				}
				this.AccountInfo = str;
				this.AccountName = account.DisplayName;
				this.AccountColor = account.GroupColor;
				this.Avatar = account.Avatar;
				if (account.MaxGames != 0)
				{
					this.Gamelimit = string.Format("{0} / {1}", account.GamesPlayed, account.MaxGames);
				}
				else
				{
					this.Gamelimit = "Unlimited";
				}
			}
			catch (Exception exception)
			{
				PlaySharpScreen.Log.Error(exception);
			}
		}

		private async Task UpdateChangelogAsync()
		{
			DashboardViewModel.<UpdateChangelogAsync>d__64 variable = new DashboardViewModel.<UpdateChangelogAsync>d__64();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<DashboardViewModel.<UpdateChangelogAsync>d__64>(ref variable);
			return variable.<>t__builder.Task;
		}

		public async Task UpdateCoreInfoAsync()
		{
			DashboardViewModel.<UpdateCoreInfoAsync>d__59 variable = new DashboardViewModel.<UpdateCoreInfoAsync>d__59();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<DashboardViewModel.<UpdateCoreInfoAsync>d__59>(ref variable);
			return variable.<>t__builder.Task;
		}

		public async Task UpdateGameInfoAsync()
		{
			string fullName;
			try
			{
				if (string.IsNullOrEmpty(this.Config.Value.ServiceSettings.GameFilePath))
				{
					this.GameVersion = "unknown";
					return;
				}
				else if (File.Exists(this.Config.Value.ServiceSettings.GameFilePath))
				{
					ServiceType service = this.Config.Value.Service;
					if (service == ServiceType.LoL)
					{
						FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(this.Config.Value.ServiceSettings.GameFilePath);
						this.GameVersion = versionInfo.FileVersion;
					}
					else if (service == ServiceType.Dota)
					{
						try
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
							Dictionary<string, string> strs = Utility.ParseInf(Path.Combine(fullName, "dota", "steam.inf"));
							this.GameVersion = string.Format("{0}", strs["ClientVersion"]);
						}
						catch (Exception exception)
						{
							PlaySharpScreen.Log.Error(exception);
						}
					}
				}
				else
				{
					this.GameVersion = "unknown";
					return;
				}
			}
			catch (Exception exception1)
			{
				PlaySharpScreen.Log.Error(exception1);
			}
		}

		public async void UploadLogs()
		{
			DashboardViewModel.<UploadLogs>d__61 variable = new DashboardViewModel.<UploadLogs>d__61();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<DashboardViewModel.<UploadLogs>d__61>(ref variable);
		}
	}
}