using System;

namespace System.Net.Sockets
{
	// Token: 0x020005AD RID: 1453
	public enum ProtocolType
	{
		// Token: 0x04002AEA RID: 10986
		IP,
		// Token: 0x04002AEB RID: 10987
		IPv6HopByHopOptions = 0,
		// Token: 0x04002AEC RID: 10988
		Icmp,
		// Token: 0x04002AED RID: 10989
		Igmp,
		// Token: 0x04002AEE RID: 10990
		Ggp,
		// Token: 0x04002AEF RID: 10991
		IPv4,
		// Token: 0x04002AF0 RID: 10992
		Tcp = 6,
		// Token: 0x04002AF1 RID: 10993
		Pup = 12,
		// Token: 0x04002AF2 RID: 10994
		Udp = 17,
		// Token: 0x04002AF3 RID: 10995
		Idp = 22,
		// Token: 0x04002AF4 RID: 10996
		IPv6 = 41,
		// Token: 0x04002AF5 RID: 10997
		IPv6RoutingHeader = 43,
		// Token: 0x04002AF6 RID: 10998
		IPv6FragmentHeader,
		// Token: 0x04002AF7 RID: 10999
		IPSecEncapsulatingSecurityPayload = 50,
		// Token: 0x04002AF8 RID: 11000
		IPSecAuthenticationHeader,
		// Token: 0x04002AF9 RID: 11001
		IcmpV6 = 58,
		// Token: 0x04002AFA RID: 11002
		IPv6NoNextHeader,
		// Token: 0x04002AFB RID: 11003
		IPv6DestinationOptions,
		// Token: 0x04002AFC RID: 11004
		ND = 77,
		// Token: 0x04002AFD RID: 11005
		Raw = 255,
		// Token: 0x04002AFE RID: 11006
		Unspecified = 0,
		// Token: 0x04002AFF RID: 11007
		Ipx = 1000,
		// Token: 0x04002B00 RID: 11008
		Spx = 1256,
		// Token: 0x04002B01 RID: 11009
		SpxII,
		// Token: 0x04002B02 RID: 11010
		Unknown = -1
	}
}
