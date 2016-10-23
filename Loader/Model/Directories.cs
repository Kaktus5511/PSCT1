using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Loader.Model
{
	public static class Directories
	{
		public static string AppData
		{
			get;
		}

		public static string Assemblies
		{
			get;
		}

		public static string Cache
		{
			get;
		}

		public static string Config
		{
			get;
		}

		public static string Core
		{
			get;
		}

		public static string Current
		{
			get;
		}

		public static string Logs
		{
			get;
		}

		public static string Packages
		{
			get;
		}

		public static string Plugins
		{
			get;
			set;
		}

		public static string References
		{
			get;
		}

		public static string Repositories
		{
			get;
		}

		public static string System
		{
			get;
		}

		static Directories()
		{
			Directories.Current = AppDomain.CurrentDomain.BaseDirectory;
			Directories.Core = Path.Combine(Directories.Current, "Core");
			Directories.Plugins = Path.Combine(Directories.Current, "Plugins");
			Directories.System = Path.Combine(Directories.Current, "System");
			Directories.References = Path.Combine(Directories.Current, "References");
			Directories.Config = Path.Combine(Directories.Current, "Config");
			Directories.Cache = Path.Combine(Directories.Current, "Cache");
			Directories.Logs = Path.Combine(Directories.Current, "Logs");
			Directories.Assemblies = Path.Combine(Directories.Cache, "assemblies");
			Directories.Packages = Path.Combine(Directories.Cache, "packages");
			Directories.Repositories = Path.Combine(Directories.Cache, "repositories");
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			int hashCode = Environment.UserName.GetHashCode();
			Directories.AppData = Path.Combine(folderPath, string.Concat("LS", hashCode.ToString("X")));
			Directories.CreateDirectories();
		}

		public static void ClearCache()
		{
			try
			{
				if (Directory.Exists(Directories.Cache))
				{
					Directory.Delete(Directories.Cache, true);
				}
			}
			catch
			{
			}
			Directories.CreateDirectories();
		}

		public static void CreateDirectories()
		{
			try
			{
				Directory.CreateDirectory(Directories.Logs);
				Directory.CreateDirectory(Directories.Config);
				Directory.CreateDirectory(Directories.Cache);
				Directory.CreateDirectory(Directories.Assemblies);
				Directory.CreateDirectory(Directories.Packages);
				Directory.CreateDirectory(Directories.Repositories);
				Directory.CreateDirectory(Directories.AppData);
			}
			catch
			{
			}
		}
	}
}