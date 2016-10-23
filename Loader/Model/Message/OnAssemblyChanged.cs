using Loader.Model.Config;
using Loader.ViewModels;
using System;
using System.Runtime.CompilerServices;

namespace Loader.Model.Message
{
	public class OnAssemblyChanged
	{
		public IPlaySharpAssembly Assembly
		{
			get;
			set;
		}

		public Loader.ViewModels.AssemblyViewModel AssemblyViewModel
		{
			get;
			set;
		}

		public OnAssemblyChanged(IPlaySharpAssembly packageAssembly)
		{
			this.Assembly = packageAssembly;
		}

		public OnAssemblyChanged(Loader.ViewModels.AssemblyViewModel assemblyViewModel)
		{
			this.AssemblyViewModel = assemblyViewModel;
		}
	}
}