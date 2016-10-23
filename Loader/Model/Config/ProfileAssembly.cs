using System;
using System.Runtime.CompilerServices;

namespace Loader.Model.Config
{
	public class ProfileAssembly : IProfileAssembly
	{
		public int Id
		{
			get;
			set;
		}

		public bool Inject
		{
			get;
			set;
		}

		public ProfileAssembly(int id)
		{
			this.Id = id;
		}
	}
}