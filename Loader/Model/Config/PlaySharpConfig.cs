using Loader.Model;
using Loader.Properties;
using Loader.Services.Model;
using Loader.Shared.LegacyConfig;
using log4net;
using Newtonsoft.Json;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Extensions.Model;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Messages;
using PlaySharp.Toolkit.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Loader.Model.Config
{
	public class PlaySharpConfig
	{
		private readonly static ILog Log;

		public string AuthKey
		{
			get;
			set;
		}

		[JsonIgnore]
		public static string FilePath
		{
			get
			{
				return Path.Combine(Directories.Config, "loader.json");
			}
		}

		[JsonIgnore]
		public int Id
		{
			get;
			set;
		}

		public bool IsFirstStart { get; set; } = true;

		[JsonConverter(typeof(UnixTimestampConverter))]
		public DateTime Modified { get; set; } = DateTime.UtcNow;

		public string Name
		{
			get;
			set;
		}

		public ServiceType Service { get; set; } = ServiceType.LoL;

		[JsonIgnore]
		public string ServiceName
		{
			get
			{
				ServiceType service = this.Service;
				return string.Format("loader.{0}", service.ToString().ToLower());
			}
		}

		[JsonIgnore]
		public IServiceSettings ServiceSettings
		{
			get;
			set;
		}

		public ISettings Settings { get; set; } = new Loader.Model.Config.Settings();

		public bool UseCloud { get; set; } = true;

		[JsonIgnore]
		public string Username
		{
			get;
			set;
		}

		static PlaySharpConfig()
		{
			PlaySharpConfig.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public PlaySharpConfig()
		{
		}

		public async Task Import(string file)
		{
			PlaySharpConfig.<Import>d__46 variable = new PlaySharpConfig.<Import>d__46();
			variable.<>4__this = this;
			variable.file = file;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<PlaySharpConfig.<Import>d__46>(ref variable);
			return variable.<>t__builder.Task;
		}

		public static PlaySharpConfig Init()
		{
			return ConfigFactory.Load<PlaySharpConfig>("loader").WithFile(null).Execute();
		}

		public void Load(bool useCloud = false)
		{
			ConfigFactory.ConfigLoad<PlaySharpConfig> settingsFactory = ConfigFactory.Load<PlaySharpConfig>("loader");
			ConfigFactory.ConfigLoad<Loader.Model.Config.ServiceSettings> serviceSettingsFactory = ConfigFactory.Load<Loader.Model.Config.ServiceSettings>(this.ServiceName);
			settingsFactory.WithFile(null);
			serviceSettingsFactory.WithFile(null);
			serviceSettingsFactory.WithResource(null, null);
			if (useCloud)
			{
				settingsFactory.WithCloud();
				serviceSettingsFactory.WithCloud();
			}
			this.Settings = settingsFactory.Execute().Settings;
			this.ServiceSettings = serviceSettingsFactory.Execute();
		}

		public void Save(bool useCloud = false)
		{
			ConfigFactory.ConfigSave<PlaySharpConfig> configFactory = ConfigFactory.Save<PlaySharpConfig>("loader", this);
			ConfigFactory.ConfigSave<IServiceSettings> serviceSettingsFactory = ConfigFactory.Save<IServiceSettings>(this.ServiceName, this.ServiceSettings);
			this.Modified = DateTime.UtcNow;
			configFactory.WithModified(this.Modified);
			serviceSettingsFactory.WithModified(this.Modified);
			configFactory.WithFile();
			serviceSettingsFactory.WithFile();
			if (useCloud)
			{
				configFactory.WithCloud();
				serviceSettingsFactory.WithCloud();
			}
			configFactory.Execute();
			serviceSettingsFactory.Execute();
		}
	}
}