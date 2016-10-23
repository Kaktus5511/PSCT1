using Loader.Model.Config;
using log4net;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Loader.Model
{
	public static class ConfigExtensions
	{
		private readonly static ILog Log;

		static ConfigExtensions()
		{
			ConfigExtensions.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public static IGameSettingEntry GetOrCreateGameSetting(this PlaySharpConfig config, string name, string value = "False")
		{
			IGameSettingEntry entry = config.ServiceSettings.GameSettings.FirstOrDefault<IGameSettingEntry>((IGameSettingEntry h) => h.Name == name);
			if (entry == null)
			{
				ConfigExtensions.Log.Debug(string.Format("Create GameSetting {0}", name));
				entry = config.ServiceSettings.GameSettings.AddItem<IGameSettingEntry>(new GameSettingEntry()
				{
					Name = name,
					SelectedValue = value
				}, -1);
			}
			return entry;
		}

		public static IHotkeyEntry GetOrCreateHotkey(this PlaySharpConfig config, string name, Key defaultKey = 156)
		{
			ConfigExtensions.Log.Debug(string.Format("Create Hotkey {0}", name));
			IHotkeyEntry entry = config.ServiceSettings.Hotkeys.FirstOrDefault<IHotkeyEntry>((IHotkeyEntry h) => h.Name == name) ?? config.ServiceSettings.Hotkeys.AddItem<IHotkeyEntry>(new HotkeyEntry()
			{
				Name = name,
				Hotkey = defaultKey
			}, -1);
			return entry;
		}
	}
}