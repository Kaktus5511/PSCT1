using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Loader.Shared.LegacyConfig
{
	[DesignerCategory("code")]
	[Serializable]
	[XmlType(AnonymousType=true)]
	public class ConfigProfilesLeagueSharpAssembly
	{
		private string authorField;

		private string descriptionField;

		private string displayNameField;

		private bool injectCheckedField;

		private bool installCheckedField;

		private string nameField;

		private string pathToProjectFileField;

		private string statusField;

		private string svnUrlField;

		public string Author
		{
			get
			{
				return this.authorField;
			}
			set
			{
				this.authorField = value;
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

		public string DisplayName
		{
			get
			{
				return this.displayNameField;
			}
			set
			{
				this.displayNameField = value;
			}
		}

		public bool InjectChecked
		{
			get
			{
				return this.injectCheckedField;
			}
			set
			{
				this.injectCheckedField = value;
			}
		}

		public bool InstallChecked
		{
			get
			{
				return this.installCheckedField;
			}
			set
			{
				this.installCheckedField = value;
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

		public string PathToProjectFile
		{
			get
			{
				return this.pathToProjectFileField;
			}
			set
			{
				this.pathToProjectFileField = value;
			}
		}

		public string Status
		{
			get
			{
				return this.statusField;
			}
			set
			{
				this.statusField = value;
			}
		}

		public string SvnUrl
		{
			get
			{
				return this.svnUrlField;
			}
			set
			{
				this.svnUrlField = value;
			}
		}

		public ConfigProfilesLeagueSharpAssembly()
		{
		}
	}
}