using Caliburn.Micro;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Loader.ViewModels
{
	public class NotificationViewModel : Screen
	{
		private DateTime created;

		private string icon;

		private bool isCompleted;

		private double maximum;

		private string text;

		private double @value;

		public DateTime Created
		{
			get
			{
				return this.created;
			}
			set
			{
				if (value.Equals(this.created))
				{
					return;
				}
				this.created = value;
				base.NotifyOfPropertyChange<DateTime>(Expression.Lambda<Func<DateTime>>(Expression.Property(Expression.Constant(this, typeof(NotificationViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(NotificationViewModel).GetMethod("get_Created").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				if (value == this.icon)
				{
					return;
				}
				this.icon = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(NotificationViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(NotificationViewModel).GetMethod("get_Icon").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool IsCompleted
		{
			get
			{
				return this.isCompleted;
			}
			set
			{
				if (value == this.isCompleted)
				{
					return;
				}
				this.isCompleted = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(NotificationViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(NotificationViewModel).GetMethod("get_IsCompleted").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public double Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (value.Equals(this.maximum))
				{
					return;
				}
				this.maximum = value;
				base.NotifyOfPropertyChange<double>(Expression.Lambda<Func<double>>(Expression.Property(Expression.Constant(this, typeof(NotificationViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(NotificationViewModel).GetMethod("get_Maximum").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (value == this.text)
				{
					return;
				}
				this.text = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(NotificationViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(NotificationViewModel).GetMethod("get_Text").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public double Value
		{
			get
			{
				return this.@value;
			}
			set
			{
				if (value.Equals(this.@value))
				{
					return;
				}
				this.@value = value;
				base.NotifyOfPropertyChange<double>(Expression.Lambda<Func<double>>(Expression.Property(Expression.Constant(this, typeof(NotificationViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(NotificationViewModel).GetMethod("get_Value").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public NotificationViewModel()
		{
		}
	}
}