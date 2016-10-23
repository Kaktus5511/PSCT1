using Loader.Model;
using PlaySharp.Service.WebService.Model;
using System;
using System.Windows.Controls;

namespace Loader.Model.Config
{
	public interface ISettings
	{
		EndpointEntry ApiEndpoint
		{
			get;
			set;
		}

		bool AssemblyDebug
		{
			get;
			set;
		}

		bool BypassCoreUpdate
		{
			get;
		}

		bool ChampionCheck
		{
			get;
			set;
		}

		string Color
		{
			get;
			set;
		}

		DataGridLength ColumnAuthorWidth
		{
			get;
			set;
		}

		DataGridLength ColumnCheckWidth
		{
			get;
			set;
		}

		DataGridLength ColumnNameWidth
		{
			get;
			set;
		}

		DataGridLength ColumnTypeWidth
		{
			get;
			set;
		}

		DataGridLength ColumnVersionWidth
		{
			get;
			set;
		}

		EndpointEntry DataEndpoint
		{
			get;
			set;
		}

		string Language
		{
			get;
			set;
		}

		bool LibraryCheck
		{
			get;
			set;
		}

		ReleaseChannel LoaderUpdateChannel
		{
			get;
			set;
		}

		double ProfileCollectionWidth
		{
			get;
			set;
		}

		bool ShowDescription
		{
			get;
			set;
		}

		bool ShowNotes
		{
			get;
			set;
		}

		string Theme
		{
			get;
			set;
		}

		bool TosAccepted
		{
			get;
			set;
		}

		bool UpdateSelectedAssembliesOnly
		{
			get;
			set;
		}

		bool UsePlaySharpEnvironment
		{
			get;
			set;
		}

		Loader.Model.UserType UserType
		{
			get;
			set;
		}

		bool UseSandbox
		{
			get;
			set;
		}

		bool UtilityCheck
		{
			get;
			set;
		}

		double WindowHeight
		{
			get;
			set;
		}

		double WindowLeft
		{
			get;
			set;
		}

		double WindowTop
		{
			get;
			set;
		}

		double WindowWidth
		{
			get;
			set;
		}

		int Workers
		{
			get;
			set;
		}
	}
}