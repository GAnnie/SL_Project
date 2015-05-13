using System;

namespace FluorineFx.IO.Readers
{	
	interface IProtoReader
	{
		object ReadData(ProtoReader reader);
	}
}
