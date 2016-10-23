using Caliburn.Micro;
using Loader.Helpers;
using Loader.ViewModels.Model;
using PlaySharp.Service.Messages;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace Loader.ViewModels
{
	[Export(typeof(IBusyOverlayView))]
	public class BusyOverlayViewModel : PlaySharpScreen, IBusyOverlayView, IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged, PlaySharp.Toolkit.EventAggregator.IHandle<OnDownloadProgressChanged>, PlaySharp.Toolkit.EventAggregator.IHandle
	{
		private long busyCounter;

		private bool canAbort;

		private double downloadProgressMaximum;

		private double downloadProgressValue;

		private string downloadText;

		private bool isBusy;

		private bool isDownloading;

		private double maximum;

		private TaskbarItemProgressState progressState;

		private string state;

		private string text;

		private double @value;

		public bool CanAbort
		{
			get
			{
				return this.canAbort;
			}
			set
			{
				if (value == this.canAbort)
				{
					return;
				}
				this.canAbort = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_CanAbort").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public double DownloadProgressMaximum
		{
			get
			{
				return this.downloadProgressMaximum;
			}
			set
			{
				if (value.Equals(this.downloadProgressMaximum))
				{
					return;
				}
				this.downloadProgressMaximum = value;
				base.NotifyOfPropertyChange<double>(Expression.Lambda<Func<double>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_DownloadProgressMaximum").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public double DownloadProgressValue
		{
			get
			{
				return this.downloadProgressValue;
			}
			set
			{
				if (value.Equals(this.downloadProgressValue))
				{
					return;
				}
				this.downloadProgressValue = value;
				base.NotifyOfPropertyChange<double>(Expression.Lambda<Func<double>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_DownloadProgressValue").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string DownloadText
		{
			get
			{
				return this.downloadText;
			}
			set
			{
				if (value == this.downloadText)
				{
					return;
				}
				this.downloadText = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_DownloadText").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			set
			{
				if (value == this.isBusy)
				{
					return;
				}
				this.isBusy = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_IsBusy").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool IsDownloading
		{
			get
			{
				if (Caliburn.Micro.Execute.InDesignMode)
				{
					return true;
				}
				return this.isDownloading;
			}
			set
			{
				if (value == this.isDownloading)
				{
					return;
				}
				this.isDownloading = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_IsDownloading").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public double Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (value.Equals(this.maximum))
				{
					return;
				}
				this.Value = 0;
				this.maximum = value;
				base.NotifyOfPropertyChange<double>(Expression.Lambda<Func<double>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_Maximum").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public TaskbarItemProgressState ProgressState
		{
			get
			{
				return JustDecompileGenerated_get_ProgressState();
			}
			set
			{
				JustDecompileGenerated_set_ProgressState(value);
			}
		}

		public TaskbarItemProgressState JustDecompileGenerated_get_ProgressState()
		{
			return this.progressState;
		}

		public void JustDecompileGenerated_set_ProgressState(TaskbarItemProgressState value)
		{
			if (value == this.progressState)
			{
				return;
			}
			this.progressState = value;
			base.NotifyOfPropertyChange<TaskbarItemProgressState>(Expression.Lambda<Func<TaskbarItemProgressState>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_ProgressState").MethodHandle)), new ParameterExpression[0]));
		}

		public string State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (value == this.state)
				{
					return;
				}
				this.state = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_State").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (value == this.text)
				{
					return;
				}
				if (this.Value + 1 <= this.Maximum)
				{
					this.Value = this.Value + 1;
				}
				this.text = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_Text").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();

		public double Value
		{
			get
			{
				return this.@value;
			}
			set
			{
				if (value.Equals(this.@value))
				{
					return;
				}
				this.@value = value;
				base.NotifyOfPropertyChange<double>(Expression.Lambda<Func<double>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_Value").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<double>(Expression.Lambda<Func<double>>(Expression.Property(Expression.Constant(this, typeof(BusyOverlayViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(BusyOverlayViewModel).GetMethod("get_ValuePercent").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public double ValuePercent
		{
			get
			{
				if (this.Maximum == 0)
				{
					return 0;
				}
				return this.Value / this.Maximum;
			}
		}

		public BusyOverlayViewModel()
		{
		}

		public void Abort()
		{
			CancellationTokenSource tokenSource = this.TokenSource;
			if (tokenSource != null)
			{
				tokenSource.Cancel();
			}
			else
			{
			}
			this.TokenSource = new CancellationTokenSource();
		}

		public void Cleanup()
		{
			this.CanAbort = false;
			this.IsBusy = false;
			this.IsDownloading = false;
			this.Text = string.Empty;
			this.Value = 0;
			this.Maximum = 1;
			this.DownloadText = string.Empty;
			this.DownloadProgressValue = 0;
			this.DownloadProgressMaximum = 1;
			this.ProgressState = TaskbarItemProgressState.None;
		}

		public void Handle(OnDownloadProgressChanged message)
		{
			if (message.Total <= (long)0)
			{
				this.DownloadText = string.Format("{0} - {1}", message.Name, Extensions.BytesToString(message.Received));
			}
			else
			{
				this.DownloadText = string.Format("{0} - {1} / {2}", message.Name, Extensions.BytesToString(message.Received), Extensions.BytesToString(message.Total));
			}
			this.DownloadProgressMaximum = (double)message.Total;
			this.DownloadProgressValue = (double)message.Received;
			this.IsDownloading = true;
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.State = BusyOverlayViewModel.CommonStates.Closed;
		}

		public void Run(Action<BusyOverlayViewModel> resultBody)
		{
			try
			{
				this.IsBusy = true;
				resultBody(this);
			}
			finally
			{
				this.IsBusy = false;
			}
		}

		public async Task RunAsync(Func<BusyOverlayViewModel, CancellationToken, Task> resultBody)
		{
			BusyOverlayViewModel.<RunAsync>d__56 variable = new BusyOverlayViewModel.<RunAsync>d__56();
			variable.<>4__this = this;
			variable.resultBody = resultBody;
			variable.<>t__builder = AsyncTaskMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<BusyOverlayViewModel.<RunAsync>d__56>(ref variable);
			return variable.<>t__builder.Task;
		}

		public static class CommonStates
		{
			public static string Closed
			{
				get
				{
					return "Closed";
				}
			}

			public static string Open
			{
				get
				{
					return "Open";
				}
			}
		}
	}
}