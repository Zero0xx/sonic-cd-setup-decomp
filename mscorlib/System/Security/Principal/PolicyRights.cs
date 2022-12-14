using System;

namespace System.Security.Principal
{
	// Token: 0x02000917 RID: 2327
	[Flags]
	internal enum PolicyRights
	{
		// Token: 0x04002BEE RID: 11246
		POLICY_VIEW_LOCAL_INFORMATION = 1,
		// Token: 0x04002BEF RID: 11247
		POLICY_VIEW_AUDIT_INFORMATION = 2,
		// Token: 0x04002BF0 RID: 11248
		POLICY_GET_PRIVATE_INFORMATION = 4,
		// Token: 0x04002BF1 RID: 11249
		POLICY_TRUST_ADMIN = 8,
		// Token: 0x04002BF2 RID: 11250
		POLICY_CREATE_ACCOUNT = 16,
		// Token: 0x04002BF3 RID: 11251
		POLICY_CREATE_SECRET = 32,
		// Token: 0x04002BF4 RID: 11252
		POLICY_CREATE_PRIVILEGE = 64,
		// Token: 0x04002BF5 RID: 11253
		POLICY_SET_DEFAULT_QUOTA_LIMITS = 128,
		// Token: 0x04002BF6 RID: 11254
		POLICY_SET_AUDIT_REQUIREMENTS = 256,
		// Token: 0x04002BF7 RID: 11255
		POLICY_AUDIT_LOG_ADMIN = 512,
		// Token: 0x04002BF8 RID: 11256
		POLICY_SERVER_ADMIN = 1024,
		// Token: 0x04002BF9 RID: 11257
		POLICY_LOOKUP_NAMES = 2048,
		// Token: 0x04002BFA RID: 11258
		POLICY_NOTIFICATION = 4096
	}
}
