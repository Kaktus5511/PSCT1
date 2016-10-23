using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Model.Message;
using Loader.Properties;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Service.Package.Model;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Loader.ViewModels
{
	public class ProfileViewModel : PlaySharpConductorOneActive<AssemblyViewModel>, IProfileView, IConductActiveItem, IConductor, IParent, INotifyPropertyChangedEx, INotifyPropertyChanged, IHaveActiveItem, IDropTarget, IEquatable<ProfileViewModel>, PlaySharp.Toolkit.EventAggregator.IHandle<OnAssemblyChanged>, PlaySharp.Toolkit.EventAggregator.IHandle, PlaySharp.Toolkit.EventAggregator.IHandle<OnProfileChanged>
	{
		private bool isEditing;

		private BindableCollection<AssemblyViewModel> selectedItems = new BindableCollection<AssemblyViewModel>();

		public bool CanDelete
		{
			get
			{
				return base.ActiveItem != null;
			}
		}

		public bool CanOpenForum
		{
			get
			{
				object assembly;
				AssemblyViewModel activeItem = base.ActiveItem;
				if (activeItem != null)
				{
					assembly = activeItem.Assembly;
				}
				else
				{
					assembly = null;
				}
				PackageAssembly packageAssembly = assembly as PackageAssembly;
				if (packageAssembly == null)
				{
					return false;
				}
				return packageAssembly.AssemblyEntry.TopicId > 0;
			}
		}

		public bool CanOpenGithub
		{
			get
			{
				string url;
				AssemblyViewModel activeItem = base.ActiveItem;
				if (activeItem != null)
				{
					IPlaySharpAssembly assembly = activeItem.Assembly;
					if (assembly != null)
					{
						url = assembly.Url;
					}
					else
					{
						url = null;
					}
				}
				else
				{
					url = null;
				}
				return !string.IsNullOrEmpty(url);
			}
		}

		public bool CanShareAssembly
		{
			get
			{
				return base.ActiveItem != null;
			}
		}

		public bool CanUpdateAndCompile
		{
			get
			{
				return base.ActiveItem != null;
			}
		}

		public override string DisplayName
		{
			get
			{
				return this.Profile.Name;
			}
			set
			{
				if (object.Equals(value, this.Profile.Name))
				{
					return;
				}
				this.Profile.Name = value;
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Screen).GetMethod("get_DisplayName").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool Inject
		{
			get
			{
				if (this.DisplayName == AssemblyType.Library.ToString())
				{
					return true;
				}
				return this.Profile.Inject;
			}
			set
			{
				if (value == this.Profile.Inject)
				{
					return;
				}
				this.Profile.Inject = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_Inject").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool IsEditing
		{
			get
			{
				return this.isEditing;
			}
			set
			{
				if (value == this.isEditing)
				{
					return;
				}
				this.isEditing = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_IsEditing").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[Import(typeof(IBusyOverlayView))]
		public Lazy<IBusyOverlayView> Overlay
		{
			get;
			set;
		}

		public IProfile Profile
		{
			get;
		}

		[Import(typeof(IProfilesView))]
		public Lazy<IProfilesView> ProfilesView
		{
			get;
			set;
		}

		public BindableCollection<AssemblyViewModel> SelectedItems
		{
			get
			{
				return this.selectedItems;
			}
			set
			{
				if (object.Equals(value, this.selectedItems))
				{
					return;
				}
				this.selectedItems = value;
				base.NotifyOfPropertyChange<BindableCollection<AssemblyViewModel>>(System.Linq.Expressions.Expression.Lambda<Func<BindableCollection<AssemblyViewModel>>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_SelectedItems").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[Import(typeof(IShellView))]
		public Lazy<IShellView> Shell
		{
			get;
			set;
		}

		public bool ShowDescription
		{
			get
			{
				return base.Config.Value.Settings.ShowDescription;
			}
		}

		public bool ShowNotes
		{
			get
			{
				return base.Config.Value.Settings.ShowNotes;
			}
		}

		public ProfileViewModel(IProfile profile)
		{
			this.Profile = profile;
		}

		public ProfileViewModel()
		{
		}

		public override void ActivateItem(AssemblyViewModel item)
		{
			base.ActivateItem(item);
			if (item != null && !this.SelectedItems.Contains(item))
			{
				this.SelectedItems.Add(item);
			}
			base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_CanUpdateAndCompile").MethodHandle)), new ParameterExpression[0]));
			base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_CanOpenGithub").MethodHandle)), new ParameterExpression[0]));
			base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_CanOpenForum").MethodHandle)), new ParameterExpression[0]));
			base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_CanShareAssembly").MethodHandle)), new ParameterExpression[0]));
			base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_CanDelete").MethodHandle)), new ParameterExpression[0]));
		}

		public void AddLocal()
		{
			this.Shell.Value.ActivateView<IInstallView>();
		}

		public async Task<bool> CompileAsync()
		{
			ProfileViewModel.<CompileAsync>d__49 variable = new ProfileViewModel.<CompileAsync>d__49();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<ProfileViewModel.<CompileAsync>d__49>(ref variable);
			return variable.<>t__builder.Task;
		}

		public void Delete()
		{
			if (base.ActiveItem == null)
			{
				return;
			}
			if (this.SelectedItems.Count == 0)
			{
				return;
			}
			AssemblyViewModel[] array = this.SelectedItems.ToArray<AssemblyViewModel>();
			for (int i = 0; i < (int)array.Length; i++)
			{
				AssemblyViewModel item = array[i];
				this.Profile.Remove(item.Assembly.Id);
				this.DeactivateItem(item, true);
			}
		}

		public void DeleteProfile()
		{
			if (this.Profile is LibrariesProfile)
			{
				return;
			}
			foreach (IProfileAssembly assembly in this.Profile.Assemblies.ToList<IProfileAssembly>())
			{
				this.Profile.Remove(assembly.Id);
			}
			base.Config.Value.ServiceSettings.Profiles.Remove(this.Profile);
			((IConductor)this.Parent).DeactivateItem(this, true);
		}

		public void DragOver(IDropInfo dropInfo)
		{
			if (dropInfo.Data is AssemblyViewModel)
			{
				dropInfo.Effects = DragDropEffects.Move;
				dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
			}
		}

		public void Drop(IDropInfo dropInfo)
		{
			AssemblyViewModel sourceAssembly = dropInfo.Data as AssemblyViewModel;
			if (sourceAssembly != null)
			{
				int oldIndex = this.Profile.Assemblies.IndexOf(sourceAssembly.ProfileAssembly);
				this.Profile.Assemblies.RemoveAt(oldIndex);
				int newIndex = dropInfo.InsertIndex;
				if (newIndex > oldIndex)
				{
					newIndex--;
				}
				this.Profile.Assemblies.Insert(newIndex, sourceAssembly.ProfileAssembly);
				GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.Drop(dropInfo);
			}
		}

		public bool Equals(ProfileViewModel other)
		{
			if (other == null)
			{
				return false;
			}
			if ((object)this == (object)other)
			{
				return true;
			}
			return object.Equals(this.Profile, other.Profile);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			return this.Equals((ProfileViewModel)obj);
		}

		private bool Filter(object item)
		{
			bool flag;
			string str;
			IPlaySharpAssembly playSharpAssembly;
			bool count;
			try
			{
				if (this.ProfilesView.Value != null)
				{
					string searchText = this.ProfilesView.Value.SearchText;
					if (searchText != null)
					{
						str = searchText.Replace("*", "(.*)");
					}
					else
					{
						str = null;
					}
					string str1 = str;
					AssemblyViewModel assemblyViewModel = item as AssemblyViewModel;
					if (assemblyViewModel != null)
					{
						playSharpAssembly = assemblyViewModel.Assembly;
					}
					else
					{
						playSharpAssembly = null;
					}
					IPlaySharpAssembly assembly = playSharpAssembly;
					if (assembly != null)
					{
						switch (assembly.Type)
						{
							case AssemblyType.Champion:
							{
								if (this.ProfilesView.Value.ChampionCheck)
								{
									break;
								}
								flag = false;
								return flag;
							}
							case AssemblyType.Utility:
							{
								if (this.ProfilesView.Value.UtilityCheck)
								{
									break;
								}
								flag = false;
								return flag;
							}
							case AssemblyType.Library:
							{
								if (this.ProfilesView.Value.LibraryCheck)
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
						else if (StringExtensions.Contains(assembly.Name, str1, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
						}
						else if (!StringExtensions.Contains(assembly.DisplayName, str1, StringComparison.OrdinalIgnoreCase))
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
							else if (StringExtensions.Contains(assembly.Author, str1, StringComparison.OrdinalIgnoreCase))
							{
								flag = true;
							}
							else if (assembly.Id.ToString() != str1)
							{
								flag = (assembly.Description.IsNullOrEmpty() || !StringExtensions.Contains(assembly.Description, str1, StringComparison.OrdinalIgnoreCase) ? false : true);
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
				else
				{
					flag = true;
				}
			}
			catch (Exception exception)
			{
				PlaySharpConductorOneActive<AssemblyViewModel>.Log.Warn(exception);
				flag = true;
			}
			return flag;
		}

		public override int GetHashCode()
		{
			IProfile profile = this.Profile;
			if (profile != null)
			{
				return profile.GetHashCode();
			}
			return 0;
		}

		public void Handle(OnAssemblyChanged message)
		{
			this.Refresh();
		}

		public void Handle(OnProfileChanged message)
		{
			this.OnDeactivate(false);
			this.OnActivate();
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			base.Items.Clear();
			base.Items.AddRange(
				from a in this.Profile.Assemblies
				select new AssemblyViewModel(a));
			CollectionViewSource.GetDefaultView(base.Items).Filter = new Predicate<object>(this.Filter);
			this.ActivateItem(base.Items.FirstOrDefault<AssemblyViewModel>());
			base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfileViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfileViewModel).GetMethod("get_ShowNotes").MethodHandle)), new ParameterExpression[0]));
		}

		public void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			foreach (AssemblyViewModel added in e.AddedItems.Cast<AssemblyViewModel>())
			{
				if (this.SelectedItems.Contains(added))
				{
					continue;
				}
				this.SelectedItems.Add(added);
			}
			foreach (AssemblyViewModel removed in e.RemovedItems.Cast<AssemblyViewModel>())
			{
				this.SelectedItems.Remove(removed);
			}
			e.Handled = true;
		}

		public static bool operator ==(ProfileViewModel left, ProfileViewModel right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(ProfileViewModel left, ProfileViewModel right)
		{
			return !object.Equals(left, right);
		}

		public void OpenForum()
		{
			object obj;
			AssemblyViewModel activeItem = base.ActiveItem;
			if (activeItem != null)
			{
				obj = activeItem.Assembly;
			}
			else
			{
				obj = null;
			}
			PackageAssembly assembly = obj as PackageAssembly;
			if (assembly != null && assembly.AssemblyEntry.TopicId > 0)
			{
				Process.Start(string.Format("https://www.joduska.me/forum/topic/{0}-", assembly.AssemblyEntry.TopicId));
			}
		}

		public void OpenGithub()
		{
			string url;
			AssemblyViewModel activeItem = base.ActiveItem;
			if (activeItem != null)
			{
				url = activeItem.Assembly.Url;
			}
			else
			{
				url = null;
			}
			if (!string.IsNullOrEmpty(url))
			{
				Process.Start(base.ActiveItem.Assembly.Url);
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
				PlaySharpConductorOneActive<AssemblyViewModel>.Log.Warn(exception.Message);
			}
		}

		public async void ShareAssembly()
		{
			bool assembly;
			AssemblyViewModel activeItem = this.ActiveItem;
			if (activeItem != null)
			{
				assembly = activeItem.Assembly;
			}
			else
			{
				assembly = false;
			}
			if (assembly)
			{
				string str = string.Format("ps://assembly/{0}", this.ActiveItem.Assembly.Id);
				for (int i = 0; i < 30; i++)
				{
					try
					{
						Clipboard.SetText(str);
						PlaySharpConductorOneActive<AssemblyViewModel>.Log.Debug(string.Format("Created Assembly link {0}", str));
						break;
					}
					catch
					{
						Thread.Sleep(25);
					}
				}
				PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage(Translation.Message_AssemblyExport_Title, Translation.Message_AssemblyExport_Body));
			}
		}

		public async void ShareProfile()
		{
			string str = HttpUtility.UrlEncode(this.DisplayName);
			IList<IProfileAssembly> assemblies = this.Profile.Assemblies;
			string str1 = string.Format("ps://profile/{0}/{1}", str, string.Join<int>(",", 
				from a in assemblies
				select a.Id));
			for (int i = 0; i < 30; i++)
			{
				try
				{
					Clipboard.SetText(str1);
					PlaySharpConductorOneActive<AssemblyViewModel>.Log.Debug(string.Format("Created Profile link {0}", str1));
					break;
				}
				catch
				{
					Thread.Sleep(25);
				}
			}
			PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage(Translation.Message_ProfileExport_Title, Translation.Message_ProfileExport_Body));
		}

		public void StartRenameProfile()
		{
			this.IsEditing = true;
		}

		public void StopRenameProfile()
		{
			this.IsEditing = false;
		}

		public async void UpdateAndCompile()
		{
			if (this.ActiveItem != null)
			{
				await this.Overlay.Value.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
					model.Maximum = 3;
					model.Text = string.Format("Update {0}", this.ActiveItem.Assembly.Name);
					await this.ActiveItem.UpdateAsync();
					model.Text = string.Format("Update {0}", this.ActiveItem.Assembly.Name);
					await this.ActiveItem.CompileAsync();
					BusyOverlayViewModel value = model;
					value.Value = value.Value + 1;
				});
			}
		}

		public async Task<bool> UpdateAsync()
		{
			ProfileViewModel.<UpdateAsync>d__66 variable = new ProfileViewModel.<UpdateAsync>d__66();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<ProfileViewModel.<UpdateAsync>d__66>(ref variable);
			return variable.<>t__builder.Task;
		}
	}
}