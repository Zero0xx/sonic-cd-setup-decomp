using System;

namespace System.Net
{
	// Token: 0x020004BE RID: 1214
	internal enum ReadState
	{
		// Token: 0x0400254D RID: 9549
		Start,
		// Token: 0x0400254E RID: 9550
		StatusLine,
		// Token: 0x0400254F RID: 9551
		Headers,
		// Token: 0x04002550 RID: 9552
		Data
	}
}
