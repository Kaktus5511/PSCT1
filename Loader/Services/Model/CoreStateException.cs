using System;
using System.Runtime.CompilerServices;

namespace Loader.Services.Model
{
	public class CoreStateException : Exception
	{
		public override string Message
		{
			get
			{
				return string.Format("Core injection failed - {0}", this.State);
			}
		}

		public CoreMemoryState State
		{
			get;
		}

		public CoreStateException(CoreMemoryState state)
		{
			this.State = state;
		}
	}
}