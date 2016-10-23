using System;
using System.Windows;

namespace Loader.Helpers
{
	public class BindingProxy : Freezable
	{
		public readonly static DependencyProperty DataProperty;

		public object Data
		{
			get
			{
				return base.GetValue(BindingProxy.DataProperty);
			}
			set
			{
				base.SetValue(BindingProxy.DataProperty, value);
			}
		}

		static BindingProxy()
		{
			BindingProxy.DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));
		}

		public BindingProxy()
		{
		}

		protected override Freezable CreateInstanceCore()
		{
			return new BindingProxy();
		}
	}
}