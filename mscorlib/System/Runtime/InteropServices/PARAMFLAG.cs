using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000554 RID: 1364
	[Flags]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.PARAMFLAG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum PARAMFLAG : short
	{
		// Token: 0x04001AAA RID: 6826
		PARAMFLAG_NONE = 0,
		// Token: 0x04001AAB RID: 6827
		PARAMFLAG_FIN = 1,
		// Token: 0x04001AAC RID: 6828
		PARAMFLAG_FOUT = 2,
		// Token: 0x04001AAD RID: 6829
		PARAMFLAG_FLCID = 4,
		// Token: 0x04001AAE RID: 6830
		PARAMFLAG_FRETVAL = 8,
		// Token: 0x04001AAF RID: 6831
		PARAMFLAG_FOPT = 16,
		// Token: 0x04001AB0 RID: 6832
		PARAMFLAG_FHASDEFAULT = 32,
		// Token: 0x04001AB1 RID: 6833
		PARAMFLAG_FHASCUSTDATA = 64
	}
}
