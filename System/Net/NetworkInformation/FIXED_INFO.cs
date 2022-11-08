using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F4 RID: 1524
	internal struct FIXED_INFO
	{
		// Token: 0x04002CE8 RID: 11496
		internal const int MAX_HOSTNAME_LEN = 128;

		// Token: 0x04002CE9 RID: 11497
		internal const int MAX_DOMAIN_NAME_LEN = 128;

		// Token: 0x04002CEA RID: 11498
		internal const int MAX_SCOPE_ID_LEN = 256;

		// Token: 0x04002CEB RID: 11499
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		internal string hostName;

		// Token: 0x04002CEC RID: 11500
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		internal string domainName;

		// Token: 0x04002CED RID: 11501
		internal uint currentDnsServer;

		// Token: 0x04002CEE RID: 11502
		internal IpAddrString DnsServerList;

		// Token: 0x04002CEF RID: 11503
		internal NetBiosNodeType nodeType;

		// Token: 0x04002CF0 RID: 11504
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		internal string scopeId;

		// Token: 0x04002CF1 RID: 11505
		internal bool enableRouting;

		// Token: 0x04002CF2 RID: 11506
		internal bool enableProxy;

		// Token: 0x04002CF3 RID: 11507
		internal bool enableDns;
	}
}
