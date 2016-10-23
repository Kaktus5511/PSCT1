using Loader.Model;
using PlaySharp.Service.WebService.Model;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Loader.Model.Config
{
	public class Settings : ISettings
	{
		public EndpointEntry ApiEndpoint
		{
			get;
			set;
		}

		public bool AssemblyDebug { get; set; } = true;

		public bool BypassCoreUpdate
		{
			get
			{
				if (Directory.Exists(Path.Combine(Directories.Current, "iwanttogetbanned")))
				{
					return true;
				}
				return false;
			}
		}

		public bool ChampionCheck { get; set; } = true;

		public string Color { get; set; } = "Cobalt";

		public DataGridLength ColumnAuthorWidth { get; set; } = 75;

		public DataGridLength ColumnCheckWidth { get; set; } = 40;

		public DataGridLength ColumnNameWidth { get; set; } = 180;

		public DataGridLength ColumnTypeWidth { get; set; } = 75;

		public DataGridLength ColumnVersionWidth { get; set; } = 90;

		public EndpointEntry DataEndpoint
		{
			get;
			set;
		}

		public string Language { get; set; } = string.Empty;

		public bool LibraryCheck { get; set; } = true;

		public ReleaseChannel LoaderUpdateChannel { get; set; } = ReleaseChannel.Beta;

		public double ProfileCollectionWidth { get; set; } = 165;

		public bool ShowDescription { get; set; } = true;

		public bool ShowNotes
		{
			get;
			set;
		}

		public string Theme { get; set; } = "BaseLight";

		public bool TosAccepted { get; set; } = true;

		public bool UpdateSelectedAssembliesOnly { get; set; } = true;

		public bool UsePlaySharpEnvironment
		{
			get;
			set;
		}

		public Loader.Model.UserType UserType { get; set; } = Loader.Model.UserType.Advanced;

		public bool UseSandbox { get; set; } = true;

		public bool UtilityCheck { get; set; } = true;

		public double WindowHeight { get; set; } = 500;

		public double WindowLeft { get; set; } = 100;

		public double WindowTop { get; set; } = 100;

		public double WindowWidth { get; set; } = 800;

		public int Workers { get; set; } = Environment.ProcessorCount;

		public Settings()
		{
		}
	}
}