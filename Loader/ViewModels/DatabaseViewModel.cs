using Caliburn.Micro;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Model.Message;
using Loader.Properties;
using Loader.Services.Model;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Loader.ViewModels
{
	[Export(typeof(IDatebaseView))]
	public class DatabaseViewModel : PlaySharpConductorOneActive<AssemblyEntryViewModel>, IDatebaseView, IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged, PlaySharp.Toolkit.EventAggregator.IHandle<AssemblyData>, PlaySharp.Toolkit.EventAggregator.IHandle
	{
		private bool canInstall;

		private bool championCheck = true;

		private bool libraryCheck;

		private BindableCollection<IProfile> profiles = new BindableCollection<IProfile>();

		private string searchText = string.Empty;

		private IProfile selectedProfile;

		private bool utilityCheck = true;

		public bool CanInstall
		{
			get
			{
				return this.canInstall;
			}
			set
			{
				if (value == this.canInstall)
				{
					return;
				}
				this.canInstall = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(DatabaseViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DatabaseViewModel).GetMethod("get_CanInstall").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool CanOpenForum
		{
			get
			{
				int? nullable;
				AssemblyEntryViewModel activeItem = base.ActiveItem;
				if (activeItem == null)
				{
					return false;
				}
				AssemblyEntry assembly = activeItem.Assembly;
				if (assembly != null)
				{
					nullable = new int?(assembly.TopicId);
				}
				else
				{
					nullable = null;
				}
				int? nullable1 = nullable;
				if (nullable1.GetValueOrDefault() <= 0)
				{
					return false;
				}
				return nullable1.HasValue;
			}
		}

		public bool ChampionCheck
		{
			get
			{
				return this.championCheck;
			}
			set
			{
				if (value == this.championCheck)
				{
					return;
				}
				this.championCheck = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(DatabaseViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DatabaseViewModel).GetMethod("get_ChampionCheck").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public bool LibraryCheck
		{
			get
			{
				return this.libraryCheck;
			}
			set
			{
				if (value == this.libraryCheck)
				{
					return;
				}
				this.libraryCheck = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(DatabaseViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DatabaseViewModel).GetMethod("get_LibraryCheck").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public BindableCollection<IProfile> Profiles
		{
			get
			{
				return this.profiles;
			}
			set
			{
				if (object.Equals(value, this.profiles))
				{
					return;
				}
				this.profiles = value;
				base.NotifyOfPropertyChange<BindableCollection<IProfile>>(Expression.Lambda<Func<BindableCollection<IProfile>>>(Expression.Property(Expression.Constant(this, typeof(DatabaseViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DatabaseViewModel).GetMethod("get_Profiles").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public string SearchText
		{
			get
			{
				return this.searchText;
			}
			set
			{
				if (value == this.searchText)
				{
					return;
				}
				this.searchText = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(DatabaseViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DatabaseViewModel).GetMethod("get_SearchText").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public IProfile SelectedProfile
		{
			get
			{
				return this.selectedProfile;
			}
			set
			{
				if (object.Equals(value, this.selectedProfile))
				{
					return;
				}
				this.selectedProfile = value;
				base.NotifyOfPropertyChange<IProfile>(Expression.Lambda<Func<IProfile>>(Expression.Property(Expression.Constant(this, typeof(DatabaseViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DatabaseViewModel).GetMethod("get_SelectedProfile").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public bool UtilityCheck
		{
			get
			{
				return this.utilityCheck;
			}
			set
			{
				if (value == this.utilityCheck)
				{
					return;
				}
				this.utilityCheck = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(DatabaseViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DatabaseViewModel).GetMethod("get_UtilityCheck").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public DatabaseViewModel()
		{
		}

		public override void ActivateItem(AssemblyEntryViewModel item)
		{
			base.ActivateItem(item);
			base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(DatabaseViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(DatabaseViewModel).GetMethod("get_CanOpenForum").MethodHandle)), new ParameterExpression[0]));
		}

		public void CheckAll()
		{
			foreach (AssemblyEntryViewModel assemblyEntryViewModel in CollectionViewSource.GetDefaultView(base.Items).Cast<AssemblyEntryViewModel>())
			{
				assemblyEntryViewModel.InstallChecked = true;
			}
		}

		private bool Filter(object item)
		{
			bool flag;
			string str;
			AssemblyEntry assemblyEntry;
			bool count;
			try
			{
				string searchText = this.SearchText;
				if (searchText != null)
				{
					str = searchText.Replace("*", "(.*)");
				}
				else
				{
					str = null;
				}
				string str1 = str;
				AssemblyEntryViewModel assemblyEntryViewModel = item as AssemblyEntryViewModel;
				if (assemblyEntryViewModel != null)
				{
					assemblyEntry = assemblyEntryViewModel.Assembly;
				}
				else
				{
					assemblyEntry = null;
				}
				AssemblyEntry assembly = assemblyEntry;
				if (assembly != null)
				{
					switch (assembly.Type)
					{
						case AssemblyType.Champion:
						{
							if (this.ChampionCheck)
							{
								break;
							}
							flag = false;
							return flag;
						}
						case AssemblyType.Utility:
						{
							if (this.UtilityCheck)
							{
								break;
							}
							flag = false;
							return flag;
						}
						case AssemblyType.Library:
						{
							if (this.LibraryCheck)
							{
								break;
							}
							flag = false;
							return flag;
						}
					}
					if (string.IsNullOrEmpty(str1))
					{
						flag = true;
					}
					else if (StringExtensions.Contains(str1, "trash", StringComparison.OrdinalIgnoreCase) && assembly.AuthorId == 71627)
					{
						flag = true;
					}
					else if (!StringExtensions.Contains(assembly.Name, str1, StringComparison.OrdinalIgnoreCase))
					{
						IReadOnlyList<string> champions = assembly.Champions;
						if (champions != null)
						{
							count = champions.Count > 0;
						}
						else
						{
							count = false;
						}
						if (count && assembly.Champions.Any<string>((string c) => StringExtensions.Contains(c, str1, StringComparison.OrdinalIgnoreCase)))
						{
							flag = true;
						}
						else if (StringExtensions.Contains(assembly.AuthorName, str1, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
						}
						else if (assembly.Id.ToString() != str1)
						{
							flag = (!StringExtensions.Contains(assembly.Description, str1, StringComparison.OrdinalIgnoreCase) ? false : true);
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag = false;
				}
			}
			catch (Exception exception)
			{
				PlaySharpConductorOneActive<AssemblyEntryViewModel>.Log.Warn(exception);
				flag = true;
			}
			return flag;
		}

		public void Handle(AssemblyData message)
		{
			this.UpdateItems(message);
		}

		public async void Install()
		{
			DatabaseViewModel.<Install>d__33 variable = new DatabaseViewModel.<Install>d__33();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<DatabaseViewModel.<Install>d__33>(ref variable);
		}

		public async void InstallSelected()
		{
			DatabaseViewModel.<InstallSelected>d__34 variable = new DatabaseViewModel.<InstallSelected>d__34();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<DatabaseViewModel.<InstallSelected>d__34>(ref variable);
		}

		protected override async void OnActivate()
		{
			DatabaseViewModel.<OnActivate>d__39 variable = new DatabaseViewModel.<OnActivate>d__39();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncVoidMethodBuilder.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<DatabaseViewModel.<OnActivate>d__39>(ref variable);
		}

		public void OpenForum()
		{
			bool topicId;
			AssemblyEntryViewModel activeItem = base.ActiveItem;
			if (activeItem != null)
			{
				topicId = activeItem.Assembly.TopicId > 0;
			}
			else
			{
				topicId = false;
			}
			if (topicId)
			{
				Process.Start(string.Format("https://www.joduska.me/forum/topic/{0}-", base.ActiveItem.Assembly.TopicId));
			}
		}

		public void OpenGithub()
		{
			if (base.ActiveItem != null)
			{
				Process.Start(base.ActiveItem.Assembly.RepositoryUrl);
			}
		}

		public override void Refresh()
		{
			try
			{
				CollectionViewSource.GetDefaultView(base.Items).Refresh();
				base.Refresh();
			}
			catch (Exception exception)
			{
				PlaySharpConductorOneActive<AssemblyEntryViewModel>.Log.Warn(exception.Message);
			}
		}

		public void UncheckAll()
		{
			foreach (AssemblyEntryViewModel assemblyEntryViewModel in CollectionViewSource.GetDefaultView(base.Items).Cast<AssemblyEntryViewModel>())
			{
				assemblyEntryViewModel.InstallChecked = false;
			}
		}

		private void UpdateItems(AssemblyData data)
		{
			try
			{
				try
				{
					this.CanInstall = false;
					base.Items.Clear();
					base.Items.AddRange(data.Assemblies.Where<AssemblyEntry>((AssemblyEntry a) => {
						if (!a.IsValid)
						{
							return false;
						}
						IReadOnlyList<VersionEntry> versions = a.Versions;
						if (versions == null)
						{
							return false;
						}
						return versions.Count > 0;
					}).Select<AssemblyEntry, AssemblyEntryViewModel>((AssemblyEntry a) => new AssemblyEntryViewModel(a)));
					this.ActivateItem(base.Items.FirstOrDefault<AssemblyEntryViewModel>());
					CollectionViewSource.GetDefaultView(base.Items).Filter = new Predicate<object>(this.Filter);
				}
				catch (Exception exception)
				{
					PlaySharpConductorOneActive<AssemblyEntryViewModel>.Log.Error(exception);
				}
			}
			finally
			{
				this.CanInstall = true;
			}
		}
	}
}