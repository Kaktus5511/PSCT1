using PlaySharp.Toolkit.Injector;
using System;
using System.Runtime.CompilerServices;

namespace Loader.Services.Model
{
	public class GameInstanceException : Exception
	{
		public IGameInstance Instance
		{
			get;
			set;
		}

		public override string Message
		{
			get
			{
				return string.Format("Unexpected State: {0} - {1}", this.Instance.State, this.Result);
			}
		}

		public InjectorResult Result
		{
			get;
			set;
		}

		public GameInstanceException(IGameInstance gameInstanceBase, InjectorResult result = 0)
		{
			this.Instance = gameInstanceBase;
			this.Result = result;
		}
	}
}