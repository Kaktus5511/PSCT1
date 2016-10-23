using Caliburn.Micro;
using System;
using System.ComponentModel;

namespace Loader.ViewModels.Model
{
	public interface IDatebaseView : IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		string SearchText
		{
			get;
			set;
		}
	}
}