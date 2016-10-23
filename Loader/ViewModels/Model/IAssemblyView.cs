using Caliburn.Micro;
using Loader.Model.Config;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Loader.ViewModels.Model
{
	public interface IAssemblyView : IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		IPlaySharpAssembly Assembly
		{
			get;
		}

		Task<bool> CompileAsync();

		Task<bool> UpdateAsync();
	}
}