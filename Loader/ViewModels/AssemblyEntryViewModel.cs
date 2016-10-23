using Caliburn.Micro;
using Loader.Helpers;
using PlaySharp.Service.WebService.Model;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Loader.ViewModels
{
	public class AssemblyEntryViewModel : PlaySharpScreen
	{
		private AssemblyEntry assembly;

		private bool installChecked;

		public AssemblyEntry Assembly
		{
			get
			{
				return this.assembly;
			}
			set
			{
				if (object.Equals(value, this.assembly))
				{
					return;
				}
				this.assembly = value;
				base.NotifyOfPropertyChange<AssemblyEntry>(Expression.Lambda<Func<AssemblyEntry>>(Expression.Property(Expression.Constant(this, typeof(AssemblyEntryViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AssemblyEntryViewModel).GetMethod("get_Assembly").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string ErrorColor
		{
			get
			{
				BuildState state = this.Assembly.State;
				if (state == BuildState.DependencyFailure)
				{
					return "Red";
				}
				if (state == BuildState.BuildFailure)
				{
					return "Yellow";
				}
				return string.Empty;
			}
		}

		public bool HasBuildError
		{
			get
			{
				BuildState state = this.Assembly.State;
				if (state != BuildState.DependencyFailure && state != BuildState.BuildFailure)
				{
					return false;
				}
				return true;
			}
		}

		public bool InstallChecked
		{
			get
			{
				return this.installChecked;
			}
			set
			{
				if (value == this.installChecked)
				{
					return;
				}
				this.installChecked = value;
				base.NotifyOfPropertyChange<bool>(Expression.Lambda<Func<bool>>(Expression.Property(Expression.Constant(this, typeof(AssemblyEntryViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AssemblyEntryViewModel).GetMethod("get_InstallChecked").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public AssemblyEntryViewModel(AssemblyEntry assembly)
		{
			this.Assembly = assembly;
		}

		public AssemblyEntryViewModel()
		{
		}
	}
}