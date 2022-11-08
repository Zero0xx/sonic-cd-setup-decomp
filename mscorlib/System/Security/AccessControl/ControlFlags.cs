using System;

namespace System.Security.AccessControl
{
	// Token: 0x0200090A RID: 2314
	[Flags]
	public enum ControlFlags
	{
		// Token: 0x04002B66 RID: 11110
		None = 0,
		// Token: 0x04002B67 RID: 11111
		OwnerDefaulted = 1,
		// Token: 0x04002B68 RID: 11112
		GroupDefaulted = 2,
		// Token: 0x04002B69 RID: 11113
		DiscretionaryAclPresent = 4,
		// Token: 0x04002B6A RID: 11114
		DiscretionaryAclDefaulted = 8,
		// Token: 0x04002B6B RID: 11115
		SystemAclPresent = 16,
		// Token: 0x04002B6C RID: 11116
		SystemAclDefaulted = 32,
		// Token: 0x04002B6D RID: 11117
		DiscretionaryAclUntrusted = 64,
		// Token: 0x04002B6E RID: 11118
		ServerSecurity = 128,
		// Token: 0x04002B6F RID: 11119
		DiscretionaryAclAutoInheritRequired = 256,
		// Token: 0x04002B70 RID: 11120
		SystemAclAutoInheritRequired = 512,
		// Token: 0x04002B71 RID: 11121
		DiscretionaryAclAutoInherited = 1024,
		// Token: 0x04002B72 RID: 11122
		SystemAclAutoInherited = 2048,
		// Token: 0x04002B73 RID: 11123
		DiscretionaryAclProtected = 4096,
		// Token: 0x04002B74 RID: 11124
		SystemAclProtected = 8192,
		// Token: 0x04002B75 RID: 11125
		RMControlValid = 16384,
		// Token: 0x04002B76 RID: 11126
		SelfRelative = 32768
	}
}
