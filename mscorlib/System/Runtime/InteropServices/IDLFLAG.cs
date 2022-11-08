using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000552 RID: 1362
	[Flags]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IDLFLAG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum IDLFLAG : short
	{
		// Token: 0x04001AA2 RID: 6818
		IDLFLAG_NONE = 0,
		// Token: 0x04001AA3 RID: 6819
		IDLFLAG_FIN = 1,
		// Token: 0x04001AA4 RID: 6820
		IDLFLAG_FOUT = 2,
		// Token: 0x04001AA5 RID: 6821
		IDLFLAG_FLCID = 4,
		// Token: 0x04001AA6 RID: 6822
		IDLFLAG_FRETVAL = 8
	}
}
