using System;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x0200073C RID: 1852
	internal struct MessageData
	{
		// Token: 0x0400212F RID: 8495
		internal IntPtr pFrame;

		// Token: 0x04002130 RID: 8496
		internal IntPtr pMethodDesc;

		// Token: 0x04002131 RID: 8497
		internal IntPtr pDelegateMD;

		// Token: 0x04002132 RID: 8498
		internal IntPtr pSig;

		// Token: 0x04002133 RID: 8499
		internal IntPtr thGoverningType;

		// Token: 0x04002134 RID: 8500
		internal int iFlags;
	}
}
