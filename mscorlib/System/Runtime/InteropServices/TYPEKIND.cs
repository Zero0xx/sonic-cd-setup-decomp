using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054D RID: 1357
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum TYPEKIND
	{
		// Token: 0x04001A64 RID: 6756
		TKIND_ENUM,
		// Token: 0x04001A65 RID: 6757
		TKIND_RECORD,
		// Token: 0x04001A66 RID: 6758
		TKIND_MODULE,
		// Token: 0x04001A67 RID: 6759
		TKIND_INTERFACE,
		// Token: 0x04001A68 RID: 6760
		TKIND_DISPATCH,
		// Token: 0x04001A69 RID: 6761
		TKIND_COCLASS,
		// Token: 0x04001A6A RID: 6762
		TKIND_ALIAS,
		// Token: 0x04001A6B RID: 6763
		TKIND_UNION,
		// Token: 0x04001A6C RID: 6764
		TKIND_MAX
	}
}
