using Loader.Model;
using Loader.Model.Config;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.Helper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Loader.Services.Model
{
	public interface IUpdateService : IActivatable, IStateful, IDeactivatable
	{
		Task HandleUrl(string url);

		Task<bool> InstallAsync(AssemblyEntry assembly, IProfile profile);

		Task<bool> InstallAsync(IList<AssemblyEntry> assemblies, IProfile profile);

		Task<bool> IsCoreSupportedAsync();

		Task PrepareAssembliesAsync(AssemblyUpdate type, bool forceCompile = false);

		Task<bool> UpdateCoreAsync();

		Task<bool> UpdateCoreAsync(string module, CancellationToken token = null);

		Task UpdateEnsageTextures();

		bool UpdateGamePath();

		Task UpdateLoaderAsync(bool confirm = true);
	}
}