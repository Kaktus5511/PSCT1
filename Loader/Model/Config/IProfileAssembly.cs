using System;

namespace Loader.Model.Config
{
	public interface IProfileAssembly
	{
		int Id
		{
			get;
			set;
		}

		bool Inject
		{
			get;
			set;
		}
	}
}