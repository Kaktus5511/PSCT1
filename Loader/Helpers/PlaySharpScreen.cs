using Caliburn.Micro;
using Loader.Model.Config;
using Loader.Services.Model;
using Loader.ViewModels.Model;
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
	public abstract class PlaySharpScreen : Screen
	{
		protected readonly static log4net.ILog Log;

		[Import(typeof(PlaySharpConfig))]
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

		[Import(typeof(IShellView))]
		public Lazy<IShellView> Shell
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

		static PlaySharpScreen()
		{
			PlaySharpScreen.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		protected PlaySharpScreen()
		{
			PlaySharp.Toolkit.Helper.IoC.BuildUp(this);
		}

		protected override void OnActivate()
		{
			this.EventAggregator.Value.Subscribe(this);
			base.OnActivate();
			this.OnLoad();
		}

		protected override void OnDeactivate(bool close)
		{
			this.EventAggregator.Value.Unsubscribe(this);
			base.OnDeactivate(close);
			this.OnSave(close);
		}

		protected virtual void OnLoad()
		{
		}

		protected virtual void OnSave(bool closing = false)
		{
		}
	}
}