using System;

namespace Loader.Model
{
	[Flags]
	public enum AssemblyUpdate
	{
		All = -1,
		Local = 2,
		GitHub = 4,
		Database = 8,
		SelectedOnly = 16,
		Libraries = 32,
		IgnoreLocalUpdate = 64
	}
}