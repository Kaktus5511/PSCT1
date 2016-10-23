using Loader.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Loader.Helpers
{
	public static class ReferenceResolver
	{
		public static List<string> ProbePaths
		{
			get;
			set;
		}

		static ReferenceResolver()
		{
			ReferenceResolver.ProbePaths = new List<string>();
			ReferenceResolver.ProbePaths.Add(Directories.References);
			ReferenceResolver.ProbePaths.Add(Directories.System);
			ReferenceResolver.ProbePaths.Add(Directories.Core);
		}

		public static string Find(string name)
		{
			string str;
			List<string>.Enumerator enumerator = ReferenceResolver.ProbePaths.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					using (IEnumerator<string> enumerator1 = Directory.EnumerateFiles(enumerator.Current, "*.dll").GetEnumerator())
					{
						while (enumerator1.MoveNext())
						{
							string file = enumerator1.Current;
							if (name != Path.GetFileNameWithoutExtension(file))
							{
								continue;
							}
							str = file;
							return str;
						}
					}
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return str;
		}
	}
}