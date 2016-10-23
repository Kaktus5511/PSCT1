using MahApps.Metro.Controls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Loader.Views
{
	public class LoginView : MetroWindow, IComponentConnector
	{
		internal TextBox AuthKey;

		internal Button Exit;

		internal Button Login;

		internal Label Message;

		internal TextBlock Header;

		private bool _contentLoaded;

		public LoginView()
		{
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Application.LoadComponent(this, new Uri("/Loader;component/views/loginview.xaml", UriKind.Relative));
		}

		[DebuggerNonUserCode]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
				case 1:
				{
					this.AuthKey = (TextBox)target;
					return;
				}
				case 2:
				{
					this.Exit = (Button)target;
					return;
				}
				case 3:
				{
					this.Login = (Button)target;
					return;
				}
				case 4:
				{
					this.Message = (Label)target;
					return;
				}
				case 5:
				{
					this.Header = (TextBlock)target;
					return;
				}
			}
			this._contentLoaded = true;
		}
	}
}