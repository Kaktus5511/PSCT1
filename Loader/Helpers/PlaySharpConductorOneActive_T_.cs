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
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Loader.Helpers
{
	public abstract class PlaySharpConductorOneActive<T> : Conductor.Collection.OneActive<T>
	where T : class
	{
		protected readonly static log4net.ILog Log;

		private bool isLoading;

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

		public bool IsLoading
		{
			get
			{
				return this.isLoading;
			}
			set
			{
				if (value == this.isLoading)
				{
					return;
				}
				this.isLoading = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(PlaySharpConductorOneActive<T>)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PlaySharpConductorOneActive<T>).GetMethod("get_IsLoading").MethodHandle, typeof(PlaySharpConductorOneActive<T>).TypeHandle)), new ParameterExpression[0]));
			}
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

		static PlaySharpConductorOneActive()
		{
			PlaySharpConductorOneActive<T>.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public PlaySharpConductorOneActive()
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