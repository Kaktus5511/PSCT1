using Loader.Services.Model;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
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

namespace Loader.ViewModels
{
	[Export(typeof(ITriggeredGameResolver))]
	public class TriggeredGameResolver : ServiceBase, ITriggeredGameResolver, IService, IControllable, IActivatable, IStateful, IDeactivatable, IHandle<OnProcessCreated>, IHandle
	{
		private readonly static ILog Log;

		[Import(typeof(IDashboardView))]
		protected Lazy<IDashboardView> Dashboard
		{
			get;
			set;
		}

		[Import(typeof(IInjector))]
		protected Lazy<IInjector> Injector
		{
			get;
			set;
		}

		[Import(typeof(IProcessResolver))]
		protected Lazy<IProcessResolver> ProcessResolver
		{
			get;
			set;
		}

		private IReadOnlyList<string> Triggers { get; } = (IReadOnlyList<string>)(new string[] { "Steam", "LoLLauncher", "LolClient", "LoLTWLauncher", "LoLVNLauncher" });

		[Import(typeof(IUpdateService))]
		protected Lazy<IUpdateService> UpdateService
		{
			get;
			set;
		}

		static TriggeredGameResolver()
		{
			TriggeredGameResolver.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public TriggeredGameResolver()
		{
		}

		public async void Handle(OnProcessCreated message)
		{
			if (this.Triggers.Contains<string>(message.Process.Name))
			{
				if (!this.Injector.Value.IsInjected)
				{
					TriggeredGameResolver.Log.Debug(string.Format("Update GamePath from {0}", message.Process));
					if (this.UpdateService.Value.UpdateGamePath())
					{
						await this.Dashboard.Value.UpdateGameInfoAsync();
						await this.UpdateService.Value.UpdateCoreAsync();
						await this.Dashboard.Value.UpdateCoreInfoAsync();
					}
				}
				else
				{
					TriggeredGameResolver.Log.Debug("Skiped Game Path Update because of active injection.");
				}
			}
		}

		protected override void OnInitialize()
		{
			foreach (string trigger in this.Triggers)
			{
				this.ProcessResolver.Value.AddProcessFilter(trigger);
			}
		}
	}
}