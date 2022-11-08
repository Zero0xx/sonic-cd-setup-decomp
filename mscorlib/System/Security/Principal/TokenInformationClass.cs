using System;

namespace System.Security.Principal
{
	// Token: 0x020004D1 RID: 1233
	[Serializable]
	internal enum TokenInformationClass
	{
		// Token: 0x040018BE RID: 6334
		TokenUser = 1,
		// Token: 0x040018BF RID: 6335
		TokenGroups,
		// Token: 0x040018C0 RID: 6336
		TokenPrivileges,
		// Token: 0x040018C1 RID: 6337
		TokenOwner,
		// Token: 0x040018C2 RID: 6338
		TokenPrimaryGroup,
		// Token: 0x040018C3 RID: 6339
		TokenDefaultDacl,
		// Token: 0x040018C4 RID: 6340
		TokenSource,
		// Token: 0x040018C5 RID: 6341
		TokenType,
		// Token: 0x040018C6 RID: 6342
		TokenImpersonationLevel,
		// Token: 0x040018C7 RID: 6343
		TokenStatistics,
		// Token: 0x040018C8 RID: 6344
		TokenRestrictedSids,
		// Token: 0x040018C9 RID: 6345
		TokenSessionId,
		// Token: 0x040018CA RID: 6346
		TokenGroupsAndPrivileges,
		// Token: 0x040018CB RID: 6347
		TokenSessionReference,
		// Token: 0x040018CC RID: 6348
		TokenSandBoxInert
	}
}
