using Caliburn.Micro;
using Loader.Model.Config;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Loader.ViewModels.Model
{
	public interface IProfileView : IConductActiveItem, IConductor, IParent, INotifyPropertyChangedEx, INotifyPropertyChanged, IHaveActiveItem
	{
		bool Inject
		{
			get;
			set;
		}

		IProfile Profile
		{
			get;
		}

		Task<bool> CompileAsync();

		Task<bool> UpdateAsync();
	}
}