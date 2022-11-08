using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000587 RID: 1415
	[Flags]
	[Serializable]
	public enum PARAMFLAG : short
	{
		// Token: 0x04001B74 RID: 7028
		PARAMFLAG_NONE = 0,
		// Token: 0x04001B75 RID: 7029
		PARAMFLAG_FIN = 1,
		// Token: 0x04001B76 RID: 7030
		PARAMFLAG_FOUT = 2,
		// Token: 0x04001B77 RID: 7031
		PARAMFLAG_FLCID = 4,
		// Token: 0x04001B78 RID: 7032
		PARAMFLAG_FRETVAL = 8,
		// Token: 0x04001B79 RID: 7033
		PARAMFLAG_FOPT = 16,
		// Token: 0x04001B7A RID: 7034
		PARAMFLAG_FHASDEFAULT = 32,
		// Token: 0x04001B7B RID: 7035
		PARAMFLAG_FHASCUSTDATA = 64
	}
}
