using System;
using System.Collections.Generic;

namespace Loader.Model.Config
{
	public interface IServiceSettings
	{
		string AppDomainFileName
		{
			get;
			set;
		}

		string AppName
		{
			get;
			set;
		}

		List<IPlaySharpAssembly> Assemblies
		{
			get;
			set;
		}

		string BootstrapFileName
		{
			get;
			set;
		}

		string CoreBridgeFileName
		{
			get;
			set;
		}

		string CoreFileName
		{
			get;
			set;
		}

		string GameFilePath
		{
			get;
			set;
		}

		List<IGameSettingEntry> GameSettings
		{
			get;
			set;
		}

		List<IHotkeyEntry> Hotkeys
		{
			get;
			set;
		}

		int InjectionDelay
		{
			get;
			set;
		}

		string PlatformTarget
		{
			get;
			set;
		}

		string ProcessName
		{
			get;
			set;
		}

		List<IProfile> Profiles
		{
			get;
			set;
		}

		string SharedMemoryName
		{
			get;
			set;
		}
	}
}