using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000343 RID: 835
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TypeAttributes
	{
		// Token: 0x04000DC3 RID: 3523
		VisibilityMask = 7,
		// Token: 0x04000DC4 RID: 3524
		NotPublic = 0,
		// Token: 0x04000DC5 RID: 3525
		Public = 1,
		// Token: 0x04000DC6 RID: 3526
		NestedPublic = 2,
		// Token: 0x04000DC7 RID: 3527
		NestedPrivate = 3,
		// Token: 0x04000DC8 RID: 3528
		NestedFamily = 4,
		// Token: 0x04000DC9 RID: 3529
		NestedAssembly = 5,
		// Token: 0x04000DCA RID: 3530
		NestedFamANDAssem = 6,
		// Token: 0x04000DCB RID: 3531
		NestedFamORAssem = 7,
		// Token: 0x04000DCC RID: 3532
		LayoutMask = 24,
		// Token: 0x04000DCD RID: 3533
		AutoLayout = 0,
		// Token: 0x04000DCE RID: 3534
		SequentialLayout = 8,
		// Token: 0x04000DCF RID: 3535
		ExplicitLayout = 16,
		// Token: 0x04000DD0 RID: 3536
		ClassSemanticsMask = 32,
		// Token: 0x04000DD1 RID: 3537
		Class = 0,
		// Token: 0x04000DD2 RID: 3538
		Interface = 32,
		// Token: 0x04000DD3 RID: 3539
		Abstract = 128,
		// Token: 0x04000DD4 RID: 3540
		Sealed = 256,
		// Token: 0x04000DD5 RID: 3541
		SpecialName = 1024,
		// Token: 0x04000DD6 RID: 3542
		Import = 4096,
		// Token: 0x04000DD7 RID: 3543
		Serializable = 8192,
		// Token: 0x04000DD8 RID: 3544
		StringFormatMask = 196608,
		// Token: 0x04000DD9 RID: 3545
		AnsiClass = 0,
		// Token: 0x04000DDA RID: 3546
		UnicodeClass = 65536,
		// Token: 0x04000DDB RID: 3547
		AutoClass = 131072,
		// Token: 0x04000DDC RID: 3548
		CustomFormatClass = 196608,
		// Token: 0x04000DDD RID: 3549
		CustomFormatMask = 12582912,
		// Token: 0x04000DDE RID: 3550
		BeforeFieldInit = 1048576,
		// Token: 0x04000DDF RID: 3551
		ReservedMask = 264192,
		// Token: 0x04000DE0 RID: 3552
		RTSpecialName = 2048,
		// Token: 0x04000DE1 RID: 3553
		HasSecurity = 262144
	}
}
