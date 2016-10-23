using MahApps.Metro.Controls.Dialogs;
using PlaySharp.Toolkit.Messages;
using System;
using System.Runtime.CompilerServices;

namespace Loader.Model.Message
{
	public class OnShowMetroMessage : OnShowMessage
	{
		public MessageDialogResult Result
		{
			get;
			set;
		}

		public MessageDialogStyle Style
		{
			get;
		}

		public OnShowMetroMessage(string title, string message, MessageDialogStyle style = 0) : base(title, message)
		{
			this.Style = style;
		}
	}
}