using System;

namespace Loader.Services.Model
{
	public enum CoreMemoryState
	{
		Unknown,
		Success,
		PreInitFailed,
		PostInitFailed,
		AuthStarted,
		AuthFailed,
		PreInitStarted
	}
}