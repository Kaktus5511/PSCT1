using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Loader.Shared
{
	[DataContract]
	[Serializable]
	public class Configuration
	{
		[DataMember]
		public string AppDataDirectory
		{
			get;
			set;
		}

		[DataMember]
		public string CacheDirectory
		{
			get;
			set;
		}

		[DataMember]
		public string ConfigDirectory
		{
			get;
			set;
		}

		[DataMember]
		public string CoreBridgeFilePath
		{
			get;
			set;
		}

		[DataMember]
		public string GameDirectory
		{
			get;
			set;
		}

		[DataMember]
		public IDictionary<string, string> GameSettings
		{
			get;
			set;
		}

		[DataMember]
		public IDictionary<string, int> HotKeys
		{
			get;
			set;
		}

		[DataMember]
		public string Language
		{
			get;
			set;
		}

		[DataMember]
		public string LoaderDirectory
		{
			get;
			set;
		}

		[DataMember]
		public int LoaderProcessId
		{
			get;
			set;
		}

		[DataMember]
		public string LogDirectory
		{
			get;
			set;
		}

		[DataMember]
		public IDictionary<string, string> Settings
		{
			get;
			set;
		}

		[DataMember]
		public bool UseSandbox
		{
			get;
			set;
		}

		[DataMember]
		public IEnumerable<string> WebPermissions
		{
			get;
			set;
		}

		public Configuration()
		{
			this.WebPermissions = new List<string>();
			this.GameSettings = new Dictionary<string, string>();
			this.Settings = new Dictionary<string, string>();
			this.HotKeys = new Dictionary<string, int>();
		}
	}
}