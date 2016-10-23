using Caliburn.Micro;
using Loader.Helpers;
using Loader.Model.Config;
using Loader.ViewModels.Model;
using PlaySharp.Service.Messages;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Loader.ViewModels
{
	[Export(typeof(ILoginView))]
	public class LoginViewModel : PlaySharpScreen, ILoginView, IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged, PlaySharp.Toolkit.EventAggregator.IHandle<OnLogin>, PlaySharp.Toolkit.EventAggregator.IHandle
	{
		private string authKey;

		private bool isBusy;

		private string message = "Authenticating on joduska.me...";

		public string AuthKey
		{
			get
			{
				return this.authKey;
			}
			set
			{
				if (value.Trim() == this.authKey)
				{
					return;
				}
				this.authKey = value.Trim();
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(LoginViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LoginViewModel).GetMethod("get_AuthKey").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(LoginViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LoginViewModel).GetMethod("get_CanLogin").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public CancellationTokenSource AutoLoginTaskToken
		{
			get;
			set;
		}

		public bool CanExit
		{
			get
			{
				if (this.IsBusy)
				{
					return false;
				}
				return true;
			}
		}

		public bool CanLogin
		{
			get
			{
				if (this.IsBusy)
				{
					return false;
				}
				if (!string.IsNullOrEmpty(this.AuthKey))
				{
					return true;
				}
				return false;
			}
		}

		public override string DisplayName
		{
			get;
			set;
		}

		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			set
			{
				if (value == this.isBusy)
				{
					return;
				}
				this.isBusy = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(LoginViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LoginViewModel).GetMethod("get_IsBusy").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(LoginViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LoginViewModel).GetMethod("get_CanLogin").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(LoginViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LoginViewModel).GetMethod("get_CanExit").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				if (value == this.message)
				{
					return;
				}
				this.message = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(LoginViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LoginViewModel).GetMethod("get_Message").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public LoginViewModel()
		{
		}

		public async Task AutoLoginAsync(CancellationToken ct)
		{
			LoginViewModel.<AutoLoginAsync>d__24 variable = new LoginViewModel.<AutoLoginAsync>d__24();
			variable.<>4__this = this;
			variable.ct = ct;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<LoginViewModel.<AutoLoginAsync>d__24>(ref variable);
			return variable.<>t__builder.Task;
		}

		public void Exit()
		{
			this.AutoLoginTaskToken.Cancel();
			Application.Current.Shutdown();
		}

		public void Handle(OnLogin message)
		{
			base.Config.Value.Username = message.Login.Name;
			base.Config.Value.AuthKey = message.Login.AuthKey;
			this.TryClose(null);
		}

		public async void Login()
		{
			LoginViewModel.<Login>d__27 variable = new LoginViewModel.<Login>d__27();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<LoginViewModel.<Login>d__27>(ref variable);
		}

		protected override async void OnActivate()
		{
			LoginViewModel.<OnActivate>d__28 variable = new LoginViewModel.<OnActivate>d__28();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<LoginViewModel.<OnActivate>d__28>(ref variable);
		}

		protected override void OnDeactivate(bool close)
		{
			base.OnDeactivate(close);
			this.AutoLoginTaskToken.Cancel();
		}
	}
}