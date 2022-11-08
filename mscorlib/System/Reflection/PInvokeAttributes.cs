using System;

namespace System.Reflection
{
	// Token: 0x02000319 RID: 793
	[Flags]
	[Serializable]
	internal enum PInvokeAttributes
	{
		// Token: 0x04000BB7 RID: 2999
		NoMangle = 1,
		// Token: 0x04000BB8 RID: 3000
		CharSetMask = 6,
		// Token: 0x04000BB9 RID: 3001
		CharSetNotSpec = 0,
		// Token: 0x04000BBA RID: 3002
		CharSetAnsi = 2,
		// Token: 0x04000BBB RID: 3003
		CharSetUnicode = 4,
		// Token: 0x04000BBC RID: 3004
		CharSetAuto = 6,
		// Token: 0x04000BBD RID: 3005
		BestFitUseAssem = 0,
		// Token: 0x04000BBE RID: 3006
		BestFitEnabled = 16,
		// Token: 0x04000BBF RID: 3007
		BestFitDisabled = 32,
		// Token: 0x04000BC0 RID: 3008
		BestFitMask = 48,
		// Token: 0x04000BC1 RID: 3009
		ThrowOnUnmappableCharUseAssem = 0,
		// Token: 0x04000BC2 RID: 3010
		ThrowOnUnmappableCharEnabled = 4096,
		// Token: 0x04000BC3 RID: 3011
		ThrowOnUnmappableCharDisabled = 8192,
		// Token: 0x04000BC4 RID: 3012
		ThrowOnUnmappableCharMask = 12288,
		// Token: 0x04000BC5 RID: 3013
		SupportsLastError = 64,
		// Token: 0x04000BC6 RID: 3014
		CallConvMask = 1792,
		// Token: 0x04000BC7 RID: 3015
		CallConvWinapi = 256,
		// Token: 0x04000BC8 RID: 3016
		CallConvCdecl = 512,
		// Token: 0x04000BC9 RID: 3017
		CallConvStdcall = 768,
		// Token: 0x04000BCA RID: 3018
		CallConvThiscall = 1024,
		// Token: 0x04000BCB RID: 3019
		CallConvFastcall = 1280,
		// Token: 0x04000BCC RID: 3020
		MaxValue = 65535
	}
}
