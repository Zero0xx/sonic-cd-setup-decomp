using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200002A RID: 42
	[Flags]
	public enum LockFlags
	{
		// Token: 0x04000CF5 RID: 3317
		Discard = 8192,
		// Token: 0x04000CF6 RID: 3318
		DoNotWait = 16384,
		// Token: 0x04000CF7 RID: 3319
		NoDirtyUpdate = 32768,
		// Token: 0x04000CF8 RID: 3320
		NoSystemLock = 2048,
		// Token: 0x04000CF9 RID: 3321
		NoOverwrite = 4096,
		// Token: 0x04000CFA RID: 3322
		ReadOnly = 16,
		// Token: 0x04000CFB RID: 3323
		None = 0
	}
}
