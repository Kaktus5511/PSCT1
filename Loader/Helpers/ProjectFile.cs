using Loader.Model.Config;
using log4net;
using Microsoft.Build.Evaluation;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Loader.Helpers
{
	public class ProjectFile : IDisposable
	{
		private readonly static ILog Log;

		public readonly Microsoft.Build.Evaluation.Project Project;

		public string Configuration
		{
			get
			{
				return "Debug";
			}
		}

		public string PlatformTarget
		{
			get
			{
				return IoC.Get<PlaySharpConfig>(null).ServiceSettings.PlatformTarget;
			}
		}

		static ProjectFile()
		{
			ProjectFile.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public ProjectFile(string file)
		{
			try
			{
				if (File.Exists(file))
				{
					ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
					this.Project = new Microsoft.Build.Evaluation.Project(file);
				}
			}
			catch (Exception exception)
			{
				ProjectFile.Log.Warn(exception);
			}
		}

		public void Change()
		{
			try
			{
				if (this.Project != null)
				{
					this.Project.SetGlobalProperty("Configuration", this.Configuration);
					this.Project.SetGlobalProperty("Platform", this.PlatformTarget);
					this.Project.SetGlobalProperty("PlatformTarget", this.PlatformTarget);
					this.Project.SetGlobalProperty("PreBuildEvent", string.Empty);
					this.Project.SetGlobalProperty("PostBuildEvent", string.Empty);
					this.Project.SetGlobalProperty("PreLinkEvent", string.Empty);
					this.Project.SetGlobalProperty("DebugSymbols", "true");
					this.Project.SetGlobalProperty("DebugType", "full");
					this.Project.SetGlobalProperty("Optimize", "false");
					this.Project.SetGlobalProperty("DefineConstants", "TRACE");
					this.Project.SetGlobalProperty("OutputPath", string.Format("bin\\{0}\\", this.Configuration));
					foreach (ProjectItem item in this.Project.GetItems("Reference"))
					{
						string assembly = item.EvaluatedInclude;
						if (assembly.Contains(","))
						{
							assembly = assembly.Split(new char[] { ',' }).First<string>();
						}
						string reference = ReferenceResolver.Find(assembly);
						if (reference.IsNullOrEmpty())
						{
							continue;
						}
						item.SetMetadataValue("HintPath", reference);
					}
					this.Project.Save();
				}
			}
			catch (Exception exception)
			{
				ProjectFile.Log.Warn(exception.Message);
			}
		}

		public void Dispose()
		{
			ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
		}
	}
}