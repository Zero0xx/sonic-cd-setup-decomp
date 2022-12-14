using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000595 RID: 1429
	[Flags]
	[Serializable]
	public enum VARFLAGS : short
	{
		// Token: 0x04001BC3 RID: 7107
		VARFLAG_FREADONLY = 1,
		// Token: 0x04001BC4 RID: 7108
		VARFLAG_FSOURCE = 2,
		// Token: 0x04001BC5 RID: 7109
		VARFLAG_FBINDABLE = 4,
		// Token: 0x04001BC6 RID: 7110
		VARFLAG_FREQUESTEDIT = 8,
		// Token: 0x04001BC7 RID: 7111
		VARFLAG_FDISPLAYBIND = 16,
		// Token: 0x04001BC8 RID: 7112
		VARFLAG_FDEFAULTBIND = 32,
		// Token: 0x04001BC9 RID: 7113
		VARFLAG_FHIDDEN = 64,
		// Token: 0x04001BCA RID: 7114
		VARFLAG_FRESTRICTED = 128,
		// Token: 0x04001BCB RID: 7115
		VARFLAG_FDEFAULTCOLLELEM = 256,
		// Token: 0x04001BCC RID: 7116
		VARFLAG_FUIDEFAULT = 512,
		// Token: 0x04001BCD RID: 7117
		VARFLAG_FNONBROWSABLE = 1024,
		// Token: 0x04001BCE RID: 7118
		VARFLAG_FREPLACEABLE = 2048,
		// Token: 0x04001BCF RID: 7119
		VARFLAG_FIMMEDIATEBIND = 4096
	}
}
