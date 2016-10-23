using Loader.Model;
using log4net;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Logging;
using PlaySharp.Toolkit.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Loader.Services
{
	internal class Compiler
	{
		private readonly static List<string> ItemsTypeBlackList;

		private readonly static ILog Log;

		static Compiler()
		{
			Compiler.ItemsTypeBlackList = new List<string>()
			{
				"PreBuildEvent",
				"PostBuildEvent",
				"PreLinkEvent",
				"CustomBuildStep"
			};
			Compiler.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public Compiler()
		{
		}

		public static bool Compile(Project project, string logfile)
		{
			bool flag;
			try
			{
				if (project == null)
				{
					return false;
				}
				else
				{
					foreach (ProjectItem item in project.Items)
					{
						try
						{
							if (Compiler.ItemsTypeBlackList.FindIndex((string listItem) => listItem.Equals(item.ItemType, StringComparison.InvariantCultureIgnoreCase)) >= 0)
							{
								Compiler.Log.Error(string.Format("Compile - Blacklisted item type detected - {0}", project.FullPath));
								flag = false;
								return flag;
							}
						}
						catch
						{
						}
					}
					bool doLog = false;
					string logErrorFile = Path.Combine(Directories.Logs, string.Concat("Error - ", Path.GetFileName(logfile)));
					if (File.Exists(logErrorFile))
					{
						File.Delete(logErrorFile);
					}
					if (!string.IsNullOrWhiteSpace(logfile))
					{
						string logDir = Path.GetDirectoryName(logfile);
						if (!string.IsNullOrWhiteSpace(logDir))
						{
							doLog = true;
							if (!Directory.Exists(logDir))
							{
								Directory.CreateDirectory(logDir);
							}
							FileLogger fileLogger = new FileLogger()
							{
								Parameters = string.Concat("logfile=", logfile),
								ShowSummary = true
							};
							ProjectCollection.GlobalProjectCollection.RegisterLogger(fileLogger);
						}
					}
					bool result = project.Build();
					ProjectCollection.GlobalProjectCollection.UnregisterAllLoggers();
					ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
					if (!result)
					{
						Compiler.Log.Info(string.Format("Compile - Check ./logs/ for details - {0}", project.FullPath));
					}
					else
					{
						Compiler.Log.Info(string.Format("Compile - {0}", project.FullPath));
					}
					if (!result & doLog && File.Exists(logfile))
					{
						if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(logfile)))
						{
							File.Move(logfile, Path.Combine(Directories.Logs, string.Concat("Error - ", Path.GetFileName(logfile))));
						}
					}
					else if (result && File.Exists(logfile))
					{
						File.Delete(logfile);
					}
					flag = result;
				}
			}
			catch (Exception exception)
			{
				Compiler.Log.Error(exception);
				return false;
			}
			return flag;
		}

		public static string GetOutputFilePath(Project project)
		{
			string str;
			if (project != null)
			{
				if (project.GetPropertyValue("OutputType").ToLower().Contains("exe"))
				{
					str = ".exe";
				}
				else
				{
					str = (project.GetPropertyValue("OutputType").ToLower() == "library" ? ".dll" : string.Empty);
				}
				string extension = str;
				string pathDir = Path.GetDirectoryName(project.FullPath);
				if (!string.IsNullOrWhiteSpace(extension) && !string.IsNullOrWhiteSpace(pathDir))
				{
					return Path.Combine(pathDir, project.GetPropertyValue("OutputPath"), string.Concat(project.GetPropertyValue("AssemblyName"), extension));
				}
			}
			return string.Empty;
		}
	}
}