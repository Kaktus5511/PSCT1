using Loader.ViewModels;
using System;

namespace Loader.ViewModels.Model
{
	public interface INotifier
	{
		void Add(NotificationViewModel notification);
	}
}