using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008EB RID: 2283
	[Flags]
	public enum SecurityInfos
	{
		// Token: 0x04002ACF RID: 10959
		Owner = 1,
		// Token: 0x04002AD0 RID: 10960
		Group = 2,
		// Token: 0x04002AD1 RID: 10961
		DiscretionaryAcl = 4,
		// Token: 0x04002AD2 RID: 10962
		SystemAcl = 8
	}
}
