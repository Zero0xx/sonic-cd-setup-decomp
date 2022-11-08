using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000342 RID: 834
	[Flags]
	public enum OpenFlags
	{
		// Token: 0x04001B25 RID: 6949
		ReadOnly = 0,
		// Token: 0x04001B26 RID: 6950
		ReadWrite = 1,
		// Token: 0x04001B27 RID: 6951
		MaxAllowed = 2,
		// Token: 0x04001B28 RID: 6952
		OpenExistingOnly = 4,
		// Token: 0x04001B29 RID: 6953
		IncludeArchived = 8
	}
}
