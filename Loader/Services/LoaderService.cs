using Loader.Model;
using Loader.Model.Config;
using Loader.Services.Model;
using Loader.Shared;
using log4net;
using Microsoft.VisualStudio.Threading;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.AppDomain.Messages;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Loader.Services
{
	[ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Reentrant, InstanceContextMode=InstanceContextMode.Single, AutomaticSessionShutdown=true)]
	public class LoaderService : ILoaderService
	{
		private readonly static ILog Log;

		[Import(typeof(PlaySharpConfig))]
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

		public int LastUpdate
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

		[Import(typeof(IUpdateService))]
		public Lazy<IUpdateService> UpdateService
		{
			get;
			set;
		}

		static LoaderService()
		{
			LoaderService.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public LoaderService()
		{
			IoC.BuildUp(this);
		}

		public List<LSharpAssembly> GetAssemblyList()
		{
			ThreadHelper.JoinableTaskFactory.Run(async () => await this.UpdateService.Value.PrepareAssembliesAsync(AssemblyUpdate.SelectedOnly | AssemblyUpdate.IgnoreLocalUpdate, false));
			LoaderService.Log.Debug("Send Assemblies");
			List<LSharpAssembly> assemblies = new List<LSharpAssembly>();
			try
			{
				foreach (IProfile profile in 
					from p in this.Config.Value.ServiceSettings.Profiles
					where p.Inject
					select p)
				{
					assemblies.AddRange(
						from a in profile.Assemblies
						where a.Inject
						select a.GetAssembly() into a
						where a.Type != AssemblyType.Library
						select new LSharpAssembly()
						{
							Id = a.Id,
							Name = a.Name,
							PathToBinary = a.PathToBinary,
							Type = (int)a.Type
						});
				}
				foreach (IPlaySharpAssembly assembly in 
					from a in this.Config.Value.ServiceSettings.Assemblies
					where a.Type == AssemblyType.Library
					select a)
				{
					assemblies.Add(new LSharpAssembly()
					{
						Id = assembly.Id,
						Name = assembly.Name,
						PathToBinary = assembly.PathToBinary,
						Type = (int)assembly.Type
					});
				}
				ServiceType service = this.Config.Value.Service;
				if (service == ServiceType.LoL)
				{
					foreach (string file in Directory.EnumerateFiles(Directories.Plugins, "*LeagueSharp*.dll"))
					{
						try
						{
							assemblies.Add(new LSharpAssembly()
							{
								Id = 0,
								Name = Path.GetFileNameWithoutExtension(file),
								PathToBinary = file,
								Type = 3
							});
						}
						catch (Exception exception)
						{
							LoaderService.Log.Error(exception);
						}
					}
				}
				else if (service == ServiceType.Dota)
				{
					foreach (string file in Directory.EnumerateFiles(Directories.Plugins, "*EnsageSharp*.dll"))
					{
						try
						{
							assemblies.Add(new LSharpAssembly()
							{
								Id = 0,
								Name = Path.GetFileNameWithoutExtension(file),
								PathToBinary = file,
								Type = 3
							});
						}
						catch (Exception exception1)
						{
							LoaderService.Log.Error(exception1);
						}
					}
				}
			}
			catch (Exception exception2)
			{
				LoaderService.Log.Error(exception2);
			}
			foreach (LSharpAssembly assembly in assemblies)
			{
				LoaderService.Log.Debug(string.Format("Assembly [{0}] {1}", assembly.Id, assembly.Name));
			}
			return assemblies;
		}

		public Configuration GetConfiguration()
		{
			Configuration configuration;
			string fullName;
			string str;
			LoaderService.Log.Debug("Send Configuration");
			try
			{
				Configuration directoryName = new Configuration()
				{
					LoaderProcessId = Process.GetCurrentProcess().Id,
					LoaderDirectory = Directories.Current,
					AppDataDirectory = Directories.AppData,
					ConfigDirectory = Directories.Config,
					CacheDirectory = Directories.Assemblies,
					Language = this.Config.Value.Settings.Language,
					UseSandbox = this.Config.Value.Settings.UseSandbox,
					LogDirectory = Directories.Logs,
					CoreBridgeFilePath = Files.Randomized.CoreBridge.FullName
				};
				try
				{
					directoryName.GameDirectory = Path.GetDirectoryName(this.Config.Value.ServiceSettings.GameFilePath);
					ServiceType service = this.Config.Value.Service;
					if (service == ServiceType.LoL)
					{
						Configuration configuration1 = directoryName;
						DirectoryInfo parent = (new DirectoryInfo(directoryName.GameDirectory)).Parent;
						if (parent != null)
						{
							DirectoryInfo directoryInfo = parent.Parent;
							if (directoryInfo != null)
							{
								DirectoryInfo parent1 = directoryInfo.Parent;
								if (parent1 != null)
								{
									DirectoryInfo directoryInfo1 = parent1.Parent;
									if (directoryInfo1 != null)
									{
										DirectoryInfo parent2 = directoryInfo1.Parent;
										if (parent2 != null)
										{
											DirectoryInfo directoryInfo2 = parent2.Parent;
											if (directoryInfo2 != null)
											{
												fullName = directoryInfo2.FullName;
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
						configuration1.GameDirectory = fullName;
					}
					else if (service == ServiceType.Dota)
					{
						Configuration configuration2 = directoryName;
						DirectoryInfo parent3 = (new DirectoryInfo(directoryName.GameDirectory)).Parent;
						if (parent3 != null)
						{
							DirectoryInfo directoryInfo3 = parent3.Parent;
							if (directoryInfo3 != null)
							{
								DirectoryInfo parent4 = directoryInfo3.Parent;
								if (parent4 != null)
								{
									str = parent4.FullName;
								}
								else
								{
									str = null;
								}
							}
							else
							{
								str = null;
							}
						}
						else
						{
							str = null;
						}
						configuration2.GameDirectory = str;
					}
				}
				catch (Exception exception)
				{
					LoaderService.Log.Warn(exception);
				}
				foreach (IHotkeyEntry hotkey in this.Config.Value.ServiceSettings.Hotkeys)
				{
					LoaderService.Log.Debug(string.Format("Hotkey [{0}] {1}", hotkey.Name, hotkey.Hotkey));
					directoryName.HotKeys[hotkey.Name] = KeyInterop.VirtualKeyFromKey(hotkey.Hotkey);
				}
				foreach (IGameSettingEntry setting in this.Config.Value.ServiceSettings.GameSettings)
				{
					LoaderService.Log.Debug(string.Format("Game Setting [{0}] {1}", setting.Name, setting.SelectedValue));
					directoryName.GameSettings[setting.Name] = setting.SelectedValue;
				}
				ThreadHelper.JoinableTaskFactory.Run(async () => {
					LoaderService.<>c__DisplayClass23_0.<<GetConfiguration>b__0>d _ = new LoaderService.<>c__DisplayClass23_0.<<GetConfiguration>b__0>d();
					_.<>4__this = this;
					_.<>t__builder = AsyncTaskMethodBuilder.Create();
					_.<>1__state = -1;
					_.<>t__builder.Start<LoaderService.<>c__DisplayClass23_0.<<GetConfiguration>b__0>d>(ref _);
					return _.<>t__builder.Task;
				});
				directoryName.Settings["ServiceToken"] = this.ServiceClient.Value.LoginData.Token;
				configuration = directoryName;
			}
			catch (Exception exception1)
			{
				LoaderService.Log.Error(exception1);
				return null;
			}
			return configuration;
		}

		public void Recompile()
		{
			LoaderService.Log.Debug("Recompile Assemblies");
			ThreadHelper.JoinableTaskFactory.Run(async () => {
				try
				{
					await this.UpdateService.Value.PrepareAssembliesAsync(AssemblyUpdate.SelectedOnly, true);
				}
				catch (Exception exception)
				{
					LoaderService.Log.Error(exception);
				}
			});
		}

		public void SetUnitName(string name)
		{
			EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnGameUnitResolved(name));
		}
	}
}