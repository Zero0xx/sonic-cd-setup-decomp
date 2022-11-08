using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FB RID: 1531
	internal struct IpPerAdapterInfo
	{
		// Token: 0x04002D33 RID: 11571
		internal bool autoconfigEnabled;

		// Token: 0x04002D34 RID: 11572
		internal bool autoconfigActive;

		// Token: 0x04002D35 RID: 11573
		internal IntPtr currentDnsServer;

		// Token: 0x04002D36 RID: 11574
		internal IpAddrString dnsServerList;
	}
}
