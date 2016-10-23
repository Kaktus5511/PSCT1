using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Loader.Model.Config
{
	public class ServiceSettings : IServiceSettings
	{
		private List<IProfile> profiles = new List<IProfile>();

		public string AppDomainFileName { get; set; } = "LeagueSharp.Sandbox.dll";

		public string AppName { get; set; } = "L#";

		public List<IPlaySharpAssembly> Assemblies { get; set; } = new List<IPlaySharpAssembly>();

		public string BootstrapFileName { get; set; } = "LeagueSharp.Bootstrap.dll";

		public string CoreBridgeFileName { get; set; } = "LeagueSharp.dll";

		public string CoreFileName { get; set; } = "LeagueSharp.Core.dll";

		public string GameFilePath
		{
			get;
			set;
		}

		public List<IGameSettingEntry> GameSettings { get; set; } = new List<IGameSettingEntry>();

		public List<IHotkeyEntry> Hotkeys { get; set; } = new List<IHotkeyEntry>();

		public int InjectionDelay { get; set; } = 10;

		public string PlatformTarget { get; set; } = "x86";

		public string ProcessName { get; set; } = "League of Legends";

		public List<IProfile> Profiles
		{
			get
			{
				if (this.profiles == null)
				{
					this.profiles = new List<IProfile>();
				}
				return this.profiles;
			}
			set
			{
				this.profiles = value;
			}
		}

		public string SharedMemoryName { get; set; } = "LeagueSharpBootstrap";

		public ServiceSettings()
		{
		}
	}
}