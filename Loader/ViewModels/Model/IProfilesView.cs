using Caliburn.Micro;
using Loader.ViewModels;
using System;
using System.ComponentModel;

namespace Loader.ViewModels.Model
{
	public interface IProfilesView : IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged, IConductor, IParent
	{
		ProfileViewModel ActiveItem
		{
			get;
		}

		bool ChampionCheck
		{
			get;
			set;
		}

		IObservableCollection<ProfileViewModel> Items
		{
			get;
		}

		bool LibraryCheck
		{
			get;
			set;
		}

		string SearchText
		{
			get;
			set;
		}

		bool UtilityCheck
		{
			get;
			set;
		}
	}
}