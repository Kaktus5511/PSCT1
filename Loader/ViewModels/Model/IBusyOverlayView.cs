using Caliburn.Micro;
using Loader.ViewModels;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace Loader.ViewModels.Model
{
	public interface IBusyOverlayView : IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		bool IsBusy
		{
			get;
			set;
		}

		double Maximum
		{
			get;
			set;
		}

		TaskbarItemProgressState ProgressState
		{
			get;
		}

		string State
		{
			get;
			set;
		}

		string Text
		{
			get;
			set;
		}

		double Value
		{
			get;
			set;
		}

		double ValuePercent
		{
			get;
		}

		void Run(Action<BusyOverlayViewModel> resultBody);

		Task RunAsync(Func<BusyOverlayViewModel, CancellationToken, Task> resultBody);
	}
}