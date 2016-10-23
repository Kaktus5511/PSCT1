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
	public class ShellView : MetroWindow, IComponentConnector
	{
		internal ShellView Window;

		internal Button Dashboard;

		internal ContentControl Overlay;

		internal ContentControl ActiveItem;

		internal TextBlock Header;

		private bool _contentLoaded;

		public ShellView()
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
			Application.LoadComponent(this, new Uri("/Loader;component/views/shellview.xaml", UriKind.Relative));
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
					this.Window = (ShellView)target;
					return;
				}
				case 2:
				{
					this.Dashboard = (Button)target;
					return;
				}
				case 3:
				{
					this.Overlay = (ContentControl)target;
					return;
				}
				case 4:
				{
					this.ActiveItem = (ContentControl)target;
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