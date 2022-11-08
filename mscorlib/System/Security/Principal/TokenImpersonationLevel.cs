using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004CA RID: 1226
	[ComVisible(true)]
	[Serializable]
	public enum TokenImpersonationLevel
	{
		// Token: 0x0400188B RID: 6283
		None,
		// Token: 0x0400188C RID: 6284
		Anonymous,
		// Token: 0x0400188D RID: 6285
		Identification,
		// Token: 0x0400188E RID: 6286
		Impersonation,
		// Token: 0x0400188F RID: 6287
		Delegation
	}
}
