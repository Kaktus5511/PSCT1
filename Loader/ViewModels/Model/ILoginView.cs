using Caliburn.Micro;
using System;
using System.ComponentModel;

namespace Loader.ViewModels.Model
{
	public interface ILoginView : IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		string AuthKey
		{
			get;
			set;
		}
	}
}