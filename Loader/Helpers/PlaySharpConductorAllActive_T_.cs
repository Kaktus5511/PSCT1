using Caliburn.Micro;
using Loader.Model.Config;
using Loader.Services.Model;
using log4net;
using PlaySharp.Service.Package.Model;
using PlaySharp.Service.WebService;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Loader.Helpers
{
	public abstract class PlaySharpConductorAllActive<T> : Conductor.Collection.AllActive<T>
	where T : class
	{
		protected readonly static log4net.ILog Log;

		[Import(typeof(PlaySharpConfig), AllowRecomposition=true)]
		public Lazy<PlaySharpConfig> Config
		{
			get;
			set;
		}

		[Import(typeof(PlaySharp.Toolkit.EventAggregator.IEventAggregator))]
		public Lazy<PlaySharp.Toolkit.EventAggregator.IEventAggregator> EventAggregator
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

		static PlaySharpConductorAllActive()
		{
			PlaySharpConductorAllActive<T>.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public PlaySharpConductorAllActive()
		{
			PlaySharp.Toolkit.Helper.IoC.BuildUp(this);
		}

		protected override void OnActivate()
		{
			this.EventAggregator.Value.Subscribe(this);
			base.OnActivate();
		}

		protected override void OnDeactivate(bool close)
		{
			this.EventAggregator.Value.Unsubscribe(this);
			base.OnDeactivate(close);
		}
	}
}