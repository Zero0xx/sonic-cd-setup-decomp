using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200061C RID: 1564
	public enum NetBiosNodeType
	{
		// Token: 0x04002DD9 RID: 11737
		Unknown,
		// Token: 0x04002DDA RID: 11738
		Broadcast,
		// Token: 0x04002DDB RID: 11739
		Peer2Peer,
		// Token: 0x04002DDC RID: 11740
		Mixed = 4,
		// Token: 0x04002DDD RID: 11741
		Hybrid = 8
	}
}
