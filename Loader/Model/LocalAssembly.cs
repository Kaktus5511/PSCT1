using Loader.Helpers;
using Loader.Model.Config;
using Loader.Services;
using log4net;
using Newtonsoft.Json;
using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using PlaySharp.Toolkit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Loader.Model
{
	[XmlType(AnonymousType=true)]
	public class LocalAssembly : IPlaySharpAssembly, INotifyPropertyChanged, IEquatable<IPlaySharpAssembly>
	{
		private readonly static ILog Log;

		private string author;

		private string description;

		private int id;

		private string name;

		private string pathToBinary;

		private string pathToProjectFile;

		private AssemblyStatus status;

		private AssemblyType type;

		private string url;

		public string Author
		{
			get
			{
				return JustDecompileGenerated_get_Author();
			}
			set
			{
				JustDecompileGenerated_set_Author(value);
			}
		}

		public string JustDecompileGenerated_get_Author()
		{
			return this.author;
		}

		public void JustDecompileGenerated_set_Author(string value)
		{
			if (value == this.author)
			{
				return;
			}
			this.author = value;
			this.OnPropertyChanged("Author");
		}

		public string AuthorColor
		{
			get
			{
				return "Black";
			}
		}

		public IReadOnlyList<string> Champions
		{
			get
			{
				return JustDecompileGenerated_get_Champions();
			}
			set
			{
				JustDecompileGenerated_set_Champions(value);
			}
		}

		private IReadOnlyList<string> JustDecompileGenerated_Champions_k__BackingField = new string[0];

		public IReadOnlyList<string> JustDecompileGenerated_get_Champions()
		{
			return this.JustDecompileGenerated_Champions_k__BackingField;
		}

		public void JustDecompileGenerated_set_Champions(IReadOnlyList<string> value)
		{
			this.JustDecompileGenerated_Champions_k__BackingField = value;
		}

		public string Description
		{
			get
			{
				return JustDecompileGenerated_get_Description();
			}
			set
			{
				JustDecompileGenerated_set_Description(value);
			}
		}

		public string JustDecompileGenerated_get_Description()
		{
			return this.description;
		}

		public void JustDecompileGenerated_set_Description(string value)
		{
			if (value == this.description)
			{
				return;
			}
			this.description = value;
			this.OnPropertyChanged("Description");
		}

		public string DisplayName
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		public int Id
		{
			get
			{
				return JustDecompileGenerated_get_Id();
			}
			set
			{
				JustDecompileGenerated_set_Id(value);
			}
		}

		public int JustDecompileGenerated_get_Id()
		{
			if (this.id == 0)
			{
				this.id = this.GetHashCode();
			}
			return this.id;
		}

		public void JustDecompileGenerated_set_Id(int value)
		{
			this.id = value;
		}

		public bool IsCached
		{
			get
			{
				if (string.IsNullOrEmpty(this.PathToBinary))
				{
					return false;
				}
				return File.Exists(this.PathToBinary);
			}
		}

		[JsonIgnore]
		public bool IsValid
		{
			get
			{
				return File.Exists(this.PathToProjectFile);
			}
		}

		public string Location
		{
			get
			{
				if (this.Url == string.Empty)
				{
					return "Local";
				}
				return this.Url;
			}
		}

		public string Name
		{
			get
			{
				return JustDecompileGenerated_get_Name();
			}
			set
			{
				JustDecompileGenerated_set_Name(value);
			}
		}

		public string JustDecompileGenerated_get_Name()
		{
			return this.name;
		}

		public void JustDecompileGenerated_set_Name(string value)
		{
			if (value == this.name)
			{
				return;
			}
			this.name = value;
			this.OnPropertyChanged("Name");
		}

		public string Note
		{
			get;
			set;
		}

		[JsonIgnore]
		public string PathToBinary
		{
			get
			{
				string str;
				try
				{
					if (this.pathToBinary == null)
					{
						using (ProjectFile project = this.GetProject())
						{
							project.Change();
							string binFileName = Path.GetFileName(Loader.Services.Compiler.GetOutputFilePath(project.Project));
							if (this.Type != AssemblyType.Library)
							{
								this.pathToBinary = Path.Combine(Directories.Assemblies, string.Format("{0}.exe", this.GetHashCode()));
							}
							else
							{
								this.pathToBinary = Path.Combine(Directories.References, binFileName);
							}
						}
					}
					str = this.pathToBinary;
				}
				catch
				{
					str = null;
				}
				return str;
			}
		}

		public string PathToProjectFile
		{
			get
			{
				return this.pathToProjectFile;
			}
			set
			{
				if (value == this.pathToProjectFile)
				{
					return;
				}
				this.pathToProjectFile = value;
				this.OnPropertyChanged("PathToProjectFile");
				this.OnPropertyChanged("PathToBinary");
			}
		}

		public ServiceType Service
		{
			get
			{
				return JustDecompileGenerated_get_Service();
			}
			set
			{
				JustDecompileGenerated_set_Service(value);
			}
		}

		private ServiceType JustDecompileGenerated_Service_k__BackingField;

		public ServiceType JustDecompileGenerated_get_Service()
		{
			return this.JustDecompileGenerated_Service_k__BackingField;
		}

		public void JustDecompileGenerated_set_Service(ServiceType value)
		{
			this.JustDecompileGenerated_Service_k__BackingField = value;
		}

		public AssemblyStatus Status
		{
			get
			{
				return this.status;
			}
			set
			{
				if (value == this.status)
				{
					return;
				}
				this.status = value;
				this.OnPropertyChanged("Status");
				this.OnPropertyChanged("VersionString");
			}
		}

		public AssemblyType Type
		{
			get
			{
				return JustDecompileGenerated_get_Type();
			}
			set
			{
				JustDecompileGenerated_set_Type(value);
			}
		}

		public AssemblyType JustDecompileGenerated_get_Type()
		{
			return this.type;
		}

		public void JustDecompileGenerated_set_Type(AssemblyType value)
		{
			if (value == this.type)
			{
				return;
			}
			this.type = value;
			this.OnPropertyChanged("Type");
			this.OnPropertyChanged("PathToBinary");
		}

		public string Url
		{
			get
			{
				return JustDecompileGenerated_get_Url();
			}
			set
			{
				JustDecompileGenerated_set_Url(value);
			}
		}

		public string JustDecompileGenerated_get_Url()
		{
			return this.url;
		}

		public void JustDecompileGenerated_set_Url(string value)
		{
			if (value == this.url)
			{
				return;
			}
			this.url = value;
			this.OnPropertyChanged("Url");
			this.OnPropertyChanged("Location");
		}

		public string VersionString
		{
			get
			{
				if (this.Status != AssemblyStatus.Ready)
				{
					return this.Status.ToString();
				}
				if (string.IsNullOrEmpty(this.PathToBinary) || !File.Exists(this.PathToBinary))
				{
					return "?";
				}
				return AssemblyName.GetAssemblyName(this.PathToBinary).Version.ToString();
			}
		}

		static LocalAssembly()
		{
			LocalAssembly.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public LocalAssembly()
		{
		}

		public LocalAssembly(string name, string path, string url)
		{
			this.Name = name;
			this.PathToProjectFile = path;
			this.Url = url;
			this.Description = string.Empty;
			this.Status = AssemblyStatus.Ready;
		}

		public async Task<bool> CompileAsync()
		{
			bool flag1 = await Task<bool>.Factory.StartNew(() => {
				bool flag;
				try
				{
					this.Status = AssemblyStatus.Compiling;
					using (ProjectFile project = this.GetProject())
					{
						project.Change();
						if (Loader.Services.Compiler.Compile(project.Project, Path.Combine(Directories.Logs, string.Concat(this.Name, ".log"))))
						{
							bool result = true;
							string assemblySource = Loader.Services.Compiler.GetOutputFilePath(project.Project);
							string assemblyDestination = this.PathToBinary;
							string pdbSource = Path.ChangeExtension(assemblySource, ".pdb");
							string pdbDestination = Path.ChangeExtension(assemblyDestination, ".pdb");
							if (File.Exists(assemblySource))
							{
								result = Utility.OverwriteFile(assemblySource, assemblyDestination);
							}
							if (File.Exists(pdbSource))
							{
								Utility.OverwriteFile(pdbSource, pdbDestination);
							}
							if (!result)
							{
								this.Status = AssemblyStatus.CompilingError;
							}
							else
							{
								this.Status = AssemblyStatus.Ready;
							}
							flag = result;
							return flag;
						}
					}
					this.Status = AssemblyStatus.CompilingError;
					flag = false;
				}
				catch (Exception exception)
				{
					LocalAssembly.Log.Error(exception);
					flag = false;
				}
				return flag;
			});
			return flag1;
		}

		public bool Equals(IPlaySharpAssembly other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			return this.Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			IPlaySharpAssembly assembly = obj as IPlaySharpAssembly;
			return this.Equals(assembly);
		}

		public override int GetHashCode()
		{
			return this.PathToProjectFile.GetHashCode();
		}

		private ProjectFile GetProject()
		{
			ProjectFile projectFile;
			try
			{
				projectFile = new ProjectFile(this.PathToProjectFile);
			}
			catch (Exception exception)
			{
				Exception e = exception;
				if (!e.Message.Contains("System.Threading.Tasks.Dataflow"))
				{
					LocalAssembly.Log.Error(e);
					projectFile = null;
				}
				else
				{
					MessageBox.Show("Microsoft Build Tools 2015 not found or corrupted.\nhttps://www.microsoft.com/de-de/download/details.aspx?id=48159", "MSBuild", MessageBoxButton.OK, MessageBoxImage.Hand);
					Process.Start("https://www.microsoft.com/de-de/download/details.aspx?id=48159");
					projectFile = null;
				}
			}
			return projectFile;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler == null)
			{
				return;
			}
			propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
		}

		public static bool operator ==(LocalAssembly left, IPlaySharpAssembly right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(LocalAssembly left, IPlaySharpAssembly right)
		{
			return !object.Equals(left, right);
		}

		public override string ToString()
		{
			return string.Format("[{0}-{1}] {2}", this.Id, this.VersionString, this.Name);
		}

		public async Task<bool> UpdateAsync()
		{
			LocalAssembly.<UpdateAsync>d__73 variable = new LocalAssembly.<UpdateAsync>d__73();
			variable.<>4__this = this;
			variable.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			variable.<>1__state = -1;
			variable.<>t__builder.Start<LocalAssembly.<UpdateAsync>d__73>(ref variable);
			return variable.<>t__builder.Task;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}