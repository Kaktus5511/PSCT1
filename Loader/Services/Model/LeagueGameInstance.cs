using Loader.Model;
using Loader.Model.Config;
using log4net;
using PlaySharp.Toolkit.Injector;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Remoting;
using PlaySharp.Toolkit.Remoting.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Loader.Services.Model
{
	public class LeagueGameInstance : GameInstanceBase
	{
		private readonly static ILog Log;

		protected override CoreMemoryState CoreState
		{
			get
			{
				return (CoreMemoryState)this.SharedMemory.Data.State;
			}
		}

		public SharedMemory<LegacySharedMemoryLayout> SharedMemory
		{
			get;
			set;
		}

		static LeagueGameInstance()
		{
			LeagueGameInstance.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public LeagueGameInstance(System.Diagnostics.Process process) : base(process, PlaySharp.Toolkit.Injector.ProcessMode.X86)
		{
		}

		protected override bool CreateSharedMemory()
		{
			bool flag;
			try
			{
				LeagueGameInstance.Log.Debug(string.Format("Create SharedMemory @ {0}", base.Config.Value.ServiceSettings.SharedMemoryName));
				this.SharedMemory = new SharedMemory<LegacySharedMemoryLayout>(base.Config.Value.ServiceSettings.SharedMemoryName, 0, true)
				{
					Data = new LegacySharedMemoryLayout(Files.Randomized.AppDomain.FullName, Files.Randomized.Bootstrap.FullName, base.Config.Value.Username, base.Config.Value.AuthKey, 0)
				};
				flag = true;
			}
			catch (Exception exception)
			{
				LeagueGameInstance.Log.Error(exception);
				flag = false;
			}
			return flag;
		}

		protected override bool InjectClr()
		{
			return true;
		}

		public override string ToString()
		{
			return string.Format("[League #{0}]", base.Pid);
		}
	}
}