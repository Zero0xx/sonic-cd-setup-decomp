using System;

namespace System.Net.Sockets
{
	// Token: 0x020005C2 RID: 1474
	[Flags]
	public enum SocketFlags
	{
		// Token: 0x04002BD3 RID: 11219
		None = 0,
		// Token: 0x04002BD4 RID: 11220
		OutOfBand = 1,
		// Token: 0x04002BD5 RID: 11221
		Peek = 2,
		// Token: 0x04002BD6 RID: 11222
		DontRoute = 4,
		// Token: 0x04002BD7 RID: 11223
		MaxIOVectorLength = 16,
		// Token: 0x04002BD8 RID: 11224
		Truncated = 256,
		// Token: 0x04002BD9 RID: 11225
		ControlDataTruncated = 512,
		// Token: 0x04002BDA RID: 11226
		Broadcast = 1024,
		// Token: 0x04002BDB RID: 11227
		Multicast = 2048,
		// Token: 0x04002BDC RID: 11228
		Partial = 32768
	}
}
