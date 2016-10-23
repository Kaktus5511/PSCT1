using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Loader.Shared.LegacyConfig
{
	[DesignerCategory("code")]
	[Serializable]
	[XmlType(AnonymousType=true)]
	public class ConfigSettings
	{
		private ConfigSettingsGameSettings[] gameSettingsField;

		[XmlArrayItem("GameSettings", IsNullable=false)]
		public ConfigSettingsGameSettings[] GameSettings
		{
			get
			{
				return this.gameSettingsField;
			}
			set
			{
				this.gameSettingsField = value;
			}
		}

		public ConfigSettings()
		{
		}
	}
}