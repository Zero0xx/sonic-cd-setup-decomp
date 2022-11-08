using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000861 RID: 2145
	[ComVisible(true)]
	[Serializable]
	public enum PaddingMode
	{
		// Token: 0x0400288A RID: 10378
		None = 1,
		// Token: 0x0400288B RID: 10379
		PKCS7,
		// Token: 0x0400288C RID: 10380
		Zeros,
		// Token: 0x0400288D RID: 10381
		ANSIX923,
		// Token: 0x0400288E RID: 10382
		ISO10126
	}
}
