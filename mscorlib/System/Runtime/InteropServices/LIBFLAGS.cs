using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000564 RID: 1380
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.LIBFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum LIBFLAGS : short
	{
		// Token: 0x04001B04 RID: 6916
		LIBFLAG_FRESTRICTED = 1,
		// Token: 0x04001B05 RID: 6917
		LIBFLAG_FCONTROL = 2,
		// Token: 0x04001B06 RID: 6918
		LIBFLAG_FHIDDEN = 4,
		// Token: 0x04001B07 RID: 6919
		LIBFLAG_FHASDISKIMAGE = 8
	}
}
