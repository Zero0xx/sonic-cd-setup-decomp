using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054F RID: 1359
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum IMPLTYPEFLAGS
	{
		// Token: 0x04001A7E RID: 6782
		IMPLTYPEFLAG_FDEFAULT = 1,
		// Token: 0x04001A7F RID: 6783
		IMPLTYPEFLAG_FSOURCE = 2,
		// Token: 0x04001A80 RID: 6784
		IMPLTYPEFLAG_FRESTRICTED = 4,
		// Token: 0x04001A81 RID: 6785
		IMPLTYPEFLAG_FDEFAULTVTABLE = 8
	}
}
