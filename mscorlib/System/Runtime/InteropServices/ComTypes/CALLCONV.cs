using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000593 RID: 1427
	[Serializable]
	public enum CALLCONV
	{
		// Token: 0x04001BAA RID: 7082
		CC_CDECL = 1,
		// Token: 0x04001BAB RID: 7083
		CC_MSCPASCAL,
		// Token: 0x04001BAC RID: 7084
		CC_PASCAL = 2,
		// Token: 0x04001BAD RID: 7085
		CC_MACPASCAL,
		// Token: 0x04001BAE RID: 7086
		CC_STDCALL,
		// Token: 0x04001BAF RID: 7087
		CC_RESERVED,
		// Token: 0x04001BB0 RID: 7088
		CC_SYSCALL,
		// Token: 0x04001BB1 RID: 7089
		CC_MPWCDECL,
		// Token: 0x04001BB2 RID: 7090
		CC_MPWPASCAL,
		// Token: 0x04001BB3 RID: 7091
		CC_MAX
	}
}
