using Loader.Model;
using Loader.Model.Config;
using log4net;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Injector;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Messages;
using PlaySharp.Toolkit.Remoting;
using PlaySharp.Toolkit.Remoting.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Loader.Services.Model
{
	public class EnsageGameInstance : GameInstanceBase
	{
		private readonly static ILog Log;

		protected override CoreMemoryState CoreState
		{
			get
			{
				return (CoreMemoryState)this.SharedMemory.Data.State;
			}
		}

		public SharedMemory<PlaySharpMemoryLayout> SharedMemory
		{
			get;
			set;
		}

		static EnsageGameInstance()
		{
			EnsageGameInstance.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public EnsageGameInstance(System.Diagnostics.Process process) : base(process, PlaySharp.Toolkit.Injector.ProcessMode.X64)
		{
		}

		protected override bool CreateSharedMemory()
		{
			bool flag;
			try
			{
				EnsageGameInstance.Log.Debug(string.Format("Create SharedMemory @ Local\\shared-{0}", base.Pid));
				this.SharedMemory = new SharedMemory<PlaySharpMemoryLayout>(string.Format("Local\\shared-{0}", base.Pid), 0, true)
				{
					Data = new PlaySharpMemoryLayout(Files.Randomized.AppDomain.FullName, "EnsageSharp.Sandbox.Sandbox", "Bootstrap", "Ensage", "Ensage", Directories.Current, Directories.Assemblies, base.Config.Value.Username, base.Config.Value.AuthKey, base.ServiceClient.Value.LoginData.Token, 0)
				};
				flag = true;
			}
			catch (Exception exception)
			{
				EnsageGameInstance.Log.Error(exception);
				flag = false;
			}
			return flag;
		}

		protected override async Task<bool> IsSupportedAsync(CancellationToken token = null)
		{
			EnsageGameInstance.<IsSupportedAsync>d__10 variable = new EnsageGameInstance.<IsSupportedAsync>d__10();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<EnsageGameInstance.<IsSupportedAsync>d__10>(ref variable);
			return variable.<>t__builder.Task;
		}

		public override string ToString()
		{
			return string.Format("[Ensage #{0}]", base.Pid);
		}
	}
}