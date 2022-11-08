using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200032D RID: 813
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum MethodAttributes
	{
		// Token: 0x04000D44 RID: 3396
		MemberAccessMask = 7,
		// Token: 0x04000D45 RID: 3397
		PrivateScope = 0,
		// Token: 0x04000D46 RID: 3398
		Private = 1,
		// Token: 0x04000D47 RID: 3399
		FamANDAssem = 2,
		// Token: 0x04000D48 RID: 3400
		Assembly = 3,
		// Token: 0x04000D49 RID: 3401
		Family = 4,
		// Token: 0x04000D4A RID: 3402
		FamORAssem = 5,
		// Token: 0x04000D4B RID: 3403
		Public = 6,
		// Token: 0x04000D4C RID: 3404
		Static = 16,
		// Token: 0x04000D4D RID: 3405
		Final = 32,
		// Token: 0x04000D4E RID: 3406
		Virtual = 64,
		// Token: 0x04000D4F RID: 3407
		HideBySig = 128,
		// Token: 0x04000D50 RID: 3408
		CheckAccessOnOverride = 512,
		// Token: 0x04000D51 RID: 3409
		VtableLayoutMask = 256,
		// Token: 0x04000D52 RID: 3410
		ReuseSlot = 0,
		// Token: 0x04000D53 RID: 3411
		NewSlot = 256,
		// Token: 0x04000D54 RID: 3412
		Abstract = 1024,
		// Token: 0x04000D55 RID: 3413
		SpecialName = 2048,
		// Token: 0x04000D56 RID: 3414
		PinvokeImpl = 8192,
		// Token: 0x04000D57 RID: 3415
		UnmanagedExport = 8,
		// Token: 0x04000D58 RID: 3416
		RTSpecialName = 4096,
		// Token: 0x04000D59 RID: 3417
		ReservedMask = 53248,
		// Token: 0x04000D5A RID: 3418
		HasSecurity = 16384,
		// Token: 0x04000D5B RID: 3419
		RequireSecObject = 32768
	}
}
