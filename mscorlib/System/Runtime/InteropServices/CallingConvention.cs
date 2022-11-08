using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000509 RID: 1289
	[ComVisible(true)]
	[Serializable]
	public enum CallingConvention
	{
		// Token: 0x040019B2 RID: 6578
		Winapi = 1,
		// Token: 0x040019B3 RID: 6579
		Cdecl,
		// Token: 0x040019B4 RID: 6580
		StdCall,
		// Token: 0x040019B5 RID: 6581
		ThisCall,
		// Token: 0x040019B6 RID: 6582
		FastCall
	}
}
