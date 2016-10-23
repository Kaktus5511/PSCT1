using Caliburn.Micro;
using Loader.Helpers;
using Loader.Model;
using Loader.Model.Config;
using Loader.Model.Message;
using Loader.ViewModels.Model;
using log4net;
using PlaySharp.Service.Package.Model;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.EventAggregator;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Loader.ViewModels
{
	public class AssemblyViewModel : PlaySharpScreen, IAssemblyView, IScreen, IHaveDisplayName, IActivate, IDeactivate, IGuardClose, IClose, INotifyPropertyChangedEx, INotifyPropertyChanged, PlaySharp.Toolkit.EventAggregator.IHandle<OnAssemblyChanged>, PlaySharp.Toolkit.EventAggregator.IHandle
	{
		private IPlaySharpAssembly assembly;

		public IPlaySharpAssembly Assembly
		{
			get
			{
				return JustDecompileGenerated_get_Assembly();
			}
			set
			{
				JustDecompileGenerated_set_Assembly(value);
			}
		}

		public IPlaySharpAssembly JustDecompileGenerated_get_Assembly()
		{
			return this.assembly;
		}

		private void JustDecompileGenerated_set_Assembly(IPlaySharpAssembly value)
		{
			if (object.Equals(value, this.assembly))
			{
				return;
			}
			this.assembly = value;
			base.NotifyOfPropertyChange<IPlaySharpAssembly>(Expression.Lambda<Func<IPlaySharpAssembly>>(Expression.Property(Expression.Constant(this, typeof(AssemblyViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AssemblyViewModel).GetMethod("get_Assembly").MethodHandle)), new ParameterExpression[0]));
			base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(AssemblyViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AssemblyViewModel).GetMethod("get_ErrorColor").MethodHandle)), new ParameterExpression[0]));
			base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(AssemblyViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AssemblyViewModel).GetMethod("get_HasBuildError").MethodHandle)), new ParameterExpression[0]));
		}

		public string ErrorColor
		{
			get
			{
				PackageAssembly package = this.Assembly as PackageAssembly;
				if (package != null)
				{
					BuildState state = package.AssemblyEntry.State;
					if (state == BuildState.DependencyFailure)
					{
						return "Red";
					}
					if (state == BuildState.BuildFailure)
					{
						return "Yellow";
					}
				}
				LocalAssembly local = this.Assembly as LocalAssembly;
				if (local != null && local.Status == AssemblyStatus.CompilingError)
				{
					return "Red";
				}
				return string.Empty;
			}
		}

		public bool HasBuildError
		{
			get
			{
				PackageAssembly package = this.Assembly as PackageAssembly;
				if (package != null)
				{
					switch (package.AssemblyEntry.State)
					{
						case BuildState.New:
						case BuildState.Success:
						{
							return false;
						}
						case BuildState.DependencyFailure:
						case BuildState.BuildFailure:
						{
							return true;
						}
					}
				}
				LocalAssembly local = this.Assembly as LocalAssembly;
				if (local != null && local.Status == AssemblyStatus.CompilingError)
				{
					return true;
				}
				return false;
			}
		}

		public bool Inject
		{
			get
			{
				if (this.Assembly.Type == AssemblyType.Library)
				{
					this.ProfileAssembly.Inject = true;
				}
				return this.ProfileAssembly.Inject;
			}
			set
			{
				if (value == this.ProfileAssembly.Inject)
				{
					return;
				}
				PlaySharp.Toolkit.EventAggregator.EventAggregatorExtensions.BeginPublishOnUIThread(base.EventAggregator.Value, new OnAssemblyChanged(this));
				this.ProfileAssembly.Inject = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(AssemblyViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AssemblyViewModel).GetMethod("get_Inject").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public IProfileAssembly ProfileAssembly
		{
			get;
		}

		public AssemblyViewModel(IProfileAssembly profileAssembly)
		{
			this.ProfileAssembly = profileAssembly;
			this.Assembly = base.Config.Value.ServiceSettings.Assemblies.FirstOrDefault<IPlaySharpAssembly>((IPlaySharpAssembly a) => a.Id == profileAssembly.Id);
			if (this.Assembly == null)
			{
				PlaySharpScreen.Log.Warn(string.Format("Invalid Assembly ID {0}", profileAssembly.Id));
			}
		}

		public async Task<bool> CompileAsync()
		{
			return await this.Assembly.CompileAsync();
		}

		public void Handle(OnAssemblyChanged message)
		{
			int? nullable;
			int? nullable1;
			int? nullable2;
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			IPlaySharpAssembly assembly = message.Assembly;
			if (assembly != null)
			{
				nullable1 = new int?(assembly.Id);
			}
			else
			{
				nullable = null;
				nullable1 = nullable;
			}
			int? nullable3 = nullable1;
			IPlaySharpAssembly playSharpAssembly = this.Assembly;
			if (playSharpAssembly != null)
			{
				nullable2 = new int?(playSharpAssembly.Id);
			}
			else
			{
				nullable = null;
				nullable2 = nullable;
			}
			int? nullable4 = nullable2;
			if ((nullable3.GetValueOrDefault() == nullable4.GetValueOrDefault() ? nullable3.HasValue == nullable4.HasValue : false))
			{
				this.Assembly = message.Assembly;
			}
		}

		public override string ToString()
		{
			return this.Assembly.ToString();
		}

		public async Task<bool> UpdateAsync()
		{
			return await this.Assembly.UpdateAsync();
		}
	}
}