using Loader.Model.Config;
using Loader.Model.Message;
using log4net;
using PlaySharp.Service.Package.Model;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Loader.Model
{
	public static class ProfileExtensions
	{
		private readonly static ILog Log;

		static ProfileExtensions()
		{
			ProfileExtensions.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public static bool Add(this IProfile profile, IPlaySharpAssembly assembly)
		{
			if (profile == null)
			{
				throw new ArgumentNullException("profile");
			}
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			PlaySharpConfig config = IoC.Get<PlaySharpConfig>(null);
			if (config == null)
			{
				throw new ArgumentNullException("PlaySharpConfig");
			}
			if (!config.ServiceSettings.Assemblies.Contains(assembly))
			{
				config.ServiceSettings.Assemblies.Add(assembly);
			}
			if (!profile.Assemblies.All<IProfileAssembly>((IProfileAssembly a) => a.Id != assembly.Id))
			{
				return false;
			}
			profile.Assemblies.Add(new ProfileAssembly(assembly.Id));
			return true;
		}

		public static IProfile Create<T>(this List<IProfile> profiles, string name = null, bool inject = true)
		where T : IProfile, new()
		{
			IProfile profile = profiles.AddItem<IProfile>(Activator.CreateInstance<T>(), 0);
			profile.Name = name ?? "New Profile";
			profile.Inject = inject;
			EventAggregatorExtensions.BeginPublishOnUIThread(IoC.Get<IEventAggregator>(null), new OnProfilesChanged());
			return profile;
		}

		public static IEnumerable<IPlaySharpAssembly> GetAssemblies(this IProfile profile)
		{
			if (profile == null)
			{
				throw new ArgumentNullException("profile");
			}
			PlaySharpConfig config = IoC.Get<PlaySharpConfig>(null);
			if (config == null)
			{
				throw new ArgumentNullException("PlaySharpConfig");
			}
			List<IPlaySharpAssembly> assemblies = new List<IPlaySharpAssembly>();
			foreach (IProfileAssembly profileAssembly in profile.Assemblies)
			{
				IPlaySharpAssembly assembly = config.ServiceSettings.Assemblies.FirstOrDefault<IPlaySharpAssembly>((IPlaySharpAssembly a) => a.Id == profileAssembly.Id);
				if (assembly != null)
				{
					assemblies.Add(assembly);
				}
				else
				{
					ProfileExtensions.Log.Warn(string.Format("Could not find Target Assembly {0} @ {1}", profileAssembly.Id, profile.Name));
				}
			}
			return assemblies;
		}

		public static IPlaySharpAssembly GetAssembly(this IProfileAssembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			PlaySharpConfig playSharpConfig = IoC.Get<PlaySharpConfig>(null);
			if (playSharpConfig == null)
			{
				throw new ArgumentNullException("PlaySharpConfig");
			}
			IPlaySharpAssembly target = playSharpConfig.ServiceSettings.Assemblies.FirstOrDefault<IPlaySharpAssembly>((IPlaySharpAssembly a) => a.Id == assembly.Id);
			if (target != null)
			{
				return target;
			}
			ProfileExtensions.Log.Warn(string.Format("Could not find Target Assembly {0}", assembly.Id));
			return null;
		}

		public static IProfile GetOrCreate<T>(this List<IProfile> profiles, string name = null, bool inject = true)
		where T : IProfile, new()
		{
			T[] typeProfiles = profiles.OfType<T>().ToArray<T>();
			if (name.IsNullOrEmpty())
			{
				T typeProfile = typeProfiles.FirstOrDefault<T>();
				if (typeProfile != null)
				{
					return (object)typeProfile;
				}
				return profiles.Create<T>(name, inject);
			}
			T namedProfile = typeProfiles.FirstOrDefault<T>((T p) => p.Name == name);
			if (namedProfile != null)
			{
				return (object)namedProfile;
			}
			return profiles.Create<T>(name, inject);
		}

		public static void Remove(this IProfile profile, int assembly)
		{
			Func<IProfileAssembly, bool> func;
			Func<IProfileAssembly, bool> func1 = null;
			Func<IProfileAssembly, bool> func2 = null;
			if (profile == null)
			{
				throw new ArgumentNullException("profile");
			}
			PlaySharpConfig config = IoC.Get<PlaySharpConfig>(null);
			IPackageClient service = IoC.Get<IPackageClient>(null);
			if (config == null)
			{
				throw new ArgumentNullException("PlaySharpConfig");
			}
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			IProfileAssembly[] array = profile.Assemblies.ToArray<IProfileAssembly>();
			Func<IProfileAssembly, bool> func3 = func1;
			if (func3 == null)
			{
				Func<IProfileAssembly, bool> id = (IProfileAssembly a) => a.Id == assembly;
				func = id;
				func1 = id;
				func3 = func;
			}
			foreach (IProfileAssembly profileAssembly in ((IEnumerable<IProfileAssembly>)array).Where<IProfileAssembly>(func3))
			{
				profile.Assemblies.Remove(profileAssembly);
			}
			foreach (IProfile profile1 in config.ServiceSettings.Profiles)
			{
				IList<IProfileAssembly> assemblies = profile1.Assemblies;
				Func<IProfileAssembly, bool> func4 = func2;
				if (func4 == null)
				{
					Func<IProfileAssembly, bool> id1 = (IProfileAssembly a) => a.Id == assembly;
					func = id1;
					func2 = id1;
					func4 = func;
				}
				if (!assemblies.Any<IProfileAssembly>(func4))
				{
					continue;
				}
				return;
			}
			config.ServiceSettings.Assemblies.RemoveAll((IPlaySharpAssembly a) => a.Id == assembly);
			service.Uninstall(assembly);
		}
	}
}