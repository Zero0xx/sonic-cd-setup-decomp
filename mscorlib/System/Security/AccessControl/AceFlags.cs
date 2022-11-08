using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008C9 RID: 2249
	[Flags]
	public enum AceFlags : byte
	{
		// Token: 0x04002A4E RID: 10830
		None = 0,
		// Token: 0x04002A4F RID: 10831
		ObjectInherit = 1,
		// Token: 0x04002A50 RID: 10832
		ContainerInherit = 2,
		// Token: 0x04002A51 RID: 10833
		NoPropagateInherit = 4,
		// Token: 0x04002A52 RID: 10834
		InheritOnly = 8,
		// Token: 0x04002A53 RID: 10835
		Inherited = 16,
		// Token: 0x04002A54 RID: 10836
		SuccessfulAccess = 64,
		// Token: 0x04002A55 RID: 10837
		FailedAccess = 128,
		// Token: 0x04002A56 RID: 10838
		InheritanceFlags = 15,
		// Token: 0x04002A57 RID: 10839
		AuditFlags = 192
	}
}
