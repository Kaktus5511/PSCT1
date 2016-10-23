using log4net;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Loader.Services
{
	public class Eudyptula : Singleton<Eudyptula>
	{
		private readonly static ILog Log;

		public string[] GameInstance
		{
			get;
			set;
		}

		public CancellationTokenSource TaskToken
		{
			get;
			set;
		}

		public Task Thread
		{
			get;
			set;
		}

		static Eudyptula()
		{
			Eudyptula.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public Eudyptula()
		{
		}

		private static string[] GetCommandline(Process process)
		{
			try
			{
				using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("SELECT CommandLine FROM Win32_Process WHERE ProcessId = {0}", process.Id)))
				{
					using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = searcher.Get().GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							string str = enumerator.Current["CommandLine"].ToString();
							int fileEnd = str.IndexOf("\"", 1);
							string file = str.Substring(1, fileEnd - 1);
							string args = str.Substring(fileEnd + 2);
							return new string[] { file, args };
						}
					}
				}
			}
			catch (Exception exception)
			{
				Eudyptula.Log.Error(exception);
			}
			return null;
		}

		public void Start()
		{
			if (this.Thread != null)
			{
				return;
			}
			this.TaskToken = new CancellationTokenSource();
			this.Thread = Task.Factory.StartNew<Task>(async () => {
				Eudyptula.<<Start>b__13_0>d variable = new Eudyptula.<<Start>b__13_0>d();
				variable.<>4__this = this;
				variable.<>t__builder = AsyncTaskMethodBuilder.Create();
				variable.<>1__state = -1;
				variable.<>t__builder.Start<Eudyptula.<<Start>b__13_0>d>(ref variable);
				return variable.<>t__builder.Task;
			}, this.TaskToken.Token);
		}

		private static void StartProcess(string[] args)
		{
			Process.Start(new ProcessStartInfo(args[0])
			{
				WorkingDirectory = Path.GetDirectoryName(args[0]),
				Arguments = args[1]
			});
		}

		public void Stop()
		{
			CancellationTokenSource taskToken = this.TaskToken;
			if (taskToken == null)
			{
				return;
			}
			taskToken.Cancel();
		}
	}
}