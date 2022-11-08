using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000902 RID: 2306
	[Flags]
	public enum RegistryRights
	{
		// Token: 0x04002B4E RID: 11086
		QueryValues = 1,
		// Token: 0x04002B4F RID: 11087
		SetValue = 2,
		// Token: 0x04002B50 RID: 11088
		CreateSubKey = 4,
		// Token: 0x04002B51 RID: 11089
		EnumerateSubKeys = 8,
		// Token: 0x04002B52 RID: 11090
		Notify = 16,
		// Token: 0x04002B53 RID: 11091
		CreateLink = 32,
		// Token: 0x04002B54 RID: 11092
		ExecuteKey = 131097,
		// Token: 0x04002B55 RID: 11093
		ReadKey = 131097,
		// Token: 0x04002B56 RID: 11094
		WriteKey = 131078,
		// Token: 0x04002B57 RID: 11095
		Delete = 65536,
		// Token: 0x04002B58 RID: 11096
		ReadPermissions = 131072,
		// Token: 0x04002B59 RID: 11097
		ChangePermissions = 262144,
		// Token: 0x04002B5A RID: 11098
		TakeOwnership = 524288,
		// Token: 0x04002B5B RID: 11099
		FullControl = 983103
	}
}
