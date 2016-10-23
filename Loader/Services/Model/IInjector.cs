using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Service;
using System;
using System.Collections.Generic;

namespace Loader.Services.Model
{
	public interface IInjector : IService, IControllable, IActivatable, IStateful, IDeactivatable
	{
		Dictionary<int, IGameInstance> Instances
		{
			get;
		}

		bool IsInjected
		{
			get;
		}
	}
}