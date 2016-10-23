using System;
using System.Runtime.CompilerServices;

namespace Loader.Model.Message
{
	public class OnCloseApplication
	{
		public string Reason
		{
			get;
			set;
		}

		public OnCloseApplication(string reason)
		{
			this.Reason = reason;
		}
	}
}