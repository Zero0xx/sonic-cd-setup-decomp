using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200084A RID: 2122
	[ComVisible(true)]
	[Serializable]
	public enum PackingSize
	{
		// Token: 0x0400281E RID: 10270
		Unspecified,
		// Token: 0x0400281F RID: 10271
		Size1,
		// Token: 0x04002820 RID: 10272
		Size2,
		// Token: 0x04002821 RID: 10273
		Size4 = 4,
		// Token: 0x04002822 RID: 10274
		Size8 = 8,
		// Token: 0x04002823 RID: 10275
		Size16 = 16,
		// Token: 0x04002824 RID: 10276
		Size32 = 32,
		// Token: 0x04002825 RID: 10277
		Size64 = 64,
		// Token: 0x04002826 RID: 10278
		Size128 = 128
	}
}
