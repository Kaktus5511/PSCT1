using Caliburn.Micro;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Loader.ViewModels.Model
{
	public interface IDashboardView : IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		Task UpdateAccountInfoAsync();

		Task UpdateCoreInfoAsync();

		Task UpdateGameInfoAsync();
	}
}