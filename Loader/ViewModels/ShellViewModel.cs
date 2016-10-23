using Caliburn.Micro;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Model.Message;
using Loader.Services;
using Loader.Services.Model;
using Loader.ViewModels.Model;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PlaySharp.Service.Messages;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Injector;
using PlaySharp.Toolkit.Messages;
using PlaySharp.Toolkit.Model;
using PlaySharp.Toolkit.Remoting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Loader.ViewModels
{
	[Export(typeof(IShellView))]
	public class ShellViewModel : PlaySharpConductorOneActive<IScreen>, IShellView, IConductor, IParent, INotifyPropertyChangedEx, INotifyPropertyChanged, PlaySharp.Toolkit.EventAggregator.IHandle<OnServiceChanged>, PlaySharp.Toolkit.EventAggregator.IHandle, PlaySharp.Toolkit.EventAggregator.IHandle<AccountData>, PlaySharp.Toolkit.EventAggregator.IHandle<OnShowMetroMessage>, PlaySharp.Toolkit.EventAggregator.IHandle<OnShowMessage>, PlaySharp.Toolkit.EventAggregator.IHandle<OnCloseApplication>, PlaySharp.Toolkit.EventAggregator.IHandle<OnCleanup>
	{
		private bool canUpdate = true;

		private string username;

		public bool CanUpdate
		{
			get
			{
				return this.canUpdate;
			}
			set
			{
				if (value == this.canUpdate)
				{
					return;
				}
				this.canUpdate = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ShellViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ShellViewModel).GetMethod("get_CanUpdate").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[Import(typeof(IDashboardView))]
		public Lazy<IDashboardView> Dashboard
		{
			get;
			set;
		}

		public override string DisplayName
		{
			get;
			set;
		}

		[Import(typeof(ITriggeredGameResolver))]
		public Lazy<ITriggeredGameResolver> GameResolver
		{
			get;
			set;
		}

		public string Header
		{
			get
			{
				return string.Format("{0} {1}", base.Config.Value.ServiceSettings.AppName, Files.Version);
			}
		}

		[Import(typeof(IInjector))]
		public Lazy<IInjector> InjectionService
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

		public ServiceHost Remoting
		{
			get;
			set;
		}

		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				if (value == this.username)
				{
					return;
				}
				this.username = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ShellViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ShellViewModel).GetMethod("get_Username").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public ShellViewModel()
		{
		}

		public void ActivateView<T>()
		where T : IScreen
		{
			this.ActivateItem(Caliburn.Micro.IoC.Get<T>(null));
		}

		public void Handle(AccountData message)
		{
			string text = "Normal";
			if (message.Account.IsSubscriber)
			{
				text = "Sub";
			}
			if (message.Account.IsBotter)
			{
				text = "Bot";
			}
			if (message.Account.IsDev)
			{
				text = "Dev";
			}
			this.Username = string.Format("{0}/{1}", message.Account.DisplayName, text);
		}

		public void Handle(OnServiceChanged message)
		{
			this.OnDeactivate(false);
			this.OnActivate();
		}

		public async void Handle(OnShowMetroMessage message)
		{
			if (!message.Handled)
			{
				message.Handled = true;
				OnShowMetroMessage onShowMetroMessage = message;
				MessageDialogResult messageDialogResult = await this.ShowMessageAsync(message.Title, message.Message.Replace("\\n", Environment.NewLine), message.Style);
				onShowMetroMessage.Result = messageDialogResult;
				onShowMetroMessage = null;
			}
		}

		public async void Handle(OnCloseApplication message)
		{
			await this.ShowMessageAsync("Closing", message.Reason, 0);
			Application.Current.Shutdown();
		}

		public async void Handle(OnShowMessage message)
		{
			if (!message.Handled)
			{
				if (message.GetType() != typeof(OnShowMetroMessage))
				{
					message.Handled = true;
					await this.ShowMessageAsync(message.Title, message.Message.Replace("\\n", Environment.NewLine), 0);
				}
			}
		}

		public void Handle(OnCleanup message)
		{
			IScreen[] array = (
				from s in base.Items
				where !s.IsActive
				select s).ToArray<IScreen>();
			for (int i = 0; i < (int)array.Length; i++)
			{
				IScreen screen = array[i];
				PlaySharpConductorOneActive<IScreen>.Log.Debug(string.Format("Close {0}", screen));
				this.DeactivateItem(screen, true);
			}
		}

		protected override async void OnActivate()
		{
			object obj1 = null;
			this.<>n__0();
			try
			{
				int num1 = 0;
				try
				{
					this.IsLoading = true;
					this.Refresh();
					Files.Invalidate();
					this.GameResolver.Value.Activate();
					if (this.Config.Value.Service == ServiceType.Dota)
					{
						Singleton<EnnolaService>.Instance.Start();
					}
					await this.UpdateService.Value.UpdateLoaderAsync(true);
					if (!await this.UpdateService.Value.UpdateCoreAsync())
					{
						await PathRandomizer.CopyFilesAsync();
					}
					await this.UpdateService.Value.UpdateEnsageTextures();
					this.UpdateService.Value.Activate();
					this.Config.Value.ServiceSettings.Profiles.GetOrCreate<LibrariesProfile>(null, true);
					if (this.Config.Value.ServiceSettings.Profiles.Count == 1)
					{
						ProfilesData profilesDatum = await this.ServiceClient.Value.DefaultProfilesAsync();
						AssemblyData assemblyDatum = await this.ServiceClient.Value.AssembliesAsync();
						foreach (ProfileEntry profile in profilesDatum.Profiles)
						{
							IProfile orCreate = this.Config.Value.ServiceSettings.Profiles.GetOrCreate<Profile>(profile.Name, true);
							List<AssemblyEntry> list = assemblyDatum.Assemblies.Where<AssemblyEntry>((AssemblyEntry a) => {
								if (!a.IsValid)
								{
									return false;
								}
								return profile.Assemblies.Contains<int>(a.Id);
							}).ToList<AssemblyEntry>();
							await this.UpdateService.Value.InstallAsync(list, orCreate);
						}
						profilesDatum = null;
						assemblyDatum = null;
					}
					await this.ServiceClient.Value.AccountAsync();
					await Task.Run(async () => {
						object obj = null;
						int num = 0;
						try
						{
							this.Remoting = FluentRemoting.ServerFactory().WithBaseEndpoint(string.Format("net.pipe://localhost/{0}/", HashFactory.MD5.String(Environment.UserName))).WithBinding(new NetNamedPipeBinding()).WithInstance(new LoaderService()).AddService<ILoaderService>().AutoOpen().Build();
						}
						catch (Exception exception)
						{
							obj = exception;
							num = 1;
						}
						if (num == 1)
						{
							Exception exception1 = (Exception)obj;
							PlaySharpConductorOneActive<IScreen>.Log.Error(exception1.ToString());
							await this.ShowMessageAsync("Fatal - Remoting", string.Format("Failed to open Remoting\n{0}", exception1.Message), 0);
							ServiceHost remoting = this.Remoting;
							if (remoting != null)
							{
								remoting.Abort();
							}
							else
							{
							}
						}
					});
					if (!this.Config.Value.Settings.UpdateSelectedAssembliesOnly)
					{
						await this.UpdateService.Value.PrepareAssembliesAsync(AssemblyUpdate.All, false);
					}
					else
					{
						await this.UpdateService.Value.PrepareAssembliesAsync(AssemblyUpdate.SelectedOnly | AssemblyUpdate.Libraries, false);
					}
					this.InjectionService.Value.Activate();
					this.Handle(await this.ServiceClient.Value.AccountAsync());
					this.OpenDashboard();
				}
				catch (Exception exception2)
				{
					obj1 = exception2;
					num1 = 1;
				}
				if (num1 == 1)
				{
					Exception exception3 = (Exception)obj1;
					PlaySharpConductorOneActive<IScreen>.Log.Fatal(exception3);
					await this.ShowMessageAsync("Fatal", exception3.ToString(), 0);
				}
				obj1 = null;
			}
			finally
			{
				this.IsLoading = false;
			}
		}

		protected override void OnDeactivate(bool close)
		{
			try
			{
				try
				{
					if (base.Config.Value.Service == ServiceType.Dota)
					{
						Singleton<EnnolaService>.Instance.Stop();
					}
					this.GameResolver.Value.Deactivate();
					base.UpdateService.Value.Deactivate();
					this.InjectionService.Value.Deactivate();
					ServiceHost remoting = this.Remoting;
					if (remoting != null)
					{
						remoting.Close(TimeSpan.FromSeconds(30));
					}
					else
					{
					}
					base.OnDeactivate(close);
				}
				catch (Exception exception)
				{
					PlaySharpConductorOneActive<IScreen>.Log.Error(exception);
				}
			}
			finally
			{
				if (close)
				{
					Application.Current.Shutdown();
				}
			}
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.Overlay.ActivateWith(this);
			try
			{
				if (!Utility.IsOnScreen(base.Config.Value.Settings.WindowLeft, base.Config.Value.Settings.WindowTop))
				{
					PlaySharpConductorOneActive<IScreen>.Log.Warn("Window out of screen");
					base.Config.Value.Settings.WindowLeft = 100;
					base.Config.Value.Settings.WindowTop = 100;
				}
				if (!InjectorWin32.Instance.EnableDebugPrivileges())
				{
					PlaySharpConductorOneActive<IScreen>.Log.Warn(string.Format("Failed to enable debug privileges for {0}", Process.GetCurrentProcess().Id));
				}
			}
			catch (Exception exception)
			{
				PlaySharpConductorOneActive<IScreen>.Log.Warn(exception);
			}
		}

		public void OpenDashboard()
		{
			this.ActivateView<IDashboardView>();
		}

		public void OpenDatabase()
		{
			this.ActivateView<IDatebaseView>();
		}

		public void OpenProfiles()
		{
			this.ActivateView<IProfilesView>();
		}

		public void OpenSettings()
		{
			this.ActivateView<ISettingsView>();
		}

		public async Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = 0)
		{
			MessageDialogResult messageDialogResult1 = 0;
			await Application.Current.Dispatcher.Invoke<Task>(async () => {
				MetroWindow mainWindow = Application.Current.MainWindow as MetroWindow;
				if (mainWindow != null)
				{
					mainWindow.Activate();
					ShellViewModel.<>c__DisplayClass45_0 variable = this;
					messageDialogResult1 = await DialogManager.ShowMessageAsync(mainWindow, title, message, style, null);
					variable = null;
				}
			});
			return messageDialogResult1;
		}

		public async void Update()
		{
			Dictionary<int, IGameInstance> instances = this.InjectionService.Value.Instances;
			if (!instances.Any<KeyValuePair<int, IGameInstance>>((KeyValuePair<int, IGameInstance> i) => {
				if (i.Value == null)
				{
					return false;
				}
				return i.Value.IsAlive;
			}))
			{
				await this.Overlay.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
					try
					{
						try
						{
							this.CanUpdate = false;
							this.ServiceClient.Value.ClearCache();
							MemoryManagement.FlushMemory();
							await this.UpdateService.Value.UpdateLoaderAsync(true);
							if (await this.UpdateService.Value.UpdateCoreAsync())
							{
								await PathRandomizer.CopyFilesAsync();
							}
							if (!this.Config.Value.Settings.UpdateSelectedAssembliesOnly)
							{
								await this.UpdateService.Value.PrepareAssembliesAsync(AssemblyUpdate.All, false);
							}
							else
							{
								await this.UpdateService.Value.PrepareAssembliesAsync(AssemblyUpdate.SelectedOnly | AssemblyUpdate.Libraries, false);
							}
							await this.Dashboard.Value.UpdateGameInfoAsync();
							await this.Dashboard.Value.UpdateAccountInfoAsync();
							await this.Dashboard.Value.UpdateCoreInfoAsync();
							await this.UpdateService.Value.UpdateEnsageTextures();
						}
						catch (Exception exception)
						{
							PlaySharpConductorOneActive<IScreen>.Log.Error(exception);
						}
					}
					finally
					{
						this.CanUpdate = true;
					}
				});
			}
			else
			{
				PlaySharpConductorOneActive<IScreen>.Log.Warn("Skip update while injected");
			}
		}
	}
}