using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000704 RID: 1796
	[ComVisible(true)]
	public interface IRemotingFormatter : IFormatter
	{
		// Token: 0x06003FE9 RID: 16361
		object Deserialize(Stream serializationStream, HeaderHandler handler);

		// Token: 0x06003FEA RID: 16362
		void Serialize(Stream serializationStream, object graph, Header[] headers);
	}
}
