using Loader.Model.Config;
using log4net;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Loader.Model
{
	public static class Files
	{
		private readonly static ILog Log;

		private static PlaySharpConfig config;

		public static FileInfo AppDomain
		{
			get;
			private set;
		}

		public static FileInfo Bootstrap
		{
			get;
			private set;
		}

		private static PlaySharpConfig Config
		{
			get
			{
				if (Files.config == null)
				{
					Files.config = IoC.Get<PlaySharpConfig>(null);
				}
				return Files.config;
			}
		}

		public static FileInfo Core
		{
			get;
			private set;
		}

		public static FileInfo CoreBridge
		{
			get;
			private set;
		}

		public static System.Version Version
		{
			get;
			private set;
		}

		static Files()
		{
			Files.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			Files.Invalidate();
		}

		public static void Invalidate()
		{
			try
			{
				Files.Version = Assembly.GetExecutingAssembly().GetName().Version;
				if (Files.Config.ServiceSettings != null)
				{
					Files.AppDomain = new FileInfo(Path.Combine(Directories.System, Files.Config.ServiceSettings.AppDomainFileName));
					Files.Bootstrap = new FileInfo(Path.Combine(Directories.System, Files.Config.ServiceSettings.BootstrapFileName));
					Files.Core = new FileInfo(Path.Combine(Directories.System, Files.Config.ServiceSettings.CoreFileName));
					Files.CoreBridge = new FileInfo(Path.Combine(Directories.References, Files.Config.ServiceSettings.CoreBridgeFileName));
					Files.Randomized.Invalidate();
				}
				else
				{
					Files.Log.Warn(string.Format("{0} is null", "ServiceSettings"));
				}
			}
			catch (Exception exception)
			{
				Files.Log.Warn(exception);
			}
		}

		public static void Refresh()
		{
			Files.AppDomain.Refresh();
			Files.Bootstrap.Refresh();
			Files.Core.Refresh();
			Files.CoreBridge.Refresh();
			Files.Randomized.Refresh();
		}

		public static class Randomized
		{
			public static FileInfo AppDomain
			{
				get;
				private set;
			}

			public static FileInfo Bootstrap
			{
				get;
				private set;
			}

			public static FileInfo Core
			{
				get;
				private set;
			}

			public static FileInfo CoreBridge
			{
				get;
				private set;
			}

			public static void Invalidate()
			{
				Files.Randomized.AppDomain = new FileInfo(Path.Combine(Directories.Assemblies, Utility.GetUniqueFile(Files.AppDomain)));
				Files.Randomized.Bootstrap = new FileInfo(Path.Combine(Directories.Assemblies, Utility.GetUniqueFile(Files.Bootstrap)));
				Files.Randomized.Core = new FileInfo(Path.Combine(Directories.Assemblies, Utility.GetUniqueFile(Files.Core)));
				Files.Randomized.CoreBridge = new FileInfo(Path.Combine(Directories.Assemblies, Utility.GetUniqueFile(Files.CoreBridge)));
			}

			public static void Refresh()
			{
				Files.Randomized.AppDomain.Refresh();
				Files.Randomized.Bootstrap.Refresh();
				Files.Randomized.Core.Refresh();
				Files.Randomized.CoreBridge.Refresh();
			}
		}

		public static class X64
		{
			public static FileInfo Injector
			{
				get;
			}

			public static FileInfo InjectorNative
			{
				get;
			}

			static X64()
			{
				Files.X64.Injector = new FileInfo(Path.Combine(Directories.System, "PlaySharp.Injector.X64.exe"));
				Files.X64.InjectorNative = new FileInfo(Path.Combine(Directories.System, "PlaySharp.Injector.Native.X64.dll"));
			}
		}

		public static class X86
		{
			public static FileInfo Injector
			{
				get;
			}

			public static FileInfo InjectorNative
			{
				get;
			}

			static X86()
			{
				Files.X86.Injector = new FileInfo(Path.Combine(Directories.System, "PlaySharp.Injector.X86.exe"));
				Files.X86.InjectorNative = new FileInfo(Path.Combine(Directories.System, "PlaySharp.Injector.Native.X86.dll"));
			}
		}
	}
}