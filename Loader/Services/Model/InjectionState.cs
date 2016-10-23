using System;

namespace Loader.Services.Model
{
	public enum InjectionState
	{
		New,
		WaitingForCore,
		LoadingCLR,
		Loaded
	}
}