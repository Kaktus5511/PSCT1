using System;

namespace Loader.Services.Model
{
	public interface IGameInstance : IDisposable
	{
		bool IsAlive
		{
			get;
		}

		bool IsCompleted
		{
			get;
		}

		bool IsDisposed
		{
			get;
		}

		GameInstanceState State
		{
			get;
		}
	}
}