using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005ED RID: 1517
	[Flags]
	internal enum AdapterFlags
	{
		// Token: 0x04002CC8 RID: 11464
		DnsEnabled = 1,
		// Token: 0x04002CC9 RID: 11465
		RegisterAdapterSuffix = 2,
		// Token: 0x04002CCA RID: 11466
		DhcpEnabled = 4,
		// Token: 0x04002CCB RID: 11467
		ReceiveOnly = 8,
		// Token: 0x04002CCC RID: 11468
		NoMulticast = 16,
		// Token: 0x04002CCD RID: 11469
		Ipv6OtherStatefulConfig = 32
	}
}
