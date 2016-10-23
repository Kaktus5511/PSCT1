using Caliburn.Micro;
using Loader.Helpers;
using Loader.ViewModels.Model;
using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Reflection;

namespace Loader.ViewModels
{
	[Export(typeof(INotifier))]
	public class NotifierViewModel : PlaySharpConductorAllActive<NotificationViewModel>, INotifier
	{
		private string state;

		public string State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (value == this.state)
				{
					return;
				}
				this.state = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(NotifierViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(NotifierViewModel).GetMethod("get_State").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public NotifierViewModel()
		{
		}

		public void Add(NotificationViewModel notification)
		{
			this.ActivateItem(notification);
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.ActivateItem(new NotificationViewModel()
			{
				Text = "Hello World"
			});
		}

		public static class CommonStates
		{
			public static string Closed
			{
				get
				{
					return "Closed";
				}
			}

			public static string Open
			{
				get
				{
					return "Open";
				}
			}
		}
	}
}