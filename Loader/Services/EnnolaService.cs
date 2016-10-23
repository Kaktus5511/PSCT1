using Ionic.Zip;
using Loader.Model;
using Loader.Model.Config;
using Loader.Services.Model;
using Loader.ViewModels;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Service.Messages;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Injector;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Messages;
using PlaySharp.Toolkit.ProcessResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loader.Services
{
	public class EnnolaService : Singleton<EnnolaService>, IHandle<OnProcessCreated>, IHandle, IHandle<OnProcessDestroyed>
	{
		private readonly static ILog Log;

		[Import(typeof(PlaySharpConfig))]
		public Lazy<PlaySharpConfig> Config
		{
			get;
			set;
		}

		public string ConfigFilePath { get; } = Path.Combine(Directories.Config, "ennola.ini");

		public string EnnolaChecksum
		{
			get
			{
				return HashFactory.MD5.File(this.EnnolaFilePath);
			}
		}

		public string EnnolaFilePath
		{
			get
			{
				return Path.Combine(Directories.System, "ennola.sem");
			}
		}

		[Import(typeof(IEventAggregator))]
		public Lazy<IEventAggregator> EventAggregator
		{
			get;
			set;
		}

		public InjectorClient Injector
		{
			get
			{
				return InjectorWin32.Instance;
			}
		}

		public bool IsInitialized
		{
			get;
			private set;
		}

		[Import(typeof(IBusyOverlayView))]
		public Lazy<IBusyOverlayView> Overlay
		{
			get;
			set;
		}

		[Import(typeof(IProcessResolver))]
		public Lazy<IProcessResolver> ProcessResolver
		{
			get;
			set;
		}

		[Import(typeof(IWebServiceClient))]
		public Lazy<IWebServiceClient> ServiceClient
		{
			get;
			set;
		}

		public List<int> Services { get; set; } = new List<int>();

		public CancellationTokenSource TaskToken
		{
			get;
			set;
		}

		static EnnolaService()
		{
			EnnolaService.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public EnnolaService()
		{
			IoC.BuildUp(this);
		}

		public void Handle(OnProcessCreated message)
		{
		}

		public void Handle(OnProcessDestroyed message)
		{
			int messageId;
			this.Services.RemoveAll((int id) => id == message.Process.Id);
			if (message.Process.Name == "dota2")
			{
				INIFile nIFile = new INIFile(this.ConfigFilePath, false);
				string messageValue = nIFile.GetValue("ennola", "message", false);
				string messageText = string.Empty;
				if (!int.TryParse(messageValue, out messageId))
				{
					EnnolaService.Log.Warn(string.Format("Failed to parse Ennola Message {0}", messageValue));
					messageText = messageValue;
				}
				else if (messageId != 0)
				{
					messageText = Loc.GetValue(string.Format("Message.Ennola.{0}", messageId));
				}
				if (!messageText.IsNullOrEmpty())
				{
					EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage("Ennola", messageText));
				}
				nIFile.SetValue("ennola", "message", "0", false);
			}
		}

		public bool IsLoaded()
		{
			bool flag;
			try
			{
				Process[] processesByName = Process.GetProcessesByName("Steam");
				int num = 0;
				while (num < (int)processesByName.Length)
				{
					Process process = processesByName[num];
					if (this.Injector.HasModule(process.Id, this.EnnolaFilePath, ModuleType.SEM) != InjectorResult.SUCCESS)
					{
						num++;
					}
					else
					{
						flag = true;
						return flag;
					}
				}
				processesByName = Process.GetProcessesByName("SteamService");
				num = 0;
				while (num < (int)processesByName.Length)
				{
					Process process = processesByName[num];
					if (this.Injector.HasModule(process.Id, this.EnnolaFilePath, ModuleType.SEM) != InjectorResult.SUCCESS)
					{
						num++;
					}
					else
					{
						flag = true;
						return flag;
					}
				}
				return false;
			}
			catch (Exception exception)
			{
				EnnolaService.Log.Error(exception);
				return false;
			}
			return flag;
		}

		private void Kill()
		{
			int i;
			if (this.Config.Value.Settings.BypassCoreUpdate)
			{
				EnnolaService.Log.Warn("I-WANT-TO-GET-BANNED");
				return;
			}
			bool killed = false;
			Process[] processesByName = Process.GetProcessesByName("dota2");
			for (i = 0; i < (int)processesByName.Length; i++)
			{
				Process process = processesByName[i];
				EnnolaService.Log.Debug(string.Format("Killing Dota2 {0}", process.Id));
				killed = true;
				process.Kill();
			}
			if (killed)
			{
				processesByName = Process.GetProcessesByName("steam");
				for (i = 0; i < (int)processesByName.Length; i++)
				{
					Process process = processesByName[i];
					EnnolaService.Log.Debug(string.Format("Killing Steam {0}", process.Id));
					process.Kill();
				}
				MessageBox.Show("Killed Steam/Dota2 to load [Ennola] Security System", "Ennola Service", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private async Task<bool> LoadAsync(int pid)
		{
			bool flag;
			bool flag1 = false;
			await this.UpdateEnnolaAsync();
			try
			{
				EnnolaService.Log.Debug(string.Format("Checking for Ennola {0}", pid));
				InjectorResult injectorResult1 = this.Injector.HasModule(pid, this.EnnolaFilePath, ModuleType.SEM);
				switch (injectorResult1)
				{
					case InjectorResult.FAILED:
					{
						InjectorResult injectorResult2 = this.Injector.HasModule(pid, "SteamService.dll", ModuleType.PE);
						switch (injectorResult2)
						{
							case InjectorResult.FAILED:
							{
								EnnolaService.Log.Warn("Could not find SteamService.dll");
								flag1 = false;
								this.IsInitialized = false;
								flag = false;
								return flag;
							}
							case InjectorResult.SUCCESS:
							{
								this.Kill();
								await this.Overlay.Value.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
									model.Text = "Injecting Ennola";
									EnnolaService.Log.Debug(string.Format("Injecting Ennola into {0}", pid));
									InjectorResult injectorResult = this.Injector.Inject(pid, this.EnnolaFilePath, ModuleType.SEM);
									switch (injectorResult)
									{
										case InjectorResult.FAILED:
										case InjectorResult.OPEN_PROCESS_FAILED:
										case InjectorResult.VIRTUAL_ALLOC_FAILED:
										case InjectorResult.WRITE_PROCESS_MEMORY_FAILED:
										case InjectorResult.CREATE_REMOTE_THREAD_FAILED:
										{
											EnnolaService.Log.Error(string.Format("Failed to Load Ennola {0}", injectorResult));
											break;
										}
										case InjectorResult.SUCCESS:
										{
											model.Text = "Waiting for Ennola to finish bootstrap.";
											EnnolaService.Log.Debug("Waiting for Ennola to finish bootstrap.");
											await Task.Delay(250, token);
											for (int i = 0; i < 30; i++)
											{
												try
												{
													if (this.Injector.HasModule(pid, this.EnnolaFilePath, ModuleType.SEM) != InjectorResult.SUCCESS)
													{
														await Task.Delay(250, token);
													}
													else
													{
														break;
													}
												}
												catch (Exception exception)
												{
													EnnolaService.Log.Error(exception);
												}
											}
											this.UpdateConfig();
											this.UpdatePipe(pid);
											model.Text = "Successfully loaded [Ennola] Security System";
											EnnolaService.Log.Info(string.Format("Successfully loaded [Ennola] Security System into {0}", pid));
											await Task.Delay(2000, token);
											this.IsInitialized = true;
											flag1 = true;
											return;
										}
									}
									flag1 = false;
									this.IsInitialized = false;
								});
								break;
							}
							case InjectorResult.OPEN_PROCESS_FAILED:
							case InjectorResult.VIRTUAL_ALLOC_FAILED:
							case InjectorResult.WRITE_PROCESS_MEMORY_FAILED:
							case InjectorResult.CREATE_REMOTE_THREAD_FAILED:
							{
								EnnolaService.Log.Error(string.Format("Has SteamService {0}", injectorResult2));
								goto case InjectorResult.SUCCESS;
							}
							default:
							{
								goto case InjectorResult.SUCCESS;
							}
						}
						break;
					}
					case InjectorResult.SUCCESS:
					{
						this.IsInitialized = true;
						flag1 = true;
						await Task.Delay(2000);
						flag = true;
						return flag;
					}
					case InjectorResult.OPEN_PROCESS_FAILED:
					case InjectorResult.VIRTUAL_ALLOC_FAILED:
					case InjectorResult.WRITE_PROCESS_MEMORY_FAILED:
					case InjectorResult.CREATE_REMOTE_THREAD_FAILED:
					{
						EnnolaService.Log.Error(string.Format("Has Ennola {0}", injectorResult1));
						goto case InjectorResult.FAILED;
					}
					default:
					{
						goto case InjectorResult.FAILED;
					}
				}
			}
			catch (CommunicationObjectFaultedException communicationObjectFaultedException)
			{
				EnnolaService.Log.Error(communicationObjectFaultedException);
				this.IsInitialized = false;
				flag1 = false;
			}
			catch (Exception exception1)
			{
				EnnolaService.Log.Fatal(exception1);
				this.IsInitialized = false;
				flag1 = false;
			}
			flag = flag1;
			return flag;
		}

		public async Task PulseAsync()
		{
			int i;
			Process[] processesByName = Process.GetProcessesByName("Steam");
			Process[] processArray = Process.GetProcessesByName("SteamService");
			Process[] processArray1 = processesByName;
			int[] array = (
				from p in (IEnumerable<Process>)processArray1
				select p.Id).ToArray<int>();
			Process[] processArray2 = processArray;
			int[] numArray = (
				from p in (IEnumerable<Process>)processArray2
				select p.Id).ToArray<int>();
			Process[] processArray3 = processesByName;
			for (i = 0; i < (int)processArray3.Length; i++)
			{
				Process process1 = processArray3[i];
				if (!this.Services.Contains(process1.Id))
				{
					if (await this.LoadAsync(process1.Id))
					{
						this.Services.AddRange(array);
						this.Services.AddRange(numArray);
					}
				}
			}
			processArray3 = null;
			processArray3 = processArray;
			for (i = 0; i < (int)processArray3.Length; i++)
			{
				Process process = processArray3[i];
				if (process != null && !this.Services.Contains(process.Id))
				{
					if (await this.LoadAsync(process.Id))
					{
						this.Services.AddRange(array);
						this.Services.AddRange(numArray);
					}
				}
			}
			processArray3 = null;
		}

		public void Start()
		{
			if (this.TaskToken != null)
			{
				return;
			}
			this.EventAggregator.Value.Subscribe(this);
			this.ProcessResolver.Value.AddProcessFilter("dota2");
			this.ProcessResolver.Value.AddProcessFilter("Steam");
			this.ProcessResolver.Value.AddProcessFilter("SteamService");
			this.ProcessResolver.Value.Activate();
			this.TaskToken = new CancellationTokenSource();
			Task.Factory.StartNew<Task>(async () => {
				EnnolaService.<<Start>b__47_0>d variable = new EnnolaService.<<Start>b__47_0>d();
				variable.<>4__this = this;
				variable.<>t__builder = AsyncTaskMethodBuilder.Create();
				variable.<>1__state = -1;
				variable.<>t__builder.Start<EnnolaService.<<Start>b__47_0>d>(ref variable);
				return variable.<>t__builder.Task;
			}, TaskCreationOptions.LongRunning);
		}

		public void Stop()
		{
			this.EventAggregator.Value.Unsubscribe(this);
			this.ProcessResolver.Value.Deactivate();
			CancellationTokenSource taskToken = this.TaskToken;
			if (taskToken == null)
			{
				return;
			}
			taskToken.Cancel();
		}

		public void UpdateConfig()
		{
			try
			{
				EnnolaService.Log.Debug("Update Ennola config");
				INIFile nIFile = new INIFile(this.ConfigFilePath, true);
				string value = this.Config.Value.GetOrCreateGameSetting("DisableKillswitch", "0").SelectedValue;
				nIFile.SetValue("ennola", "killswitchdisabled", value, false);
				nIFile.SetValue("ennola", "message", "0", false);
			}
			catch (Exception exception)
			{
				EnnolaService.Log.Error(exception);
			}
		}

		private async Task UpdateEnnolaAsync()
		{
			EnnolaService.<UpdateEnnolaAsync>d__52 variable = new EnnolaService.<UpdateEnnolaAsync>d__52();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<EnnolaService.<UpdateEnnolaAsync>d__52>(ref variable);
			return variable.<>t__builder.Task;
		}

		private void UpdatePipe(int pid)
		{
			try
			{
				EnnolaService.Log.Debug(string.Format("Update ennola{0} '{1}'", pid, this.ConfigFilePath));
				using (NamedPipeClientStream pipe = new NamedPipeClientStream(".", string.Format("ennola{0}", pid), PipeDirection.Out))
				{
					pipe.Connect(5000);
					byte[] data = Encoding.Unicode.GetBytes(this.ConfigFilePath);
					pipe.Write(data, 0, (int)data.Length);
					pipe.Flush();
				}
			}
			catch (Exception exception)
			{
				EnnolaService.Log.Warn(exception);
			}
		}
	}
}