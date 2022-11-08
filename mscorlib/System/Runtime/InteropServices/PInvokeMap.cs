using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000518 RID: 1304
	[Serializable]
	internal enum PInvokeMap
	{
		// Token: 0x040019DD RID: 6621
		NoMangle = 1,
		// Token: 0x040019DE RID: 6622
		CharSetMask = 6,
		// Token: 0x040019DF RID: 6623
		CharSetNotSpec = 0,
		// Token: 0x040019E0 RID: 6624
		CharSetAnsi = 2,
		// Token: 0x040019E1 RID: 6625
		CharSetUnicode = 4,
		// Token: 0x040019E2 RID: 6626
		CharSetAuto = 6,
		// Token: 0x040019E3 RID: 6627
		PinvokeOLE = 32,
		// Token: 0x040019E4 RID: 6628
		SupportsLastError = 64,
		// Token: 0x040019E5 RID: 6629
		BestFitMask = 48,
		// Token: 0x040019E6 RID: 6630
		BestFitEnabled = 16,
		// Token: 0x040019E7 RID: 6631
		BestFitDisabled = 32,
		// Token: 0x040019E8 RID: 6632
		BestFitUseAsm = 48,
		// Token: 0x040019E9 RID: 6633
		ThrowOnUnmappableCharMask = 12288,
		// Token: 0x040019EA RID: 6634
		ThrowOnUnmappableCharEnabled = 4096,
		// Token: 0x040019EB RID: 6635
		ThrowOnUnmappableCharDisabled = 8192,
		// Token: 0x040019EC RID: 6636
		ThrowOnUnmappableCharUseAsm = 12288,
		// Token: 0x040019ED RID: 6637
		CallConvMask = 1792,
		// Token: 0x040019EE RID: 6638
		CallConvWinapi = 256,
		// Token: 0x040019EF RID: 6639
		CallConvCdecl = 512,
		// Token: 0x040019F0 RID: 6640
		CallConvStdcall = 768,
		// Token: 0x040019F1 RID: 6641
		CallConvThiscall = 1024,
		// Token: 0x040019F2 RID: 6642
		CallConvFastcall = 1280
	}
}
