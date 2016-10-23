using Caliburn.Micro;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Loader.Model
{
	public class AssemblyProject : PropertyChangedBase
	{
		private string filePath;

		public string FilePath
		{
			get
			{
				return this.filePath;
			}
			set
			{
				if (value == this.filePath)
				{
					return;
				}
				this.filePath = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(AssemblyProject)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AssemblyProject).GetMethod("get_FilePath").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(AssemblyProject)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AssemblyProject).GetMethod("get_Name").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string Name
		{
			get
			{
				return Path.GetFileNameWithoutExtension(this.FilePath);
			}
		}

		public AssemblyProject(string file)
		{
			this.FilePath = file;
		}
	}
}