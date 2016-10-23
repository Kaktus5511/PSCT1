using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loader.Model.Config
{
	public interface IProfile
	{
		IList<IProfileAssembly> Assemblies
		{
			get;
			set;
		}

		IList<string> Flags
		{
			get;
			set;
		}

		bool Inject
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}

		Task<bool> CompileAsync();

		Task<bool> UpdateAsync();
	}
}