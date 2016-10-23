using CachedImage;
using Caliburn.Micro;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Services.Model;
using Loader.ViewModels.Model;
using log4net;
using MahApps.Metro;
using PlaySharp.Service.Messages;
using PlaySharp.Service.Package.Model;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Model;
using PlaySharp.Toolkit.Raygun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace Loader.Services
{
	public class AppBootstrapper : BootstrapperBase, PlaySharp.Toolkit.EventAggregator.IHandleWithTask<OnLogin>, PlaySharp.Toolkit.EventAggregator.IHandle, PlaySharp.Toolkit.EventAggregator.IHandle<AccountData>
	{
		private readonly static log4net.ILog Log;

		public AggregateCatalog Catalog
		{
			get;
			private set;
		}

		[Import(typeof(PlaySharpConfig))]
		public Lazy<PlaySharpConfig> Config
		{
			get;
			set;
		}

		public PlaySharp.Toolkit.Helper.ConsoleControl ConsoleControl
		{
			get;
			set;
		}

		public CompositionContainer Container
		{
			get;
			private set;
		}

		[Import(typeof(PlaySharp.Toolkit.EventAggregator.IEventAggregator))]
		public Lazy<PlaySharp.Toolkit.EventAggregator.IEventAggregator> EventAggregator
		{
			get;
			set;
		}

		[Import(typeof(IInjector))]
		public Lazy<IInjector> InjectionService
		{
			get;
			set;
		}

		[Import(typeof(ILoginView))]
		public Lazy<ILoginView> LoginShell
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

		[Import(typeof(IUpdateService))]
		public Lazy<IUpdateService> UpdateService
		{
			get;
			set;
		}

		static AppBootstrapper()
		{
			AppBootstrapper.Log = Logs.GetLogger("Loader", MethodBase.GetCurrentMethod().DeclaringType);
		}

		public AppBootstrapper() : base(true)
		{
			string[] args = Environment.GetCommandLineArgs();
			if ((int)args.Length == 2 && args[1] == "/debug")
			{
				this.ConfigureConsole();
			}
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.OnAssemblyResolve);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.OnUnhandledException);
			Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.OnDispatcherUnhandledException);
			ResxLocalizationProvider.get_Instance().set_FallbackAssembly(Loc.DefaultAssembly);
			ResxLocalizationProvider.get_Instance().set_FallbackDictionary(Loc.DefaultDictionary);
			FileCache.AppCacheMode = FileCache.CacheMode.Dedicated;
			FileCache.AppCacheDirectory = Path.Combine(Directories.Cache, "images");
			KeyTrigger.Register();
			base.Initialize();
		}

		protected override void BuildUp(object instance)
		{
			this.Container.SatisfyImportsOnce(instance);
		}

		protected override void Configure()
		{
			int i;
			bool fatal = false;
			try
			{
				try
				{
					if (!Caliburn.Micro.Execute.InDesignMode)
					{
						AppBootstrapper.Log.Debug(string.Format("AssemblyCatalog ({0})", Assembly.GetExecutingAssembly().GetName().Name));
						this.Catalog = new AggregateCatalog(new ComposablePartCatalog[] { new AssemblyCatalog(Assembly.GetExecutingAssembly()) });
						Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
						for (i = 0; i < (int)assemblies.Length; i++)
						{
							Assembly assembly = assemblies[i];
							if (assembly.GetName().Name.StartsWith("PlaySharp."))
							{
								AppBootstrapper.Log.Debug(string.Format("AssemblyCatalog ({0})", assembly.GetName().Name));
								this.Catalog.Catalogs.Add(new AssemblyCatalog(assembly));
							}
						}
						this.Container = new CompositionContainer(this.Catalog, CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe, new ExportProvider[0]);
						AppBootstrapper appBootstrapper = this;
						Action<object> action = new Action<object>(appBootstrapper.BuildUp);
						AppBootstrapper appBootstrapper1 = this;
						AppBootstrapper appBootstrapper2 = this;
						PlaySharp.Toolkit.Helper.IoC.Initialize(action, new Func<Type, string, object>(appBootstrapper1.GetInstance), new Func<Type, IEnumerable<object>>(appBootstrapper2.GetAllInstances));
						CompositionBatch batch = new CompositionBatch();
						batch.AddPart(this);
						batch.AddExportedValue<BootstrapperBase>(this);
						batch.AddExportedValue<IWindowManager>(new WindowManager());
						batch.AddExportedValue<Caliburn.Micro.IEventAggregator>(new Caliburn.Micro.EventAggregator());
						batch.AddExportedValue<PlaySharpConfig>(PlaySharpConfig.Init());
						batch.AddExportedValue<CompositionContainer>(this.Container);
						this.Container.Compose(batch);
						try
						{
							RaygunClient.Instance.ApplicationVersion = Files.Version.ToString();
							RaygunClient.Instance.SendingMessage += new EventHandler<RaygunSendingMessageEventArgs>(this.OnSendingMessage);
						}
						catch (Exception exception)
						{
							AppBootstrapper.Log.Error(exception);
						}
						this.EventAggregator.Value.Subscribe(this);
					}
				}
				catch (ReflectionTypeLoadException reflectionTypeLoadException)
				{
					ReflectionTypeLoadException e = reflectionTypeLoadException;
					fatal = true;
					AppBootstrapper.Log.Fatal(e);
					Exception[] loaderExceptions = e.LoaderExceptions;
					for (i = 0; i < (int)loaderExceptions.Length; i++)
					{
						Exception ex = loaderExceptions[i];
						AppBootstrapper.Log.Error(ex.ToString());
					}
					MessageBox.Show(e.Message, "Fatal - Bootstrapper Error", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
				}
				catch (Exception exception1)
				{
					Exception e = exception1;
					fatal = true;
					AppBootstrapper.Log.Fatal(e);
					MessageBox.Show(e.Message, "Fatal - Bootstrapper Error", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
				}
			}
			finally
			{
				if (fatal)
				{
					Environment.Exit(0);
				}
			}
		}

		private void ConfigureConsole()
		{
			if (this.ConsoleControl != null)
			{
				return;
			}
			try
			{
				AppBootstrapper.Log.Debug("Setup Console");
				this.ConsoleControl = new PlaySharp.Toolkit.Helper.ConsoleControl();
				this.ConsoleControl.AttachSelf();
				Console.Title = "PlaySharp Console";
				Console.OutputEncoding = Encoding.Default;
				Console.WindowWidth = (int)((double)Console.LargestWindowWidth * 0.6);
				Console.BufferWidth = (int)((double)Console.LargestWindowWidth * 0.6);
				Console.WindowHeight = (int)((double)Console.LargestWindowHeight * 0.6);
				Console.WindowWidth = (int)((double)Console.LargestWindowWidth * 0.6);
			}
			catch (Exception exception)
			{
				AppBootstrapper.Log.Warn(exception);
			}
		}

		public void DisplayView<T>()
		{
			base.DisplayRootViewFor<T>(null);
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return this.Container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
		}

		protected override object GetInstance(Type serviceType, string key)
		{
			string contract = (string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key);
			IEnumerable<object> exports = this.Container.GetExportedValues<object>(contract);
			if (!exports.Any<object>())
			{
				throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
			}
			return exports.First<object>();
		}

		public async Task Handle(OnLogin message)
		{
			this.Config.Value.Load(this.Config.Value.UseCloud);
			this.Config.Value.Id = message.Login.Id;
			this.Config.Value.Username = message.Login.Name;
			this.Config.Value.AuthKey = message.Login.AuthKey;
			this.Config.Value.Name = Process.GetCurrentProcess().ProcessName;
			this.Config.Value.Save(false);
			await this.PostConfigure();
			try
			{
				this.DisplayView<IShellView>();
			}
			catch (Exception exception)
			{
				AppBootstrapper.Log.Fatal(exception);
			}
		}

		public void Handle(AccountData message)
		{
			if (message.Account.IsDev)
			{
				this.ConfigureConsole();
				this.Config.Value.Settings.UserType = UserType.Developer;
			}
		}

		private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name == "Loader")
			{
				return Assembly.GetExecutingAssembly();
			}
			return null;
		}

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			File.AppendAllText(Path.Combine(Directories.Logs, "FATAL.log"), string.Concat(e.Exception, Environment.NewLine, Environment.NewLine));
			AppBootstrapper.Log.Fatal(e.Exception);
			MessageBox.Show(string.Format("An unhandled exception just occurred: {0}", e.Exception), "Exception", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
			e.Handled = true;
		}

		protected override void OnExit(object sender, EventArgs e)
		{
			this.UpdateService.Value.Deactivate();
			this.PackageClient.Value.Save();
			this.Config.Value.Save(this.Config.Value.UseCloud);
			if (!this.InjectionService.Value.IsInjected && Directory.Exists(Directories.Assemblies))
			{
				Utility.DeleteFiles(Directories.Assemblies, (string file) => file.EndsWith(".dll"));
			}
			base.OnExit(sender, e);
		}

		private void OnSendingMessage(object sender, RaygunSendingMessageEventArgs args)
		{
			bool value;
			string str;
			string str1;
			object obj;
			object obj1;
			object gameFilePath;
			object settings;
			try
			{
				Lazy<PlaySharpConfig> config = this.Config;
				if (config != null)
				{
					value = config.Value;
				}
				else
				{
					value = false;
				}
				if (value)
				{
					if (args.Message.Details.UserCustomData == null)
					{
						args.Message.Details.UserCustomData = new Dictionary<string, object>();
					}
					if (args.Message.Details.Tags == null)
					{
						args.Message.Details.Tags = new List<string>();
					}
					IList<string> tags = args.Message.Details.Tags;
					PlaySharpConfig playSharpConfig = this.Config.Value;
					if (playSharpConfig != null)
					{
						ISettings setting = playSharpConfig.Settings;
						if (setting != null)
						{
							str = setting.LoaderUpdateChannel.ToString();
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
					tags.Add(str);
					IList<string> strs = args.Message.Details.Tags;
					PlaySharpConfig value1 = this.Config.Value;
					if (value1 != null)
					{
						str1 = value1.Service.ToString();
					}
					else
					{
						str1 = null;
					}
					strs.Add(str1);
					args.Message.Details.Tags.Add(Files.Version.ToString());
					IDictionary userCustomData = args.Message.Details.UserCustomData;
					PlaySharpConfig playSharpConfig1 = this.Config.Value;
					if (playSharpConfig1 != null)
					{
						ISettings settings1 = playSharpConfig1.Settings;
						if (settings1 != null)
						{
							obj = settings1.LoaderUpdateChannel.ToString();
						}
						else
						{
							obj = null;
						}
					}
					else
					{
						obj = null;
					}
					userCustomData["Channel"] = obj;
					IDictionary dictionaries = args.Message.Details.UserCustomData;
					PlaySharpConfig value2 = this.Config.Value;
					if (value2 != null)
					{
						obj1 = value2.Service.ToString();
					}
					else
					{
						obj1 = null;
					}
					dictionaries["Service"] = obj1;
					IDictionary userCustomData1 = args.Message.Details.UserCustomData;
					PlaySharpConfig playSharpConfig2 = this.Config.Value;
					if (playSharpConfig2 != null)
					{
						IServiceSettings serviceSettings = playSharpConfig2.ServiceSettings;
						if (serviceSettings != null)
						{
							gameFilePath = serviceSettings.GameFilePath;
						}
						else
						{
							gameFilePath = null;
						}
					}
					else
					{
						gameFilePath = null;
					}
					userCustomData1["GameFilePath"] = gameFilePath;
					IDictionary dictionaries1 = args.Message.Details.UserCustomData;
					PlaySharpConfig value3 = this.Config.Value;
					if (value3 != null)
					{
						settings = value3.Settings;
					}
					else
					{
						settings = null;
					}
					dictionaries1["Settings"] = settings;
				}
			}
			catch (Exception exception)
			{
				AppBootstrapper.Log.Warn(exception);
			}
		}

		protected override async void OnStartup(object sender, StartupEventArgs e)
		{
			string authKey;
			string str;
			try
			{
				Lazy<PlaySharpConfig> config = this.Config;
				if (config != null)
				{
					PlaySharpConfig value = config.Value;
					if (value != null)
					{
						authKey = value.AuthKey;
					}
					else
					{
						authKey = null;
					}
				}
				else
				{
					authKey = null;
				}
				if (HashFactory.MD5.IsValid(authKey))
				{
					AppBootstrapper.Log.Debug("AutoLogin using AuthKey");
					IWebServiceClient webServiceClient = this.ServiceClient.Value;
					Lazy<PlaySharpConfig> lazy = this.Config;
					if (lazy != null)
					{
						PlaySharpConfig playSharpConfig = lazy.Value;
						if (playSharpConfig != null)
						{
							str = playSharpConfig.AuthKey;
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
					if (await webServiceClient.LoginAsync(str))
					{
						return;
					}
				}
			}
			catch (AuthenticationException authenticationException1)
			{
				AuthenticationException authenticationException = authenticationException1;
				AppBootstrapper.Log.Info(string.Format("AutoLogin failed {0}", authenticationException.Message));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				AppBootstrapper.Log.Error(exception);
				MessageBox.Show(exception.Message, "OnStartup", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			this.DisplayView<ILoginView>();
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			if (!args.IsTerminating)
			{
				AppBootstrapper.Log.Error(args.ExceptionObject);
			}
			else
			{
				File.AppendAllText(Path.Combine(Directories.Logs, "FATAL.log"), string.Concat(args.ExceptionObject, Environment.NewLine, Environment.NewLine));
				AppBootstrapper.Log.Fatal(args.ExceptionObject);
			}
			MessageBox.Show(string.Format("An unhandled exception just occurred: {0}", args.ExceptionObject), "Exception", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
		}

		private async Task PostConfigure()
		{
			if (this.Config.Value.Settings.Language.IsNullOrEmpty())
			{
				this.Config.Value.Settings.Language = CultureInfo.InstalledUICulture.Name;
			}
			LocalizeDictionary.get_Instance().set_Culture(new CultureInfo(this.Config.Value.Settings.Language));
			this.PackageClient.Value.ConfigDirectory = Directories.Config;
			this.PackageClient.Value.PackageCacheDirectory = Directories.Packages;
			this.PackageClient.Value.Load();
			this.ServiceClient.Value.ServiceType = this.Config.Value.Service;
			this.ServiceClient.Value.Language = LocalizeDictionary.get_Instance().get_Culture();
			RaygunClient instance = RaygunClient.Instance;
			int id = this.Config.Value.Id;
			instance.UserInfo = new RaygunIdentifierMessage(id.ToString());
			RaygunClient.Instance.UserInfo.FullName = this.Config.Value.Username;
			if (this.Config.Value.Settings.ApiEndpoint != null)
			{
				this.ServiceClient.Value.UseEndpoint(EndpointType.API, this.Config.Value.Settings.ApiEndpoint);
			}
			if (this.Config.Value.Settings.DataEndpoint != null)
			{
				this.ServiceClient.Value.UseEndpoint(EndpointType.DATA, this.Config.Value.Settings.DataEndpoint);
			}
			try
			{
				Application current = Application.Current;
				string color = this.Config.Value.Settings.Color;
				if (color == null)
				{
					color = "Cobalt";
				}
				Accent accent = ThemeManager.GetAccent(color);
				string theme = this.Config.Value.Settings.Theme;
				if (theme == null)
				{
					theme = "BaseLight";
				}
				ThemeManager.ChangeAppStyle(current, accent, ThemeManager.GetAppTheme(theme));
			}
			catch (Exception exception)
			{
				AppBootstrapper.Log.Warn(exception);
			}
			try
			{
				await this.ServiceClient.Value.AssembliesAsync();
				await this.ServiceClient.Value.CoresAsync();
				await this.ServiceClient.Value.FilesAsync();
				await this.ServiceClient.Value.SandboxPermissionsAsync();
			}
			catch (TaskCanceledException taskCanceledException)
			{
				AppBootstrapper.Log.Warn("Canceled");
			}
			catch (Exception exception1)
			{
				AppBootstrapper.Log.Error(exception1);
			}
		}
	}
}