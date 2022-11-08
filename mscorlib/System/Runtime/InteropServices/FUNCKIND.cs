using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200055D RID: 1373
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum FUNCKIND
	{
		// Token: 0x04001ACE RID: 6862
		FUNC_VIRTUAL,
		// Token: 0x04001ACF RID: 6863
		FUNC_PUREVIRTUAL,
		// Token: 0x04001AD0 RID: 6864
		FUNC_NONVIRTUAL,
		// Token: 0x04001AD1 RID: 6865
		FUNC_STATIC,
		// Token: 0x04001AD2 RID: 6866
		FUNC_DISPATCH
	}
}
