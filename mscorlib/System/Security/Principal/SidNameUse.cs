using System;

namespace System.Security.Principal
{
	// Token: 0x02000914 RID: 2324
	internal enum SidNameUse
	{
		// Token: 0x04002B9B RID: 11163
		User = 1,
		// Token: 0x04002B9C RID: 11164
		Group,
		// Token: 0x04002B9D RID: 11165
		Domain,
		// Token: 0x04002B9E RID: 11166
		Alias,
		// Token: 0x04002B9F RID: 11167
		WellKnownGroup,
		// Token: 0x04002BA0 RID: 11168
		DeletedAccount,
		// Token: 0x04002BA1 RID: 11169
		Invalid,
		// Token: 0x04002BA2 RID: 11170
		Unknown,
		// Token: 0x04002BA3 RID: 11171
		Computer
	}
}
