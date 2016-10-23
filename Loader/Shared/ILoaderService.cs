using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Loader.Shared
{
	[ServiceContract]
	public interface ILoaderService
	{
		[OperationContract]
		List<LSharpAssembly> GetAssemblyList();

		[OperationContract]
		Configuration GetConfiguration();

		[OperationContract]
		void Recompile();

		[OperationContract(IsOneWay=true)]
		void SetUnitName(string name);
	}
}