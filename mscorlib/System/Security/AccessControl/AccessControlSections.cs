using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008ED RID: 2285
	[Flags]
	public enum AccessControlSections
	{
		// Token: 0x04002AE2 RID: 10978
		None = 0,
		// Token: 0x04002AE3 RID: 10979
		Audit = 1,
		// Token: 0x04002AE4 RID: 10980
		Access = 2,
		// Token: 0x04002AE5 RID: 10981
		Owner = 4,
		// Token: 0x04002AE6 RID: 10982
		Group = 8,
		// Token: 0x04002AE7 RID: 10983
		All = 15
	}
}
