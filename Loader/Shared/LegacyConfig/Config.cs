using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Loader.Shared.LegacyConfig
{
	[DesignerCategory("code")]
	[Serializable]
	[XmlRoot(Namespace="", IsNullable=false)]
	[XmlType(AnonymousType=true)]
	public class Config
	{
		private string authKeyField;

		private object blockedRepositoriesField;

		private bool championCheckField;

		private byte columnCheckWidthField;

		private decimal columnLocationWidthField;

		private decimal columnNameWidthField;

		private byte columnTypeWidthField;

		private byte columnVersionWidthField;

		private bool enableDebugField;

		private bool firstRunField;

		private ConfigHotkeys hotkeysField;

		private bool installField;

		private string leagueOfLegendsExePathField;

		private bool libraryCheckField;

		private string passwordField;

		private ConfigProfiles[] profilesField;

		private string randomNameField;

		private string selectedColorField;

		private string selectedLanguageField;

		private byte selectedProfileIdField;

		private ConfigSettings settingsField;

		private bool showDevOptionsField;

		private bool tosAcceptedField;

		private bool updateCoreOnInjectField;

		private bool updateOnLoadField;

		private bool useCloudConfigField;

		private string usernameField;

		private bool utilityCheckField;

		private ushort windowHeightField;

		private short windowLeftField;

		private ushort windowTopField;

		private ushort windowWidthField;

		private byte workersField;

		public string AuthKey
		{
			get
			{
				return this.authKeyField;
			}
			set
			{
				this.authKeyField = value;
			}
		}

		public object BlockedRepositories
		{
			get
			{
				return this.blockedRepositoriesField;
			}
			set
			{
				this.blockedRepositoriesField = value;
			}
		}

		public bool ChampionCheck
		{
			get
			{
				return this.championCheckField;
			}
			set
			{
				this.championCheckField = value;
			}
		}

		public byte ColumnCheckWidth
		{
			get
			{
				return this.columnCheckWidthField;
			}
			set
			{
				this.columnCheckWidthField = value;
			}
		}

		public decimal ColumnLocationWidth
		{
			get
			{
				return this.columnLocationWidthField;
			}
			set
			{
				this.columnLocationWidthField = value;
			}
		}

		public decimal ColumnNameWidth
		{
			get
			{
				return this.columnNameWidthField;
			}
			set
			{
				this.columnNameWidthField = value;
			}
		}

		public byte ColumnTypeWidth
		{
			get
			{
				return this.columnTypeWidthField;
			}
			set
			{
				this.columnTypeWidthField = value;
			}
		}

		public byte ColumnVersionWidth
		{
			get
			{
				return this.columnVersionWidthField;
			}
			set
			{
				this.columnVersionWidthField = value;
			}
		}

		public bool EnableDebug
		{
			get
			{
				return this.enableDebugField;
			}
			set
			{
				this.enableDebugField = value;
			}
		}

		public bool FirstRun
		{
			get
			{
				return this.firstRunField;
			}
			set
			{
				this.firstRunField = value;
			}
		}

		public ConfigHotkeys Hotkeys
		{
			get
			{
				return this.hotkeysField;
			}
			set
			{
				this.hotkeysField = value;
			}
		}

		public bool Install
		{
			get
			{
				return this.installField;
			}
			set
			{
				this.installField = value;
			}
		}

		public string LeagueOfLegendsExePath
		{
			get
			{
				return this.leagueOfLegendsExePathField;
			}
			set
			{
				this.leagueOfLegendsExePathField = value;
			}
		}

		public bool LibraryCheck
		{
			get
			{
				return this.libraryCheckField;
			}
			set
			{
				this.libraryCheckField = value;
			}
		}

		public string Password
		{
			get
			{
				return this.passwordField;
			}
			set
			{
				this.passwordField = value;
			}
		}

		[XmlArrayItem("Profiles", IsNullable=false)]
		public ConfigProfiles[] Profiles
		{
			get
			{
				return this.profilesField;
			}
			set
			{
				this.profilesField = value;
			}
		}

		public string RandomName
		{
			get
			{
				return this.randomNameField;
			}
			set
			{
				this.randomNameField = value;
			}
		}

		public string SelectedColor
		{
			get
			{
				return this.selectedColorField;
			}
			set
			{
				this.selectedColorField = value;
			}
		}

		public string SelectedLanguage
		{
			get
			{
				return this.selectedLanguageField;
			}
			set
			{
				this.selectedLanguageField = value;
			}
		}

		public byte SelectedProfileId
		{
			get
			{
				return this.selectedProfileIdField;
			}
			set
			{
				this.selectedProfileIdField = value;
			}
		}

		public ConfigSettings Settings
		{
			get
			{
				return this.settingsField;
			}
			set
			{
				this.settingsField = value;
			}
		}

		public bool ShowDevOptions
		{
			get
			{
				return this.showDevOptionsField;
			}
			set
			{
				this.showDevOptionsField = value;
			}
		}

		public bool TosAccepted
		{
			get
			{
				return this.tosAcceptedField;
			}
			set
			{
				this.tosAcceptedField = value;
			}
		}

		public bool UpdateCoreOnInject
		{
			get
			{
				return this.updateCoreOnInjectField;
			}
			set
			{
				this.updateCoreOnInjectField = value;
			}
		}

		public bool UpdateOnLoad
		{
			get
			{
				return this.updateOnLoadField;
			}
			set
			{
				this.updateOnLoadField = value;
			}
		}

		public bool UseCloudConfig
		{
			get
			{
				return this.useCloudConfigField;
			}
			set
			{
				this.useCloudConfigField = value;
			}
		}

		public string Username
		{
			get
			{
				return this.usernameField;
			}
			set
			{
				this.usernameField = value;
			}
		}

		public bool UtilityCheck
		{
			get
			{
				return this.utilityCheckField;
			}
			set
			{
				this.utilityCheckField = value;
			}
		}

		public ushort WindowHeight
		{
			get
			{
				return this.windowHeightField;
			}
			set
			{
				this.windowHeightField = value;
			}
		}

		public short WindowLeft
		{
			get
			{
				return this.windowLeftField;
			}
			set
			{
				this.windowLeftField = value;
			}
		}

		public ushort WindowTop
		{
			get
			{
				return this.windowTopField;
			}
			set
			{
				this.windowTopField = value;
			}
		}

		public ushort WindowWidth
		{
			get
			{
				return this.windowWidthField;
			}
			set
			{
				this.windowWidthField = value;
			}
		}

		public byte Workers
		{
			get
			{
				return this.workersField;
			}
			set
			{
				this.workersField = value;
			}
		}

		public Config()
		{
		}
	}
}