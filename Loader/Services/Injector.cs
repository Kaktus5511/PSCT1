using Loader.Model.Config;
using Loader.Services.Model;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Service.WebService;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Model;
using PlaySharp.Toolkit.ProcessResolver;
using PlaySharp.Toolkit.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Loader.Services
{
	[Export(typeof(IInjector))]
	public class Injector : ServiceBase, IInjector, IService, IControllable, IActivatable, IStateful, IDeactivatable, IHandle<OnProcessCreated>, IHandle, IHandle<OnProcessDestroyed>
	{
		private readonly static ILog Log;

		[Import(typeof(PlaySharpConfig))]
		public Lazy<PlaySharpConfig> Config
		{
			get;
			set;
		}

		private ProcessWrapper[] GameProcess
		{
			get
			{
				return (
					from p in this.ProcessResolver.Value.Running
					where p.Name == this.Config.Value.ServiceSettings.ProcessName
					select p).ToArray<ProcessWrapper>();
			}
		}

		public Dictionary<int, IGameInstance> Instances
		{
			get
			{
				return JustDecompileGenerated_get_Instances();
			}
			set
			{
				JustDecompileGenerated_set_Instances(value);
			}
		}

		private Dictionary<int, IGameInstance> JustDecompileGenerated_Instances_k__BackingField = new Dictionary<int, IGameInstance>();

		public Dictionary<int, IGameInstance> JustDecompileGenerated_get_Instances()
		{
			return this.JustDecompileGenerated_Instances_k__BackingField;
		}

		private void JustDecompileGenerated_set_Instances(Dictionary<int, IGameInstance> value)
		{
			this.JustDecompileGenerated_Instances_k__BackingField = value;
		}

		public bool IsInjected
		{
			get
			{
				return this.GameProcess.Any<ProcessWrapper>(new Func<ProcessWrapper, bool>(this.IsProcessInjected));
			}
		}

		[Import(typeof(IBusyOverlayView))]
		public Lazy<IBusyOverlayView> Overlay
		{
			get;
			set;
		}

		[Import(typeof(IProcessResolver))]
		public Lazy<IProcessResolver> ProcessResolver
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

		static Injector()
		{
			Injector.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public Injector()
		{
		}

		public async void Handle(OnProcessCreated message)
		{
			ProcessWrapper process = message.Process;
			if (process.Name == this.Config.Value.ServiceSettings.ProcessName)
			{
				await Task.Yield();
				if (!this.Instances.ContainsKey(process.Id))
				{
					ServiceType service = this.Config.Value.Service;
					if (service != ServiceType.Dota || Singleton<EnnolaService>.Instance.IsLoaded())
					{
						service = this.Config.Value.Service;
						if (service == ServiceType.LoL)
						{
							this.Instances[process.Id] = new LeagueGameInstance(process.Process);
						}
						else if (service == ServiceType.Dota)
						{
							this.Instances[process.Id] = new EnsageGameInstance(process.Process);
						}
					}
					else
					{
						Injector.Log.Warn("Ennola not loaded!");
						return;
					}
				}
			}
		}

		public async void Handle(OnProcessDestroyed message)
		{
			ProcessWrapper process = message.Process;
			await Task.Yield();
			if (this.Instances.ContainsKey(process.Id))
			{
				this.Instances.Remove(process.Id);
			}
		}

		private bool IsProcessInjected(ProcessWrapper leagueProcess)
		{
			return this.Instances.ContainsKey(leagueProcess.Id);
		}

		protected override void OnActivate()
		{
			bool processName;
			PlaySharpConfig value = this.Config.Value;
			if (value != null)
			{
				IServiceSettings serviceSettings = value.ServiceSettings;
				if (serviceSettings != null)
				{
					processName = serviceSettings.ProcessName;
				}
				else
				{
					processName = false;
				}
			}
			else
			{
				processName = false;
			}
			if (!processName)
			{
				Injector.Log.Fatal("Service ProcessName not found.");
				return;
			}
			this.ProcessResolver.Value.AddProcessFilter(this.Config.Value.ServiceSettings.ProcessName);
			this.ProcessResolver.Value.Activate();
		}
	}
}