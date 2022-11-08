using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000561 RID: 1377
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.VARFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum VARFLAGS : short
	{
		// Token: 0x04001AF2 RID: 6898
		VARFLAG_FREADONLY = 1,
		// Token: 0x04001AF3 RID: 6899
		VARFLAG_FSOURCE = 2,
		// Token: 0x04001AF4 RID: 6900
		VARFLAG_FBINDABLE = 4,
		// Token: 0x04001AF5 RID: 6901
		VARFLAG_FREQUESTEDIT = 8,
		// Token: 0x04001AF6 RID: 6902
		VARFLAG_FDISPLAYBIND = 16,
		// Token: 0x04001AF7 RID: 6903
		VARFLAG_FDEFAULTBIND = 32,
		// Token: 0x04001AF8 RID: 6904
		VARFLAG_FHIDDEN = 64,
		// Token: 0x04001AF9 RID: 6905
		VARFLAG_FRESTRICTED = 128,
		// Token: 0x04001AFA RID: 6906
		VARFLAG_FDEFAULTCOLLELEM = 256,
		// Token: 0x04001AFB RID: 6907
		VARFLAG_FUIDEFAULT = 512,
		// Token: 0x04001AFC RID: 6908
		VARFLAG_FNONBROWSABLE = 1024,
		// Token: 0x04001AFD RID: 6909
		VARFLAG_FREPLACEABLE = 2048,
		// Token: 0x04001AFE RID: 6910
		VARFLAG_FIMMEDIATEBIND = 4096
	}
}
