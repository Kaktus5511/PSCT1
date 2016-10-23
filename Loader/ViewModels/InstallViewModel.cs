using Caliburn.Micro;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Properties;
using Loader.Services;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Extensions.Model;
using PlaySharp.Toolkit.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loader.ViewModels
{
	[Export(typeof(IInstallView))]
	public class InstallViewModel : PlaySharpScreen, IInstallView, IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged
	{
		private string author;

		private string description;

		private string name;

		private BindableCollection<IProfile> profiles = new BindableCollection<IProfile>();

		private BindableCollection<AssemblyProject> projects = new BindableCollection<AssemblyProject>();

		private IProfile selectedProfile;

		private AssemblyProject selectedProject;

		private string source;

		private AssemblyType type;

		private BindableCollection<AssemblyType> types = new BindableCollection<AssemblyType>();

		public string Author
		{
			get
			{
				return this.author;
			}
			set
			{
				if (value == this.author)
				{
					return;
				}
				this.author = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_Author").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_CanAdd").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public bool CanAdd
		{
			get
			{
				if (this.SelectedProject == null)
				{
					return false;
				}
				if (this.SelectedProfile == null)
				{
					return false;
				}
				if (string.IsNullOrEmpty(this.Name))
				{
					return false;
				}
				if (string.IsNullOrEmpty(this.Author))
				{
					return false;
				}
				if (this.Type == AssemblyType.Unknown)
				{
					return false;
				}
				return true;
			}
		}

		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				if (value == this.description)
				{
					return;
				}
				this.description = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_Description").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == this.name)
				{
					return;
				}
				this.name = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_Name").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_CanAdd").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[Import(typeof(IBusyOverlayView))]
		public IBusyOverlayView Overlay
		{
			get;
			set;
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
				base.NotifyOfPropertyChange<BindableCollection<IProfile>>(Expression.Lambda<Func<BindableCollection<IProfile>>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_Profiles").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public BindableCollection<AssemblyProject> Projects
		{
			get
			{
				return this.projects;
			}
			set
			{
				if (object.Equals(value, this.projects))
				{
					return;
				}
				this.projects = value;
				base.NotifyOfPropertyChange<BindableCollection<AssemblyProject>>(Expression.Lambda<Func<BindableCollection<AssemblyProject>>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_Projects").MethodHandle)), new ParameterExpression[0]));
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
				base.NotifyOfPropertyChange<IProfile>(Expression.Lambda<Func<IProfile>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_SelectedProfile").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public AssemblyProject SelectedProject
		{
			get
			{
				return this.selectedProject;
			}
			set
			{
				string name;
				if (object.Equals(value, this.selectedProject))
				{
					return;
				}
				if (value != null)
				{
					name = value.Name;
				}
				else
				{
					name = null;
				}
				this.Name = name;
				Match repositoryMatch = Regex.Match(this.Source, "^(http[s]?)://(?<host>.*?)/(?<author>.*?)/(?<repo>.*?)(/{1}|$)");
				if (!repositoryMatch.Success)
				{
					this.Author = "Local";
				}
				else
				{
					this.Author = repositoryMatch.Groups["author"].Value;
				}
				this.selectedProject = value;
				base.NotifyOfPropertyChange<AssemblyProject>(Expression.Lambda<Func<AssemblyProject>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_SelectedProject").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_CanAdd").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Source
		{
			get
			{
				return this.source;
			}
			set
			{
				if (value == this.source)
				{
					return;
				}
				this.source = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_Source").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public AssemblyType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value == this.type)
				{
					return;
				}
				this.type = value;
				base.NotifyOfPropertyChange<AssemblyType>(Expression.Lambda<Func<AssemblyType>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_Type").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(InstallViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(InstallViewModel).GetMethod("get_CanAdd").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public InstallViewModel()
		{
		}

		public async void Add()
		{
			if (!(this.SelectedProfile is LibrariesProfile) || this.Type == AssemblyType.Library)
			{
				LocalAssembly localAssembly = new LocalAssembly()
				{
					Name = this.Name,
					DisplayName = this.Name,
					Author = this.Author,
					Type = this.Type,
					Description = this.Description,
					PathToProjectFile = this.SelectedProject.FilePath,
					Service = this.Config.Value.Service,
					Status = AssemblyStatus.Ready,
					Champions = new string[0]
				};
				LocalAssembly localAssembly1 = localAssembly;
				Match match = Regex.Match(this.Source, "^(http[s]?)://(?<host>.*?)/(?<author>.*?)/(?<repo>.*?)(/{1}|$)");
				if (match.Success)
				{
					localAssembly1.Url = string.Format("https://{0}/{1}/{2}", match.Groups["host"].Value, match.Groups["author"].Value, match.Groups["repo"].Value);
				}
				await this.Overlay.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
					model.Maximum = 3;
					model.Text = string.Format("Update {0}", this.Name);
					await localAssembly1.UpdateAsync();
					model.Text = string.Format("Compile {0}", this.Name);
					await localAssembly1.CompileAsync();
					model.Value = 3;
				});
				this.SelectedProfile.Add(localAssembly1);
				this.Name = string.Empty;
				this.Author = string.Empty;
				this.Description = string.Empty;
			}
			else
			{
				PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(this.EventAggregator.Value, new OnShowMessage(Translation.Message_Install_InvalidProfileType_Title, string.Format(Translation.Message_Install_InvalidProfileType_Body, this.Type, this.SelectedProfile.Name)));
			}
		}

		public void BrowseLocal()
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog()
			{
				RootFolder = Environment.SpecialFolder.Desktop
			};
			if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
			{
				this.Source = dialog.SelectedPath;
				this.UpdateProjects();
			}
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			this.Profiles = new BindableCollection<IProfile>(base.Config.Value.ServiceSettings.Profiles);
			this.SelectedProfile = this.Profiles.FirstOrDefault<IProfile>();
		}

		public async void UpdateProjects()
		{
			if (!string.IsNullOrEmpty(this.Source))
			{
				string source = this.Source;
				Match match = Regex.Match(this.Source, "^(http[s]?)://(?<host>.*?)/(?<author>.*?)/(?<repo>.*?)(/{1}|$)");
				if (match.Success)
				{
					source = string.Format("https://{0}/{1}/{2}", match.Groups["host"].Value, match.Groups["author"].Value, match.Groups["repo"].Value);
				}
				if (source.StartsWith("https"))
				{
					await this.Overlay.RunAsync(async (BusyOverlayViewModel model, CancellationToken token) => {
						RepositoryInfo repositoryInfo = source.ToRepositoryInfo();
						string str = Path.Combine(Directories.Repositories, repositoryInfo.Host, repositoryInfo.Author, repositoryInfo.Repository);
						model.Maximum = 2;
						model.Text = string.Format("Downloading {0}", source);
						await GitUpdater.UpdateAsync(source, null);
						source = str;
						model.Value = 2;
					});
				}
				try
				{
					this.Projects.Clear();
					BindableCollection<AssemblyProject> projects = this.Projects;
					IEnumerable<string> strs = Directory.EnumerateFiles(source, "*.csproj", SearchOption.AllDirectories);
					projects.AddRange(
						from f in strs
						select new AssemblyProject(f));
					BindableCollection<AssemblyProject> assemblyProjects = this.Projects;
					IEnumerable<string> strs1 = Directory.EnumerateFiles(source, "*.vbproj", SearchOption.AllDirectories);
					assemblyProjects.AddRange(
						from f in strs1
						select new AssemblyProject(f));
				}
				catch (Exception exception)
				{
					PlaySharpScreen.Log.Error(source, exception);
				}
			}
		}
	}
}