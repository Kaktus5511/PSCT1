using Loader.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Loader.Model.Config
{
	public abstract class ProfileBase : IProfile
	{
		public IList<IProfileAssembly> Assemblies { get; set; } = new List<IProfileAssembly>();

		public IList<string> Flags { get; set; } = new List<string>();

		public virtual bool Inject
		{
			get;
			set;
		}

		public virtual string Name
		{
			get;
			set;
		}

		protected ProfileBase()
		{
		}

		public virtual async Task<bool> CompileAsync()
		{
			ProfileBase.<CompileAsync>d__16 variable = new ProfileBase.<CompileAsync>d__16();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<ProfileBase.<CompileAsync>d__16>(ref variable);
			return variable.<>t__builder.Task;
		}

		public virtual async Task<bool> UpdateAsync()
		{
			ProfileBase.<UpdateAsync>d__17 variable = new ProfileBase.<UpdateAsync>d__17();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<ProfileBase.<UpdateAsync>d__17>(ref variable);
			return variable.<>t__builder.Task;
		}
	}
}