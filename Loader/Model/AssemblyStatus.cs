using System;

namespace Loader.Model
{
	public enum AssemblyStatus
	{
		Ready,
		Updating,
		UpdatingError,
		CompilingError,
		Compiling
	}
}