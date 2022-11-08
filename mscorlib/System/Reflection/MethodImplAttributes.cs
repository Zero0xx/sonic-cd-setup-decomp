using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200032E RID: 814
	[ComVisible(true)]
	[Serializable]
	public enum MethodImplAttributes
	{
		// Token: 0x04000D5D RID: 3421
		CodeTypeMask = 3,
		// Token: 0x04000D5E RID: 3422
		IL = 0,
		// Token: 0x04000D5F RID: 3423
		Native,
		// Token: 0x04000D60 RID: 3424
		OPTIL,
		// Token: 0x04000D61 RID: 3425
		Runtime,
		// Token: 0x04000D62 RID: 3426
		ManagedMask,
		// Token: 0x04000D63 RID: 3427
		Unmanaged = 4,
		// Token: 0x04000D64 RID: 3428
		Managed = 0,
		// Token: 0x04000D65 RID: 3429
		ForwardRef = 16,
		// Token: 0x04000D66 RID: 3430
		PreserveSig = 128,
		// Token: 0x04000D67 RID: 3431
		InternalCall = 4096,
		// Token: 0x04000D68 RID: 3432
		Synchronized = 32,
		// Token: 0x04000D69 RID: 3433
		NoInlining = 8,
		// Token: 0x04000D6A RID: 3434
		NoOptimization = 64,
		// Token: 0x04000D6B RID: 3435
		MaxMethodImplVal = 65535
	}
}
