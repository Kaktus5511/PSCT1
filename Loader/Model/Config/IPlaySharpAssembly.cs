using PlaySharp.Service.WebService.Model;
using PlaySharp.Toolkit.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loader.Model.Config
{
	public interface IPlaySharpAssembly
	{
		string Author
		{
			get;
		}

		string AuthorColor
		{
			get;
		}

		IReadOnlyList<string> Champions
		{
			get;
		}

		string Description
		{
			get;
		}

		string DisplayName
		{
			get;
			set;
		}

		int Id
		{
			get;
		}

		bool IsCached
		{
			get;
		}

		bool IsValid
		{
			get;
		}

		string Location
		{
			get;
		}

		string Name
		{
			get;
		}

		string Note
		{
			get;
			set;
		}

		string PathToBinary
		{
			get;
		}

		ServiceType Service
		{
			get;
		}

		AssemblyType Type
		{
			get;
		}

		string Url
		{
			get;
		}

		string VersionString
		{
			get;
		}

		Task<bool> CompileAsync();

		Task<bool> UpdateAsync();
	}
}