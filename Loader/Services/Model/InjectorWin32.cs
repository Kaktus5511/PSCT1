using Loader.Model;
using PlaySharp.Toolkit.Injector;
using System;
using System.IO;

namespace Loader.Services.Model
{
	public class InjectorWin32 : InjectorClient
	{
		private static InjectorWin32 instance;

		public static InjectorWin32 Instance
		{
			get
			{
				if (InjectorWin32.instance == null)
				{
					InjectorWin32.instance = new InjectorWin32();
				}
				return InjectorWin32.instance;
			}
		}

		public InjectorWin32() : base(Files.X86.Injector.FullName, Files.X86.InjectorNative.FullName, ProcessMode.X86)
		{
		}
	}
}