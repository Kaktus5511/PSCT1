using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Loader.ViewModels.Model
{
	public interface IShellView : IConductor, IParent, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		string Header
		{
			get;
		}

		void ActivateView<T>()
		where T : IScreen;

		Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = 0);
	}
}