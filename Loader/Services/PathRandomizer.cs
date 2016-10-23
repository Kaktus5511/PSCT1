using Loader.Model;
using Loader.Model.Config;
using log4net;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.StrongName;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Loader.Services
{
	internal class PathRandomizer
	{
		private readonly static ILog Log;

		static PathRandomizer()
		{
			PathRandomizer.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public PathRandomizer()
		{
		}

		public static bool CopyFiles()
		{
			PlaySharpConfig config = IoC.Get<PlaySharpConfig>(null);
			if (string.IsNullOrEmpty(config.ServiceSettings.CoreFileName))
			{
				PathRandomizer.Log.Warn(string.Format("Invalid {0}", "CoreFileName"));
				return false;
			}
			if (string.IsNullOrEmpty(config.ServiceSettings.CoreBridgeFileName))
			{
				PathRandomizer.Log.Warn(string.Format("Invalid {0}", "CoreBridgeFileName"));
				return false;
			}
			Files.Refresh();
			if (!Files.Core.Exists)
			{
				PathRandomizer.Log.Fatal(string.Format("Missing {0}", Files.Core));
				return false;
			}
			if (!Files.CoreBridge.Exists)
			{
				PathRandomizer.Log.Fatal(string.Format("Missing {0}", Files.CoreBridge));
				return false;
			}
			try
			{
				Files.Randomized.AppDomain.DeleteFile();
				Files.Randomized.Bootstrap.DeleteFile();
				Files.Randomized.Core.DeleteFile();
				Files.Randomized.CoreBridge.DeleteFile();
			}
			catch
			{
			}
			Utility.CopyAssembly(Files.AppDomain.FullName, Files.Randomized.AppDomain.FullName);
			Utility.CopyAssembly(Files.Bootstrap.FullName, Files.Randomized.Bootstrap.FullName);
			Utility.CopyAssembly(Files.Core.FullName, Files.Randomized.Core.FullName);
			byte[] data = Utility.ReplaceFilling(Files.CoreBridge, Files.Core.Name, Files.Randomized.Core.Name);
			File.WriteAllBytes(Files.Randomized.CoreBridge.FullName, data);
			try
			{
				StrongNameUtility.ReSign(Files.Randomized.CoreBridge.FullName, Utility.ReadResource("key.snk", Assembly.GetExecutingAssembly()), null);
				StrongNameUtility.Verify(Files.Randomized.CoreBridge.FullName, true);
			}
			catch (Exception exception)
			{
				PathRandomizer.Log.Warn(exception);
			}
			return true;
		}

		public static async Task<bool> CopyFilesAsync()
		{
			bool flag = await Task.Factory.StartNew<bool>(new Func<bool>(PathRandomizer.CopyFiles));
			return flag;
		}
	}
}