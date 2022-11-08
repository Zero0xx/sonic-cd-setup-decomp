using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054A RID: 1354
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.DESCKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum DESCKIND
	{
		// Token: 0x04001A5A RID: 6746
		DESCKIND_NONE,
		// Token: 0x04001A5B RID: 6747
		DESCKIND_FUNCDESC,
		// Token: 0x04001A5C RID: 6748
		DESCKIND_VARDESC,
		// Token: 0x04001A5D RID: 6749
		DESCKIND_TYPECOMP,
		// Token: 0x04001A5E RID: 6750
		DESCKIND_IMPLICITAPPOBJ,
		// Token: 0x04001A5F RID: 6751
		DESCKIND_MAX
	}
}
