using Caliburn.Micro;
using System.ComponentModel;

namespace Loader.ViewModels.Model
{
	public interface IUpdateView : IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{

	}
}