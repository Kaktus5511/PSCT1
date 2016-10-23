using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Loader.Shared.LegacyConfig
{
	[DesignerCategory("code")]
	[Serializable]
	[XmlType(AnonymousType=true)]
	public class ConfigProfiles
	{
		private ConfigProfilesLeagueSharpAssembly[] installedAssembliesField;

		private string nameField;

		[XmlArrayItem("LeagueSharpAssembly", IsNullable=false)]
		public ConfigProfilesLeagueSharpAssembly[] InstalledAssemblies
		{
			get
			{
				return this.installedAssembliesField;
			}
			set
			{
				this.installedAssembliesField = value;
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

		public ConfigProfiles()
		{
		}
	}
}