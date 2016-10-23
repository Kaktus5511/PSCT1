using Loader.Model.Config;
using System;
using System.Runtime.CompilerServices;

namespace Loader.Model.Message
{
	public class OnProfileChanged
	{
		public IProfile Profile
		{
			get;
			set;
		}

		public OnProfileChanged(IProfile profile)
		{
			this.Profile = profile;
		}
	}
}