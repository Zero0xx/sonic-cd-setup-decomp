using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000035 RID: 53
	[ComVisible(true)]
	public interface IDeserializationCallback
	{
		// Token: 0x06000321 RID: 801
		void OnDeserialization(object sender);
	}
}
