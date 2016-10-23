using System;
using System.Windows.Input;

namespace Loader.Model.Config
{
	public interface IHotkeyEntry
	{
		Key DefaultKey
		{
			get;
			set;
		}

		string Description
		{
			get;
			set;
		}

		string DisplayDescription
		{
			get;
		}

		Key Hotkey
		{
			get;
			set;
		}

		byte HotkeyInt
		{
			get;
		}

		string HotkeyString
		{
			get;
		}

		string Name
		{
			get;
			set;
		}
	}
}