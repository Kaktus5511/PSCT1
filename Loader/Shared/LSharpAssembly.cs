using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Loader.Shared
{
	[DataContract]
	[Serializable]
	public class LSharpAssembly
	{
		[DataMember]
		public int Id
		{
			get;
			set;
		}

		[DataMember]
		public string Name
		{
			get;
			set;
		}

		[DataMember]
		public string PathToBinary
		{
			get;
			set;
		}

		[DataMember]
		public int Type
		{
			get;
			set;
		}

		public LSharpAssembly()
		{
		}
	}
}