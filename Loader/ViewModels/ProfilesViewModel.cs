using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Model.Message;
using Loader.Services.Model;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Service.Messages;
using PlaySharp.Toolkit.AppDomain.Messages;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Messages;
using Polly;
using Polly.Retry;
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
using System.Windows;

namespace Loader.ViewModels
{
	[Export(typeof(IProfilesView))]
	public class ProfilesViewModel : PlaySharpConductorOneActive<ProfileViewModel>, IProfilesView, IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged, IConductor, IParent, IDropTarget, PlaySharp.Toolkit.EventAggregator.IHandle<OnProfilesChanged>, PlaySharp.Toolkit.EventAggregator.IHandle
	{
		private string searchText;

		public bool ChampionCheck
		{
			get
			{
				return base.Config.Value.Settings.ChampionCheck;
			}
			set
			{
				if (value == base.Config.Value.Settings.ChampionCheck)
				{
					return;
				}
				base.Config.Value.Settings.ChampionCheck = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfilesViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfilesViewModel).GetMethod("get_ChampionCheck").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public bool LibraryCheck
		{
			get
			{
				return base.Config.Value.Settings.LibraryCheck;
			}
			set
			{
				if (value == base.Config.Value.Settings.LibraryCheck)
				{
					return;
				}
				base.Config.Value.Settings.LibraryCheck = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfilesViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfilesViewModel).GetMethod("get_LibraryCheck").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public double ProfileCollectionWidth
		{
			get
			{
				return base.Config.Value.Settings.ProfileCollectionWidth;
			}
			set
			{
				if (value.Equals(base.Config.Value.Settings.ProfileCollectionWidth))
				{
					return;
				}
				base.Config.Value.Settings.ProfileCollectionWidth = value;
				base.NotifyOfPropertyChange<double>(System.Linq.Expressions.Expression.Lambda<Func<double>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfilesViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfilesViewModel).GetMethod("get_ProfileCollectionWidth").MethodHandle)), new ParameterExpression[0]));
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
				base.NotifyOfPropertyChange<string>(System.Linq.Expressions.Expression.Lambda<Func<string>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfilesViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfilesViewModel).GetMethod("get_SearchText").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		[Import(typeof(IShellView))]
		public Lazy<IShellView> Shell
		{
			get;
			set;
		}

		public bool UtilityCheck
		{
			get
			{
				return base.Config.Value.Settings.UtilityCheck;
			}
			set
			{
				if (value == base.Config.Value.Settings.UtilityCheck)
				{
					return;
				}
				base.Config.Value.Settings.UtilityCheck = value;
				base.NotifyOfPropertyChange<bool>(System.Linq.Expressions.Expression.Lambda<Func<bool>>(System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Constant(this, typeof(ProfilesViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(ProfilesViewModel).GetMethod("get_UtilityCheck").MethodHandle)), new ParameterExpression[0]));
				this.Refresh();
			}
		}

		public ProfilesViewModel()
		{
		}

		public void AddLocal()
		{
			this.Shell.Value.ActivateView<IInstallView>();
		}

		public void CreateProfile()
		{
			IProfile newProfile = base.Config.Value.ServiceSettings.Profiles.Create<Profile>(null, true);
			this.ActivateItem(new ProfileViewModel(newProfile));
		}

		public void DragOver(IDropInfo dropInfo)
		{
			AssemblyViewModel data = dropInfo.Data as AssemblyViewModel;
			ProfileViewModel sourceProfile = dropInfo.Data as ProfileViewModel;
			ProfileViewModel targetProfile = dropInfo.TargetItem as ProfileViewModel;
			if (data != null && targetProfile != null)
			{
				if (targetProfile.Profile.Assemblies.Any<IProfileAssembly>((IProfileAssembly a) => a.Id == data.Assembly.Id))
				{
					dropInfo.Effects = DragDropEffects.None;
					return;
				}
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton) && dropInfo.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
				{
					dropInfo.Effects = DragDropEffects.Copy;
					return;
				}
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton))
				{
					dropInfo.Effects = DragDropEffects.Move;
					return;
				}
			}
			if (sourceProfile != null && targetProfile != null && dropInfo.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton))
			{
				dropInfo.Effects = DragDropEffects.Move;
				dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
				return;
			}
			if (dropInfo.Data is List<AssemblyViewModel> && targetProfile != null)
			{
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton) && dropInfo.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
				{
					dropInfo.Effects = DragDropEffects.Copy;
					return;
				}
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton))
				{
					dropInfo.Effects = DragDropEffects.Move;
					return;
				}
			}
		}

		public void Drop(IDropInfo dropInfo)
		{
			AssemblyViewModel sourceAssembly = dropInfo.Data as AssemblyViewModel;
			List<AssemblyViewModel> sourceAssemblyCollection = dropInfo.Data as List<AssemblyViewModel>;
			ProfileViewModel sourceProfile = dropInfo.Data as ProfileViewModel;
			ProfileViewModel targetProfile = dropInfo.TargetItem as ProfileViewModel;
			if (sourceAssembly != null && targetProfile != null)
			{
				PlaySharpConductorOneActive<ProfileViewModel>.Log.Debug(string.Format("Drop {0} -> {1}", sourceAssembly.Assembly, targetProfile.DisplayName));
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
				{
					targetProfile.Profile.Add(sourceAssembly.Assembly);
					return;
				}
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.None))
				{
					targetProfile.Profile.Add(sourceAssembly.Assembly);
					base.ActiveItem.Profile.Remove(sourceAssembly.Assembly.Id);
					base.ActiveItem.DeactivateItem(sourceAssembly, true);
					return;
				}
			}
			if (sourceAssemblyCollection != null && targetProfile != null)
			{
				PlaySharpConductorOneActive<ProfileViewModel>.Log.Debug(string.Format("Drop {0} -> {1}", string.Join<AssemblyViewModel>(",", sourceAssemblyCollection), targetProfile.DisplayName));
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
				{
					foreach (AssemblyViewModel assembly in sourceAssemblyCollection)
					{
						targetProfile.Profile.Add(assembly.Assembly);
					}
					return;
				}
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.None))
				{
					foreach (AssemblyViewModel assembly in sourceAssemblyCollection)
					{
						if (!targetProfile.Profile.Add(assembly.Assembly))
						{
							continue;
						}
						base.ActiveItem.Profile.Remove(assembly.Assembly.Id);
						base.ActiveItem.DeactivateItem(assembly, true);
					}
					return;
				}
			}
			if (sourceProfile != null && targetProfile != null)
			{
				PlaySharpConductorOneActive<ProfileViewModel>.Log.Debug(string.Format("Drop {0} -> {1}", sourceProfile.DisplayName, targetProfile.DisplayName));
				if (dropInfo.KeyStates.HasFlag(DragDropKeyStates.None) && sourceProfile != targetProfile)
				{
					int oldIndex = base.Config.Value.ServiceSettings.Profiles.IndexOf(sourceProfile.Profile);
					base.Config.Value.ServiceSettings.Profiles.RemoveAt(oldIndex);
					int newIndex = dropInfo.InsertIndex;
					if (newIndex > oldIndex)
					{
						newIndex--;
					}
					base.Config.Value.ServiceSettings.Profiles.Insert(newIndex, sourceProfile.Profile);
					GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.Drop(dropInfo);
				}
			}
		}

		public void Handle(OnPackageInstalled message)
		{
			this.Refresh();
		}

		public void Handle(OnPackageUninstalled message)
		{
			this.Refresh();
		}

		public void Handle(OnProfilesChanged message)
		{
			this.OnDeactivate(true);
			this.OnActivate();
		}

		public void Handle(OnGameUnitResolved message)
		{
			this.SearchText = message.Name;
		}

		public async void ImportProfile()
		{
			PolicyBuilder policyBuilder = Policy.Handle<Exception>();
			RetryPolicy retryPolicy = policyBuilder.WaitAndRetry(3, (int attempt) => TimeSpan.FromSeconds(Math.Pow(2, (double)attempt)));
			PolicyResult<string> policyResult = retryPolicy.ExecuteAndCapture<string>(() => {
				if (Clipboard.ContainsText())
				{
					return Clipboard.GetText();
				}
				return "";
			});
			if (policyResult.Outcome == OutcomeType.Failure)
			{
				PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage("Clipboard", string.Format("Failed to access Clipboard\n{0}", policyResult.FinalException.Message)));
			}
			else if (policyResult.Result.StartsWith("ps://"))
			{
				await this.UpdateService.Value.HandleUrl(policyResult.Result);
			}
			else
			{
				PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage("Invalid Link", "Profile Link must start with ps://"));
			}
		}

		ProfileViewModel Loader.ViewModels.Model.IProfilesView.get_ActiveItem()
		{
			return base.ActiveItem;
		}

		IObservableCollection<ProfileViewModel> Loader.ViewModels.Model.IProfilesView.get_Items()
		{
			return base.Items;
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			base.Items.Clear();
			base.Items.AddRange(
				from p in base.Config.Value.ServiceSettings.Profiles
				select new ProfileViewModel(p));
			this.ActivateItem(base.Items.FirstOrDefault<ProfileViewModel>());
		}

		public override void Refresh()
		{
			ProfileViewModel activeItem = base.ActiveItem;
			if (activeItem != null)
			{
				activeItem.Refresh();
			}
			else
			{
			}
			base.Refresh();
		}
	}
}