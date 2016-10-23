using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using WPFLocalizeExtension.Extensions;

namespace Loader.Services
{
	public static class Loc
	{
		public static string DefaultAssembly
		{
			get;
			set;
		}

		public static string DefaultDictionary
		{
			get;
			set;
		}

		static Loc()
		{
			Loc.DefaultAssembly = Assembly.GetExecutingAssembly().GetName().Name;
			Loc.DefaultDictionary = "Translation";
		}

		public static T GetValue<T>(string key)
		{
			return LocExtension.GetLocalizedValue<T>(string.Format("{0}:{1}:{2}", Loc.DefaultAssembly, Loc.DefaultDictionary, key), null, null);
		}

		public static string GetValue(string key)
		{
			return LocExtension.GetLocalizedValue<string>(string.Format("{0}:{1}:{2}", Loc.DefaultAssembly, Loc.DefaultDictionary, key), null, null);
		}
	}
}