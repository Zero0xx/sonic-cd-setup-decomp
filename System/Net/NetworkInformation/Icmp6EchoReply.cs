using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060B RID: 1547
	internal struct Icmp6EchoReply
	{
		// Token: 0x04002DB2 RID: 11698
		internal Ipv6Address Address;

		// Token: 0x04002DB3 RID: 11699
		internal uint Status;

		// Token: 0x04002DB4 RID: 11700
		internal uint RoundTripTime;

		// Token: 0x04002DB5 RID: 11701
		internal IntPtr data;
	}
}
