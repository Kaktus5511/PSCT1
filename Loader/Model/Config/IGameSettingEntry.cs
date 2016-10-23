using System;
using System.Collections.ObjectModel;

namespace Loader.Model.Config
{
	public interface IGameSettingEntry
	{
		string DisplayName
		{
			get;
		}

		string Name
		{
			get;
			set;
		}

		ObservableCollection<string> PosibleValues
		{
			get;
			set;
		}

		string SelectedValue
		{
			get;
			set;
		}
	}
}