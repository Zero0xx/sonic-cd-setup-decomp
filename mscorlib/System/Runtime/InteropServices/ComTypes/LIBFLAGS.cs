using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000598 RID: 1432
	[Flags]
	[Serializable]
	public enum LIBFLAGS : short
	{
		// Token: 0x04001BD6 RID: 7126
		LIBFLAG_FRESTRICTED = 1,
		// Token: 0x04001BD7 RID: 7127
		LIBFLAG_FCONTROL = 2,
		// Token: 0x04001BD8 RID: 7128
		LIBFLAG_FHIDDEN = 4,
		// Token: 0x04001BD9 RID: 7129
		LIBFLAG_FHASDISKIMAGE = 8
	}
}
