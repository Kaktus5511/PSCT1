using System;

namespace Loader.Model.Config
{
	public class LibrariesProfile : ProfileBase
	{
		public override bool Inject
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public override string Name
		{
			get
			{
				return "Libraries";
			}
			set
			{
			}
		}

		public LibrariesProfile()
		{
		}
	}
}