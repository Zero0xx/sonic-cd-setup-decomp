using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F0 RID: 1520
	[Flags]
	internal enum GetAdaptersAddressesFlags
	{
		// Token: 0x04002CD9 RID: 11481
		SkipUnicast = 1,
		// Token: 0x04002CDA RID: 11482
		SkipAnycast = 2,
		// Token: 0x04002CDB RID: 11483
		SkipMulticast = 4,
		// Token: 0x04002CDC RID: 11484
		SkipDnsServer = 8,
		// Token: 0x04002CDD RID: 11485
		IncludePrefix = 16,
		// Token: 0x04002CDE RID: 11486
		SkipFriendlyName = 32
	}
}
