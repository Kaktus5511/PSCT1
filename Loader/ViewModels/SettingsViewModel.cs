using Caliburn.Micro;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Model.Message;
using Loader.Services;
using Loader.Services.Model;
using Loader.ViewModels.Model;
using log4net;
using MahApps.Metro;
using PlaySharp.Service.Messages;
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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFLocalizeExtension.Engine;

namespace Loader.ViewModels
{
	[Export(typeof(ISettingsView))]
	public class SettingsViewModel : PlaySharpScreen, ISettingsView, IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		private BindableCollection<EndpointEntry> apiEndpoints = new BindableCollection<EndpointEntry>();

		private bool canClearCache = true;

		private BindableCollection<Accent> colors = new BindableCollection<Accent>();

		private BindableCollection<EndpointEntry> dataEndpoints = new BindableCollection<EndpointEntry>();

		private IReadOnlyList<HealthEntry> health = new HealthEntry[0];

		private BindableCollection<CultureInfo> languages = new BindableCollection<CultureInfo>();

		private BindableCollection<ReleaseChannel> releaseChannels = new BindableCollection<ReleaseChannel>();

		private CultureInfo selectedLanguage;

		private BindableCollection<PlaySharp.Toolkit.Model.ServiceType> serviceTypes = new BindableCollection<PlaySharp.Toolkit.Model.ServiceType>();

		private BindableCollection<AppTheme> themes = new BindableCollection<AppTheme>();

		private BindableCollection<Loader.Model.UserType> userTypes = new BindableCollection<Loader.Model.UserType>();

		public BindableCollection<EndpointEntry> ApiEndpoints
		{
			get
			{
				return this.apiEndpoints;
			}
			set
			{
				if (object.Equals(value, this.apiEndpoints))
				{
					return;
				}
				this.apiEndpoints = value;
				base.NotifyOfPropertyChange<BindableCollection<EndpointEntry>>(System.Linq.Expressions.Expression.Lambda<Func<BindableCollection<EndpointEntry>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_ApiEndpoints").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool AutoAccept
		{
			get
			{
				return this.GetOrCreateGameSetting("AutoAccept", "False").SelectedValue == "True";
			}
			set
			{
				this.GetOrCreateGameSetting("AutoAccept", "False").SelectedValue = (value ? "True" : "False");
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_AutoAccept").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool CanClearCache
		{
			get
			{
				return this.canClearCache;
			}
			set
			{
				if (value == this.canClearCache)
				{
					return;
				}
				this.canClearCache = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_CanClearCache").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public BindableCollection<Accent> Colors
		{
			get
			{
				return this.colors;
			}
			set
			{
				if (object.Equals(value, this.colors))
				{
					return;
				}
				this.colors = value;
				base.NotifyOfPropertyChange<BindableCollection<Accent>>(System.Linq.Expressions.Expression.Lambda<Func<BindableCollection<Accent>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Colors").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool Console
		{
			get
			{
				return this.GetOrCreateGameSetting("Console", "True").SelectedValue == "True";
			}
			set
			{
				this.GetOrCreateGameSetting("Console", "False").SelectedValue = (value ? "True" : "False");
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Console").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public BindableCollection<EndpointEntry> DataEndpoints
		{
			get
			{
				return this.dataEndpoints;
			}
			set
			{
				if (object.Equals(value, this.dataEndpoints))
				{
					return;
				}
				this.dataEndpoints = value;
				base.NotifyOfPropertyChange<BindableCollection<EndpointEntry>>(System.Linq.Expressions.Expression.Lambda<Func<BindableCollection<EndpointEntry>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_DataEndpoints").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool DisableDrawings
		{
			get
			{
				return this.GetOrCreateGameSetting("DisableDrawings", "False").SelectedValue == "True";
			}
			set
			{
				this.GetOrCreateGameSetting("DisableDrawings", "False").SelectedValue = (value ? "True" : "False");
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_DisableDrawings").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool DisableKillswitch
		{
			get
			{
				return this.GetOrCreateGameSetting("DisableKillswitch", "0").SelectedValue == "1";
			}
			set
			{
				if (value)
				{
					PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(base.EventAggregator.Value, new OnShowMessage("Ennola", Loc.GetValue("Message.EnnolaKillswitch.Body")));
				}
				this.GetOrCreateGameSetting("DisableKillswitch", "False").SelectedValue = (value ? "1" : "0");
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_DisableKillswitch").MethodHandle)), new ParameterExpression[0]));
				Singleton<EnnolaService>.Instance.UpdateConfig();
			}
		}

		public IReadOnlyList<HealthEntry> Health
		{
			get
			{
				return this.health;
			}
			set
			{
				if (object.Equals(value, this.health))
				{
					return;
				}
				this.health = value;
				base.NotifyOfPropertyChange<IReadOnlyList<HealthEntry>>(System.Linq.Expressions.Expression.Lambda<Func<IReadOnlyList<HealthEntry>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Health").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool IsDotaActive
		{
			get
			{
				return this.ServiceType == PlaySharp.Toolkit.Model.ServiceType.Dota;
			}
		}

		public bool IsLeagueActive
		{
			get
			{
				return this.ServiceType == PlaySharp.Toolkit.Model.ServiceType.LoL;
			}
		}

		public BindableCollection<CultureInfo> Languages
		{
			get
			{
				return this.languages;
			}
			set
			{
				if (object.Equals(value, this.languages))
				{
					return;
				}
				this.languages = value;
				base.NotifyOfPropertyChange<BindableCollection<CultureInfo>>(System.Linq.Expressions.Expression.Lambda<Func<BindableCollection<CultureInfo>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Languages").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public ReleaseChannel LoaderUpdateChannel
		{
			get
			{
				return base.Config.Value.Settings.LoaderUpdateChannel;
			}
			set
			{
				if (value == base.Config.Value.Settings.LoaderUpdateChannel)
				{
					return;
				}
				base.Config.Value.Settings.LoaderUpdateChannel = value;
				base.NotifyOfPropertyChange<ReleaseChannel>(System.Linq.Expressions.Expression.Lambda<Func<ReleaseChannel>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_LoaderUpdateChannel").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public Key Menu
		{
			get
			{
				return this.GetOrCreateHotkey("Menu", Key.LeftShift).Hotkey;
			}
			set
			{
				this.GetOrCreateHotkey("Menu", Key.System).Hotkey = value;
				base.NotifyOfPropertyChange<Key>(System.Linq.Expressions.Expression.Lambda<Func<Key>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Menu").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public Key MenuToggle
		{
			get
			{
				return this.GetOrCreateHotkey("MenuToggle", Key.F9).Hotkey;
			}
			set
			{
				this.GetOrCreateHotkey("MenuToggle", Key.System).Hotkey = value;
				base.NotifyOfPropertyChange<Key>(System.Linq.Expressions.Expression.Lambda<Func<Key>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_MenuToggle").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[Import(typeof(IBusyOverlayView))]
		public IBusyOverlayView Overlay
		{
			get;
			set;
		}

		public Key Reload
		{
			get
			{
				return this.GetOrCreateHotkey("Reload", Key.F5).Hotkey;
			}
			set
			{
				this.GetOrCreateHotkey("Reload", Key.System).Hotkey = value;
				base.NotifyOfPropertyChange<Key>(System.Linq.Expressions.Expression.Lambda<Func<Key>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Reload").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public Key ReloadAndRecompile
		{
			get
			{
				return this.GetOrCreateHotkey("ReloadAndRecompile", Key.F8).Hotkey;
			}
			set
			{
				this.GetOrCreateHotkey("ReloadAndRecompile", Key.System).Hotkey = value;
				base.NotifyOfPropertyChange<Key>(System.Linq.Expressions.Expression.Lambda<Func<Key>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_ReloadAndRecompile").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public EndpointEntry SelectedApiEndpoint
		{
			get
			{
				return base.Config.Value.Settings.ApiEndpoint;
			}
			set
			{
				if (object.Equals(value, base.Config.Value.Settings.ApiEndpoint))
				{
					return;
				}
				base.ServiceClient.Value.UseEndpoint(EndpointType.API, value);
				base.Config.Value.Settings.ApiEndpoint = value;
				base.ServiceClient.Value.ClearCache();
				base.NotifyOfPropertyChange<EndpointEntry>(System.Linq.Expressions.Expression.Lambda<Func<EndpointEntry>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_SelectedApiEndpoint").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public Accent SelectedColor
		{
			get
			{
				if (Caliburn.Micro.Execute.InDesignMode)
				{
					return ThemeManager.GetAccent("Red");
				}
				return ThemeManager.GetAccent(base.Config.Value.Settings.Color);
			}
			set
			{
				if (value.get_Name() == base.Config.Value.Settings.Color)
				{
					return;
				}
				Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
				ThemeManager.ChangeAppStyle(Application.Current, value, appStyle.Item1);
				base.Config.Value.Settings.Color = value.get_Name();
				base.NotifyOfPropertyChange<Accent>(System.Linq.Expressions.Expression.Lambda<Func<Accent>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_SelectedColor").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public EndpointEntry SelectedDataEndpoint
		{
			get
			{
				return base.Config.Value.Settings.DataEndpoint;
			}
			set
			{
				if (object.Equals(value, base.Config.Value.Settings.DataEndpoint))
				{
					return;
				}
				base.ServiceClient.Value.UseEndpoint(EndpointType.DATA, value);
				base.Config.Value.Settings.DataEndpoint = value;
				base.ServiceClient.Value.ClearCache();
				base.NotifyOfPropertyChange<EndpointEntry>(System.Linq.Expressions.Expression.Lambda<Func<EndpointEntry>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_SelectedDataEndpoint").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public CultureInfo SelectedLanguage
		{
			get
			{
				return this.selectedLanguage;
			}
			set
			{
				if (object.Equals(value, this.selectedLanguage))
				{
					return;
				}
				LocalizeDictionary.get_Instance().set_Culture(value);
				base.ServiceClient.Value.Language = value;
				this.selectedLanguage = value;
				base.NotifyOfPropertyChange<CultureInfo>(System.Linq.Expressions.Expression.Lambda<Func<CultureInfo>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_SelectedLanguage").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public AppTheme SelectedTheme
		{
			get
			{
				return ThemeManager.GetAppTheme(base.Config.Value.Settings.Theme);
			}
			set
			{
				if (value.get_Name() == base.Config.Value.Settings.Theme)
				{
					return;
				}
				Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
				ThemeManager.ChangeAppStyle(Application.Current, appStyle.Item2, value);
				base.Config.Value.Settings.Theme = value.get_Name();
				base.NotifyOfPropertyChange<AppTheme>(System.Linq.Expressions.Expression.Lambda<Func<AppTheme>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_SelectedTheme").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public PlaySharp.Toolkit.Model.ServiceType ServiceType
		{
			get
			{
				return base.Config.Value.Service;
			}
			set
			{
				if (value == base.Config.Value.Service)
				{
					return;
				}
				if (!Environment.Is64BitOperatingSystem && value == PlaySharp.Toolkit.Model.ServiceType.Dota)
				{
					PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(base.EventAggregator.Value, new OnShowMessage("Invalid Service", string.Format("{0} is for 64bit Operating System only!", value)));
					return;
				}
				base.Config.Value.Save(this.UseCloud);
				base.ServiceClient.Value.ServiceType = value;
				base.Config.Value.Service = value;
				base.Config.Value.Load(this.UseCloud);
				base.Config.Value.Save(false);
				PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(base.EventAggregator.Value, new OnServiceChanged(value));
				base.NotifyOfPropertyChange<PlaySharp.Toolkit.Model.ServiceType>(System.Linq.Expressions.Expression.Lambda<Func<PlaySharp.Toolkit.Model.ServiceType>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_ServiceType").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool ShowDescription
		{
			get
			{
				return base.Config.Value.Settings.ShowDescription;
			}
			set
			{
				if (value == base.Config.Value.Settings.ShowDescription)
				{
					return;
				}
				base.Config.Value.Settings.ShowDescription = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_ShowDescription").MethodHandle)), new ParameterExpression[0]));
				PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(base.EventAggregator.Value, new OnCleanup());
			}
		}

		public bool ShowNotes
		{
			get
			{
				return base.Config.Value.Settings.ShowNotes;
			}
			set
			{
				if (value == base.Config.Value.Settings.ShowNotes)
				{
					return;
				}
				base.Config.Value.Settings.ShowNotes = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_ShowNotes").MethodHandle)), new ParameterExpression[0]));
				PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(base.EventAggregator.Value, new OnCleanup());
			}
		}

		public BindableCollection<AppTheme> Themes
		{
			get
			{
				return this.themes;
			}
			set
			{
				if (object.Equals(value, this.themes))
				{
					return;
				}
				this.themes = value;
				base.NotifyOfPropertyChange<BindableCollection<AppTheme>>(System.Linq.Expressions.Expression.Lambda<Func<BindableCollection<AppTheme>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Themes").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public Key Unload
		{
			get
			{
				return this.GetOrCreateHotkey("Unload", Key.F6).Hotkey;
			}
			set
			{
				this.GetOrCreateHotkey("Unload", Key.System).Hotkey = value;
				base.NotifyOfPropertyChange<Key>(System.Linq.Expressions.Expression.Lambda<Func<Key>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Unload").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool UpdateSelectedAssembliesOnly
		{
			get
			{
				return base.Config.Value.Settings.UpdateSelectedAssembliesOnly;
			}
			set
			{
				if (value == base.Config.Value.Settings.UpdateSelectedAssembliesOnly)
				{
					return;
				}
				base.Config.Value.Settings.UpdateSelectedAssembliesOnly = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_UpdateSelectedAssembliesOnly").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool UseCloud
		{
			get
			{
				return base.Config.Value.UseCloud;
			}
			set
			{
				if (value == base.Config.Value.UseCloud)
				{
					return;
				}
				base.Config.Value.UseCloud = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_UseCloud").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public Loader.Model.UserType UserType
		{
			get
			{
				return base.Config.Value.Settings.UserType;
			}
			set
			{
				if (value == base.Config.Value.Settings.UserType)
				{
					return;
				}
				base.Config.Value.Settings.UserType = value;
				base.NotifyOfPropertyChange<Loader.Model.UserType>(System.Linq.Expressions.Expression.Lambda<Func<Loader.Model.UserType>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_UserType").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool UseSandbox
		{
			get
			{
				return base.Config.Value.Settings.UseSandbox;
			}
			set
			{
				if (value == base.Config.Value.Settings.UseSandbox)
				{
					return;
				}
				base.Config.Value.Settings.UseSandbox = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_UseSandbox").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public int Workers
		{
			get
			{
				return base.Config.Value.Settings.Workers;
			}
			set
			{
				if (value == base.Config.Value.Settings.Workers)
				{
					return;
				}
				base.Config.Value.Settings.Workers = value;
				base.NotifyOfPropertyChange<int>(System.Linq.Expressions.Expression.Lambda<Func<int>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_Workers").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public SettingsViewModel()
		{
			if (Caliburn.Micro.Execute.InDesignMode)
			{
				this.Health = (IReadOnlyList<HealthEntry>)(new HealthEntry[] { new HealthEntry()
				{
					Name = "HTTP",
					State = "OK"
				}, new HealthEntry()
				{
					Name = "REDIS",
					State = "OK"
				}, new HealthEntry()
				{
					Name = "MYSQL",
					State = "OK"
				} });
			}
		}

		public async void ClearCache()
		{
			await this.Overlay.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
				try
				{
					try
					{
						this.CanClearCache = false;
						model.Maximum = 3;
						model.Text = "Clearing File Cache";
						Directories.ClearCache();
						model.Text = "Clearing Memory Cache";
						this.ServiceClient.Value.ClearCache();
						model.Text = "Rebuilding Memory Cache";
						await this.ServiceClient.Value.AssembliesAsync();
						await this.ServiceClient.Value.SandboxPermissionsAsync();
						await this.UpdateService.Value.PrepareAssembliesAsync(AssemblyUpdate.All, true);
						await PathRandomizer.CopyFilesAsync();
						Extensions.SetFullAccessControl(Directories.Cache);
					}
					catch (Exception exception)
					{
						PlaySharpScreen.Log.Error(exception);
					}
				}
				finally
				{
					this.CanClearCache = true;
				}
			});
		}

		private IGameSettingEntry GetOrCreateGameSetting(string name, string value = "False")
		{
			return base.Config.Value.GetOrCreateGameSetting(name, value);
		}

		private IHotkeyEntry GetOrCreateHotkey(string name, Key defaultKey = 156)
		{
			return base.Config.Value.GetOrCreateHotkey(name, defaultKey);
		}

		public void Logout()
		{
			base.Config.Value.AuthKey = null;
			base.Config.Value.Username = null;
			base.Config.Value.Save(false);
			Environment.Exit(0);
		}

		protected override async void OnInitialize()
		{
			SettingsViewModel.<OnInitialize>d__114 variable = new SettingsViewModel.<OnInitialize>d__114();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<SettingsViewModel.<OnInitialize>d__114>(ref variable);
		}

		public void OnKeyUp(object sender, KeyEventArgs e)
		{
			string name;
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				name = frameworkElement.Name;
			}
			else
			{
				name = null;
			}
			string str = name;
			if (str == "Menu")
			{
				this.Menu = e.Key;
				return;
			}
			if (str == "MenuToggle")
			{
				this.MenuToggle = e.Key;
				return;
			}
			if (str == "Reload")
			{
				this.Reload = e.Key;
				return;
			}
			if (str == "ReloadAndRecompile")
			{
				this.ReloadAndRecompile = e.Key;
				return;
			}
			if (str != "Unload")
			{
				return;
			}
			this.Unload = e.Key;
		}

		protected override void OnLoad()
		{
			base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_IsLeagueActive").MethodHandle)), new ParameterExpression[0]));
			base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(SettingsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(SettingsViewModel).GetMethod("get_IsDotaActive").MethodHandle)), new ParameterExpression[0]));
			this.SelectedLanguage = new CultureInfo(base.Config.Value.Settings.Language);
		}

		protected override void OnSave(bool closing = false)
		{
			base.Config.Value.Settings.Language = this.SelectedLanguage.Name;
		}

		public async void Save()
		{
			this.Config.Value.Save(this.UseCloud);
		}
	}
}