using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Loader.Shared.LegacyConfig
{
	[DesignerCategory("code")]
	[Serializable]
	[XmlType(AnonymousType=true)]
	public class ConfigHotkeysSelectedHotkeys
	{
		private string defaultKeyField;

		private string descriptionField;

		private string hotkeyField;

		private byte hotkeyIntField;

		private string nameField;

		public string DefaultKey
		{
			get
			{
				return this.defaultKeyField;
			}
			set
			{
				this.defaultKeyField = value;
			}
		}

		public string Description
		{
			get
			{
				return this.descriptionField;
			}
			set
			{
				this.descriptionField = value;
			}
		}

		public string Hotkey
		{
			get
			{
				return this.hotkeyField;
			}
			set
			{
				this.hotkeyField = value;
			}
		}

		public byte HotkeyInt
		{
			get
			{
				return this.hotkeyIntField;
			}
			set
			{
				this.hotkeyIntField = value;
			}
		}

		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		public ConfigHotkeysSelectedHotkeys()
		{
		}
	}
}