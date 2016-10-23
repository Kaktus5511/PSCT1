using Loader.Model;
using Loader.Model.Config;
using Loader.Properties;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows;

namespace Loader
{
	public partial class App : Application
	{
		private static bool created;

		private static string currentFile;

		private static string loaderFile;

		private static string CurrentFile
		{
			get
			{
				if (App.currentFile == null)
				{
					App.currentFile = Environment.GetCommandLineArgs().First<string>();
				}
				return App.currentFile;
			}
		}

		private static string LoaderFile
		{
			get
			{
				if (App.loaderFile == null)
				{
					App.loaderFile = Path.Combine(Directories.Current, "Loader.exe");
				}
				return App.loaderFile;
			}
		}

		private static System.Threading.Mutex Mutex
		{
			get;
			set;
		}

		static App()
		{
			try
			{
				Logs.AppName = "Loader";
				CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
				CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
				Utility.SyncTime((new CancellationTokenSource(TimeSpan.FromSeconds(5))).Token);
				Extensions.SetFullAccessControl(Directories.Current);
				App.Mutex = new System.Threading.Mutex(true, HashFactory.MD5.String(Environment.UserName), out App.created);
				App.UriHandler();
				App.TerminateRunning();
				App.Randomize();
				Utility.CreateRegistryCommand("ps", App.CurrentFile);
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.ToString(), "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}

		public App()
		{
			this.InitializeComponent();
		}

		private static void CleanupRandomization()
		{
			string loader = App.LoaderFile;
			string config = Path.ChangeExtension(App.LoaderFile, ".exe.config");
			string pdb = Path.ChangeExtension(App.LoaderFile, ".pdb");
			string str = Path.Combine(Path.GetTempPath(), "update.cmd");
			loader.DeleteFile();
			config.DeleteFile();
			pdb.DeleteFile();
			str.DeleteFile();
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(App.OnProcessExit);
		}

		public static void Feelsbadman(string filePath)
		{
			try
			{
				using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					Guid guid = Guid.NewGuid();
					int time = (new Random(guid.GetHashCode())).Next(1451602800, (int)DateTime.UtcNow.ToUnixTime());
					BinaryReader reader = new BinaryReader(fs);
					BinaryWriter binaryWriter = new BinaryWriter(fs);
					fs.Seek((long)60, SeekOrigin.Begin);
					fs.Seek((long)reader.ReadInt32(), SeekOrigin.Begin);
					fs.Seek((long)8, SeekOrigin.Current);
					binaryWriter.Write(time);
				}
			}
			catch
			{
			}
		}

		private static void OnProcessExit(object sender, EventArgs eventArgs)
		{
			Process.Start(new ProcessStartInfo()
			{
				Arguments = string.Concat("/C choice /C Y /N /D Y /T 1 & Del \"", Path.ChangeExtension(App.LoaderFile, ".pdb"), "\""),
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				FileName = "cmd.exe"
			});
		}

		private static void Randomize()
		{
			if (StringExtensions.Contains(App.CurrentFile, "playsharp", StringComparison.OrdinalIgnoreCase) || StringExtensions.Contains(App.CurrentFile, "leaguesharp", StringComparison.OrdinalIgnoreCase))
			{
				App.RandomizePath();
			}
			if (StringExtensions.Contains(Assembly.GetExecutingAssembly().Location, App.LoaderFile, StringComparison.OrdinalIgnoreCase))
			{
				App.RandomizeFile();
				return;
			}
			App.CleanupRandomization();
		}

		private static void RandomizeFile()
		{
			object uniqueKey;
			string name = Utility.GetUniqueKey(8);
			try
			{
				if (File.Exists(PlaySharpConfig.FilePath))
				{
					PlaySharpConfig playSharpConfig = PlaySharpConfig.Init();
					if (playSharpConfig != null)
					{
						uniqueKey = playSharpConfig.Name;
					}
					else
					{
						uniqueKey = null;
					}
					if (uniqueKey == null)
					{
						uniqueKey = Utility.GetUniqueKey(8);
					}
					name = (string)uniqueKey;
				}
			}
			catch
			{
			}
			string config = Path.ChangeExtension(App.LoaderFile, ".exe.config");
			string pdb = Path.ChangeExtension(App.LoaderFile, ".pdb");
			string rndLoader = Path.Combine(Directories.Current, string.Format("{0}.exe", name));
			string rndConfig = Path.ChangeExtension(rndLoader, ".exe.config");
			string rndPdb = Path.ChangeExtension(rndLoader, ".pdb");
			if (File.Exists(App.LoaderFile))
			{
				File.Copy(App.LoaderFile, rndLoader, true);
			}
			if (File.Exists(config))
			{
				File.Copy(config, rndConfig, true);
			}
			if (File.Exists(pdb))
			{
				File.Copy(pdb, rndPdb, true);
			}
			App.Feelsbadman(rndLoader);
			Process.Start(rndLoader);
			Environment.Exit(0);
		}

		private static void RandomizePath()
		{
			object uniqueKey;
			string name = Utility.GetUniqueKey(8);
			try
			{
				if (File.Exists(PlaySharpConfig.FilePath))
				{
					PlaySharpConfig playSharpConfig = PlaySharpConfig.Init();
					if (playSharpConfig != null)
					{
						uniqueKey = playSharpConfig.Name;
					}
					else
					{
						uniqueKey = null;
					}
					if (uniqueKey == null)
					{
						uniqueKey = Utility.GetUniqueKey(8);
					}
					name = (string)uniqueKey;
				}
			}
			catch
			{
			}
			string oldPath = Directory.GetCurrentDirectory();
			string newPath = Utility.RandomizeKeywords(oldPath, new string[] { "play", "league", "sharp" });
			string str = Path.Combine(Path.GetTempPath(), "update.cmd");
			string log = Path.Combine(Directories.Logs, "randomizer.log");
			string oldLoader = Path.Combine(oldPath, "Loader.exe");
			string oldConfig = Path.Combine(oldPath, "Loader.exe.config");
			string oldPdb = Path.Combine(oldPath, "Loader.pdb");
			string newLoader = string.Format("{0}.exe", name);
			string newConfig = string.Format("{0}.exe.config", name);
			string newPdb = string.Format("{0}.pdb", name);
			string newLoaderPath = Path.Combine(newPath, newLoader);
			MessageBoxResult messageBoxResult = MessageBox.Show(string.Format(Translation.Message_PathRandomization_Body.Replace("\\n", Environment.NewLine), newPath), Translation.Message_PathRandomization_Title, MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("@echo off");
			sb.AppendLine("echo ############################################################################################");
			sb.AppendLine("echo #                                                                                          #");
			sb.AppendLine("echo #  ########  ##          ###    ##    ##  ######  ##     ##    ###    ########  ########   #");
			sb.AppendLine("echo #  ##     ## ##         ## ##    ##  ##  ##    ## ##     ##   ## ##   ##     ## ##     ##  #");
			sb.AppendLine("echo #  ##     ## ##        ##   ##    ####   ##       ##     ##  ##   ##  ##     ## ##     ##  #");
			sb.AppendLine("echo #  ########  ##       ##     ##    ##     ######  ######### ##     ## ########  ########   #");
			sb.AppendLine("echo #  ##        ##       #########    ##          ## ##     ## ######### ##   ##   ##         #");
			sb.AppendLine("echo #  ##        ##       ##     ##    ##    ##    ## ##     ## ##     ## ##    ##  ##         #");
			sb.AppendLine("echo #  ##        ######## ##     ##    ##     ######  ##     ## ##     ## ##     ## ##         #");
			sb.AppendLine("echo #                                                                                          #");
			sb.AppendLine("echo ############################################################################################");
			sb.AppendLine("echo #                                                                                          #");
			sb.AppendLine("echo #                                PlaySharp Path Randomizer                                 #");
			sb.AppendLine("echo #              Your PlaySharp Loader files will be randomized, Please wait.                #");
			sb.AppendLine("echo #                                                                                          #");
			sb.AppendLine("echo ############################################################################################");
			sb.AppendLine("timeout /t 5 >nul");
			sb.AppendLine(string.Format("rename \"{0}\" \"{1}\"", oldLoader, newLoader));
			sb.AppendLine(string.Format("rename \"{0}\" \"{1}\"", oldConfig, newConfig));
			sb.AppendLine(string.Format("rename \"{0}\" \"{1}\"", oldPdb, newPdb));
			sb.AppendLine(string.Format("xcopy \"{0}\" \"{1}\" /E /H /R /Y /I > \"{2}\"", oldPath, newPath, log));
			sb.AppendLine("if %ERRORLEVEL% == 0 (");
			sb.AppendLine(string.Format("start /d \"{0}\" {1}", newPath, newLoader));
			sb.AppendLine(string.Format("rmdir \"{0}\" /S /Q", oldPath));
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				string desktopShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), string.Format("{0}.exe", name));
				sb.AppendLine(string.Format("mklink \"{0}\" \"{1}\"", desktopShortcut, newLoaderPath));
			}
			sb.AppendLine("(goto) 2>nul & del \"%~f0\"");
			sb.AppendLine(")");
			sb.AppendLine(string.Format("rmdir \"{0}\" /S /Q", newPath));
			sb.AppendLine("echo ---------------------------------------------");
			sb.AppendLine("echo Copy failed for some reason.");
			sb.AppendLine(string.Format("start \"\" \"{0}\"", log));
			sb.AppendLine("pause");
			sb.AppendLine("(goto) 2>nul & del \"%~f0\"");
			File.WriteAllText(str, sb.ToString());
			if (!Directory.Exists(Directories.Logs))
			{
				Directory.CreateDirectory(Directories.Logs);
			}
			Process.Start(new ProcessStartInfo(str)
			{
				WindowStyle = ProcessWindowStyle.Normal,
				WorkingDirectory = Path.GetTempPath()
			});
			Environment.Exit(0);
		}

		private static void TerminateRunning()
		{
			if (App.created)
			{
				return;
			}
			if (MessageBox.Show("Another instance of the launcher is running. Do you want to terminate it?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
			{
				Environment.Exit(0);
				return;
			}
			foreach (Process process in 
				from p in (IEnumerable<Process>)Process.GetProcessesByName(Path.GetFileNameWithoutExtension(App.CurrentFile))
				where p.Id != Process.GetCurrentProcess().Id
				select p)
			{
				try
				{
					process.CloseMainWindow();
					for (int i = 0; i < 10 && !process.HasExited; i++)
					{
						Thread.Sleep(250);
					}
					if (!process.HasExited)
					{
						process.Kill();
					}
				}
				catch
				{
				}
			}
		}

		private static void UriHandler()
		{
			string[] args = Environment.GetCommandLineArgs();
			if ((int)args.Length == 2 && args[1].StartsWith("ps"))
			{
				try
				{
					string url = HttpUtility.UrlDecode(args[1]);
					if (url != null && url.StartsWith("ps://"))
					{
						File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache", "install.url"), url);
					}
				}
				catch
				{
				}
				if (!App.created)
				{
					Environment.Exit(0);
				}
			}
		}
	}
}