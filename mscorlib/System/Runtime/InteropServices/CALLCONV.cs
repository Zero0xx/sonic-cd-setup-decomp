using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200055F RID: 1375
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.CALLCONV instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum CALLCONV
	{
		// Token: 0x04001AD9 RID: 6873
		CC_CDECL = 1,
		// Token: 0x04001ADA RID: 6874
		CC_MSCPASCAL,
		// Token: 0x04001ADB RID: 6875
		CC_PASCAL = 2,
		// Token: 0x04001ADC RID: 6876
		CC_MACPASCAL,
		// Token: 0x04001ADD RID: 6877
		CC_STDCALL,
		// Token: 0x04001ADE RID: 6878
		CC_RESERVED,
		// Token: 0x04001ADF RID: 6879
		CC_SYSCALL,
		// Token: 0x04001AE0 RID: 6880
		CC_MPWCDECL,
		// Token: 0x04001AE1 RID: 6881
		CC_MPWPASCAL,
		// Token: 0x04001AE2 RID: 6882
		CC_MAX
	}
}
