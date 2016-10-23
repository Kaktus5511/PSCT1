using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Loader.Shared.LegacyConfig
{
	[DesignerCategory("code")]
	[Serializable]
	[XmlType(AnonymousType=true)]
	public class ConfigSettingsGameSettings
	{
		private string nameField;

		private string[] posibleValuesField;

		private string selectedValueField;

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

		[XmlArrayItem(IsNullable=false)]
		public string[] PosibleValues
		{
			get
			{
				return this.posibleValuesField;
			}
			set
			{
				this.posibleValuesField = value;
			}
		}

		public string SelectedValue
		{
			get
			{
				return this.selectedValueField;
			}
			set
			{
				this.selectedValueField = value;
			}
		}

		public ConfigSettingsGameSettings()
		{
		}
	}
}