using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Loader.Shared
{
	public class CacheAssembly
	{
		private System.Reflection.AssemblyName assemblyName;

		private string name;

		public string[] Arguments
		{
			get;
			set;
		}

		public System.Reflection.Assembly Assembly
		{
			get;
			set;
		}

		public System.Reflection.AssemblyName AssemblyName
		{
			get
			{
				if (this.assemblyName == null && this.Exists)
				{
					this.assemblyName = System.Reflection.AssemblyName.GetAssemblyName(this.PathToBinary);
				}
				return this.assemblyName;
			}
		}

		public bool Exists
		{
			get
			{
				if (string.IsNullOrEmpty(this.PathToBinary))
				{
					return true;
				}
				return File.Exists(this.PathToBinary);
			}
		}

		public int Id
		{
			get;
			set;
		}

		public bool IsLibrary
		{
			get;
			set;
		}

		public bool IsLoaded
		{
			get
			{
				return this.Assembly != null;
			}
		}

		public string Name
		{
			get
			{
				if (this.name == null && this.AssemblyName != null)
				{
					this.Name = this.AssemblyName.Name;
				}
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public string PathToBinary
		{
			get;
			set;
		}

		public CacheAssembly()
		{
		}

		public override string ToString()
		{
			return string.Format("[{0}] {1}", this.Id, this.Name);
		}
	}
}