using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Loader.Shared.LegacyConfig
{
	[DesignerCategory("code")]
	[Serializable]
	[XmlType(AnonymousType=true)]
	public class ConfigHotkeys
	{
		private ConfigHotkeysSelectedHotkeys[] selectedHotkeysField;

		[XmlArrayItem("SelectedHotkeys", IsNullable=false)]
		public ConfigHotkeysSelectedHotkeys[] SelectedHotkeys
		{
			get
			{
				return this.selectedHotkeysField;
			}
			set
			{
				this.selectedHotkeysField = value;
			}
		}

		public ConfigHotkeys()
		{
		}
	}
}