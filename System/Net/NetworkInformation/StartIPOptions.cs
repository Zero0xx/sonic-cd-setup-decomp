using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060F RID: 1551
	[Flags]
	internal enum StartIPOptions
	{
		// Token: 0x04002DBA RID: 11706
		Both = 3,
		// Token: 0x04002DBB RID: 11707
		None = 0,
		// Token: 0x04002DBC RID: 11708
		StartIPv4 = 1,
		// Token: 0x04002DBD RID: 11709
		StartIPv6 = 2
	}
}
