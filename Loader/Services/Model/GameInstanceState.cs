using System;

namespace Loader.Services.Model
{
	public enum GameInstanceState
	{
		New,
		Created,
		Locked,
		LockError,
		InjectionError,
		CoreResponseError,
		OutdatedError,
		Sleep,
		UnexpectedError,
		CLRInjectionError,
		Completed,
		Injecting,
		InjectingCLR,
		WaitingForAuth,
		AlreadyInjected,
		Injected,
		CLRInjected
	}
}