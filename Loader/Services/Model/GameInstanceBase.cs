using Loader.Model;
using Loader.Model.Config;
using Loader.Services;
using Loader.ViewModels;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Injector;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Messages;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Loader.Services.Model
{
	public abstract class GameInstanceBase : IGameInstance, IDisposable
	{
		private readonly static ILog Log;

		private GameInstanceState state;

		[Import(typeof(PlaySharpConfig))]
		public Lazy<PlaySharpConfig> Config
		{
			get;
			set;
		}

		protected abstract CoreMemoryState CoreState
		{
			get;
		}

		[Import(typeof(IEventAggregator))]
		public Lazy<IEventAggregator> EventAggregator
		{
			get;
			set;
		}

		[Import(typeof(IInjector))]
		public Lazy<IInjector> InjectionService
		{
			get;
			set;
		}

		protected PlaySharp.Toolkit.Injector.InjectorClient InjectorClient
		{
			get;
			set;
		}

		public bool IsAlive
		{
			get
			{
				return Extensions.IsRunning(this.Process);
			}
		}

		public bool IsCompleted
		{
			get
			{
				return JustDecompileGenerated_get_IsCompleted();
			}
			set
			{
				JustDecompileGenerated_set_IsCompleted(value);
			}
		}

		private bool JustDecompileGenerated_IsCompleted_k__BackingField;

		public virtual bool JustDecompileGenerated_get_IsCompleted()
		{
			return this.JustDecompileGenerated_IsCompleted_k__BackingField;
		}

		private void JustDecompileGenerated_set_IsCompleted(bool value)
		{
			this.JustDecompileGenerated_IsCompleted_k__BackingField = value;
		}

		public bool IsDisposed
		{
			get
			{
				return JustDecompileGenerated_get_IsDisposed();
			}
			set
			{
				JustDecompileGenerated_set_IsDisposed(value);
			}
		}

		private bool JustDecompileGenerated_IsDisposed_k__BackingField;

		public bool JustDecompileGenerated_get_IsDisposed()
		{
			return this.JustDecompileGenerated_IsDisposed_k__BackingField;
		}

		public void JustDecompileGenerated_set_IsDisposed(bool value)
		{
			this.JustDecompileGenerated_IsDisposed_k__BackingField = value;
		}

		protected string MainModuleFilePath
		{
			get;
			set;
		}

		[Import(typeof(IBusyOverlayView))]
		public Lazy<IBusyOverlayView> Overlay
		{
			get;
			set;
		}

		public int Pid
		{
			get
			{
				return this.Process.Id;
			}
		}

		public System.Diagnostics.Process Process
		{
			get;
		}

		protected PlaySharp.Toolkit.Injector.ProcessMode ProcessMode
		{
			get;
		}

		[Import(typeof(IWebServiceClient))]
		public Lazy<IWebServiceClient> ServiceClient
		{
			get;
			set;
		}

		public GameInstanceState State
		{
			get
			{
				return JustDecompileGenerated_get_State();
			}
			set
			{
				JustDecompileGenerated_set_State(value);
			}
		}

		public virtual GameInstanceState JustDecompileGenerated_get_State()
		{
			return this.state;
		}

		private void JustDecompileGenerated_set_State(GameInstanceState value)
		{
			GameInstanceBase.Log.Debug(string.Format("{0} {1} -> {2}", this, this.state, value));
			this.state = value;
		}

		protected CancellationTokenSource TokenSource
		{
			get;
			set;
		}

		[Import(typeof(IUpdateService))]
		public Lazy<IUpdateService> UpdateService
		{
			get;
			set;
		}

		static GameInstanceBase()
		{
			GameInstanceBase.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public GameInstanceBase(System.Diagnostics.Process process, PlaySharp.Toolkit.Injector.ProcessMode mode)
		{
			if (process == null)
			{
				throw new ArgumentNullException("process");
			}
			if (!Enum.IsDefined(typeof(PlaySharp.Toolkit.Injector.ProcessMode), mode))
			{
				throw new ArgumentOutOfRangeException("mode");
			}
			this.Process = process;
			this.ProcessMode = mode;
			IoC.BuildUp(this);
			GameInstanceBase gameInstanceBase = this;
			Task.Factory.StartNew(new Action(gameInstanceBase.RunAsync), TaskCreationOptions.LongRunning);
		}

		protected abstract bool CreateSharedMemory();

		public void Dispose()
		{
			try
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}
			catch (Exception exception)
			{
				GameInstanceBase.Log.Error(exception);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			PlaySharp.Toolkit.Injector.ProcessMode? nullable;
			PlaySharp.Toolkit.Injector.ProcessMode? nullable1;
			PlaySharp.Toolkit.Injector.ProcessMode? nullable2;
			PlaySharp.Toolkit.Injector.InjectorClient injectorClient = this.InjectorClient;
			if (injectorClient != null)
			{
				nullable1 = new PlaySharp.Toolkit.Injector.ProcessMode?(injectorClient.CurrentProcessMode);
			}
			else
			{
				nullable = null;
				nullable1 = nullable;
			}
			PlaySharp.Toolkit.Injector.ProcessMode? nullable3 = nullable1;
			PlaySharp.Toolkit.Injector.InjectorClient injectorClient1 = this.InjectorClient;
			if (injectorClient1 != null)
			{
				nullable2 = new PlaySharp.Toolkit.Injector.ProcessMode?(injectorClient1.Mode);
			}
			else
			{
				nullable = null;
				nullable2 = nullable;
			}
			PlaySharp.Toolkit.Injector.ProcessMode? nullable4 = nullable2;
			if ((nullable3.GetValueOrDefault() == nullable4.GetValueOrDefault() ? nullable3.HasValue != nullable4.HasValue : true))
			{
				this.InjectorClient.Dispose();
			}
			this.IsDisposed = true;
		}

		protected virtual bool Inject()
		{
			GameInstanceBase.Log.Debug("Injecting Core");
			this.State = GameInstanceState.Injecting;
			this.Verify();
			this.Overlay.Value.Text = string.Format("{0} Injecting Core", this);
			InjectorResult result = this.InjectorClient.Inject(this.Pid, Files.Randomized.Core.FullName, ModuleType.PE);
			if (result != InjectorResult.SUCCESS)
			{
				if (result == InjectorResult.SUCCESS_CN)
				{
					this.State = GameInstanceState.Injected;
					throw new OperationCanceledException("CN Injection");
				}
				GameInstanceBase.Log.Error(string.Format("{0} Core injection failed with {1}", this, result));
				this.State = GameInstanceState.InjectionError;
				this.Process.Kill();
				throw new GameInstanceException(this, result);
			}
			GameInstanceBase.Log.Info(string.Format("{0} Core injection successful", this));
			this.State = GameInstanceState.Injected;
			return true;
		}

		protected virtual bool InjectClr()
		{
			GameInstanceBase.Log.Debug("Injecting CLR");
			this.State = GameInstanceState.InjectingCLR;
			this.Verify();
			this.Overlay.Value.Text = string.Format("{0} Injecting CLR", this);
			InjectorResult result = this.InjectorClient.Inject(this.Process.Id, Files.Randomized.Bootstrap.FullName, ModuleType.PE);
			if (result == InjectorResult.SUCCESS)
			{
				GameInstanceBase.Log.Info(string.Format("{0} CLR injection successful", this));
				this.State = GameInstanceState.CLRInjected;
				return true;
			}
			GameInstanceBase.Log.Error(string.Format("{0} CLR injection failed with {1}", this, result));
			this.State = GameInstanceState.CLRInjectionError;
			this.Process.Kill();
			return false;
		}

		protected virtual bool IsInjected()
		{
			GameInstanceBase.Log.Debug("Checking Injection");
			this.Verify();
			InjectorResult result = this.InjectorClient.HasModule(this.Pid, Files.Randomized.Core.FullName, ModuleType.PE);
			if (result == InjectorResult.FAILED)
			{
				return false;
			}
			if (result != InjectorResult.SUCCESS)
			{
				GameInstanceBase.Log.Error(string.Format("HasModule failed with {0}", result));
				return false;
			}
			GameInstanceBase.Log.Warn(string.Format("{0} Injected but GameInstance Lock missing", this));
			this.State = GameInstanceState.AlreadyInjected;
			return true;
		}

		protected virtual async Task<bool> IsLoadedAsync(CancellationToken token = null)
		{
			bool flag;
			CoreMemoryState coreState;
			Stopwatch stopwatch = Stopwatch.StartNew();
			do
			{
				this.Verify();
				coreState = this.CoreState;
				GameInstanceBase.Log.Debug(string.Format("{0} Core State {1}-{2}", this, (int)coreState, coreState));
				switch (coreState)
				{
					case CoreMemoryState.Unknown:
					{
						this.Overlay.Value.Text = string.Format("{0} Core initializing", this);
						break;
					}
					case CoreMemoryState.Success:
					{
						this.Overlay.Value.Text = string.Format("{0} Core Init Success", this);
						flag = true;
						return flag;
					}
					case CoreMemoryState.PreInitFailed:
					{
						this.Overlay.Value.Text = string.Format("{0} Core Pre Init failed", this);
						throw new CoreStateException(coreState);
					}
					case CoreMemoryState.PostInitFailed:
					{
						this.Overlay.Value.Text = string.Format("{0} Core Post Init failed", this);
						throw new CoreStateException(coreState);
					}
					case CoreMemoryState.AuthStarted:
					{
						this.Overlay.Value.Text = string.Format("{0} Core Waiting for Auth", this);
						break;
					}
					case CoreMemoryState.AuthFailed:
					{
						this.Overlay.Value.Text = string.Format("{0} Core Auth failed", this);
						throw new CoreStateException(coreState);
					}
					case CoreMemoryState.PreInitStarted:
					{
						this.Overlay.Value.Text = string.Format("{0} Core Pre Init", this);
						break;
					}
					default:
					{
						throw new InvalidOperationException(string.Format("Received invalid CoreState {0}", (int)coreState));
					}
				}
				await Task.Delay(250, token);
			}
			while (stopwatch.Elapsed.Seconds <= 120);
			this.Process.Kill();
			object obj = (int)coreState;
			TimeSpan elapsed = stopwatch.Elapsed;
			throw new InvalidOperationException(string.Format("Core [{0}] timeout after {1}sec", obj, elapsed.Seconds));
			return flag;
			throw new InvalidOperationException(string.Format("Received invalid CoreState {0}", (int)coreState));
		}

		protected virtual bool IsLocked()
		{
			this.Verify();
			bool flag = File.Exists(Path.Combine(Directories.Cache, string.Format("{0}_lock", this.Pid)));
			if (flag)
			{
				GameInstanceBase.Log.Warn(string.Format("{0} GameInstance Locked", this));
				this.State = GameInstanceState.Locked;
			}
			return flag;
		}

		protected virtual async Task<bool> IsSupportedAsync(CancellationToken token = null)
		{
			GameInstanceBase.<IsSupportedAsync>d__70 variable = new GameInstanceBase.<IsSupportedAsync>d__70();
			variable.<>4__this = this;
			variable.token = token;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<GameInstanceBase.<IsSupportedAsync>d__70>(ref variable);
			return variable.<>t__builder.Task;
		}

		protected virtual void ResolveGamePath()
		{
			this.Verify();
			try
			{
				this.MainModuleFilePath = this.InjectorClient.GetMainModuleFilePath(this.Pid);
				this.Config.Value.ServiceSettings.GameFilePath = this.MainModuleFilePath;
			}
			catch (Exception exception)
			{
				GameInstanceBase.Log.Warn(exception);
			}
		}

		protected virtual async void RunAsync()
		{
			if (this.TokenSource == null)
			{
				try
				{
					try
					{
						Singleton<Eudyptula>.Instance.Stop();
						this.State = GameInstanceState.Created;
						this.TokenSource = new CancellationTokenSource();
						CancellationToken token = this.TokenSource.Token;
						if (!this.IsLocked())
						{
							this.Overlay.Value.Maximum = 5;
							this.Overlay.Value.State = BusyOverlayViewModel.CommonStates.Open;
							PlaySharp.Toolkit.Injector.ProcessMode processMode = this.ProcessMode;
							if (processMode == PlaySharp.Toolkit.Injector.ProcessMode.X86)
							{
								this.InjectorClient = InjectorWin32.Instance;
							}
							else if (processMode == PlaySharp.Toolkit.Injector.ProcessMode.X64)
							{
								this.InjectorClient = new PlaySharp.Toolkit.Injector.InjectorClient(Files.X64.Injector.FullName, Files.X64.InjectorNative.FullName, PlaySharp.Toolkit.Injector.ProcessMode.X64);
							}
							if (!this.IsInjected())
							{
								this.CreateSharedMemory();
								this.ResolveGamePath();
								if (await this.IsSupportedAsync(token))
								{
									this.Inject();
									if (await this.IsLoadedAsync(token))
									{
										this.InjectClr();
										this.Overlay.Value.Maximum = 1;
										this.Overlay.Value.Text = string.Format("{0} Completed", this);
										this.State = GameInstanceState.Completed;
										this.IsCompleted = true;
										await Task.Delay(TimeSpan.FromSeconds(2), token);
										token = new CancellationToken();
									}
									else
									{
										return;
									}
								}
								else
								{
									return;
								}
							}
							else
							{
								return;
							}
						}
						else
						{
							return;
						}
					}
					catch (OperationCanceledException operationCanceledException1)
					{
						OperationCanceledException operationCanceledException = operationCanceledException1;
						GameInstanceBase.Log.Debug(string.Format("{0} Canceled - Reason: {1}", this, operationCanceledException.Message));
					}
					catch (CoreStateException coreStateException1)
					{
						CoreStateException coreStateException = coreStateException1;
						GameInstanceBase.Log.Error(coreStateException);
						EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage(this.ToString(), coreStateException.Message));
					}
					catch (GameInstanceException gameInstanceException1)
					{
						GameInstanceException gameInstanceException = gameInstanceException1;
						GameInstanceBase.Log.Error(gameInstanceException);
						EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage(this.ToString(), gameInstanceException.Message));
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						GameInstanceBase.Log.Error(exception);
						EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage(this.ToString(), exception.Message));
					}
				}
				finally
				{
					this.Overlay.Value.State = BusyOverlayViewModel.CommonStates.Closed;
					this.Dispose();
				}
			}
			else
			{
				GameInstanceBase.Log.Warn(string.Format("{0} is already running", this));
			}
		}

		protected virtual void Verify()
		{
			this.TokenSource.Token.ThrowIfCancellationRequested();
			this.Process.Refresh();
			if (!this.IsAlive)
			{
				GameInstanceBase.Log.Warn(string.Format("{0} Process died.", this));
				throw new OperationCanceledException();
			}
		}
	}
}