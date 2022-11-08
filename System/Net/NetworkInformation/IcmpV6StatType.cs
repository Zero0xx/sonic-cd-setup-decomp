using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062A RID: 1578
	internal enum IcmpV6StatType
	{
		// Token: 0x04002E2C RID: 11820
		DestinationUnreachable = 1,
		// Token: 0x04002E2D RID: 11821
		PacketTooBig,
		// Token: 0x04002E2E RID: 11822
		TimeExceeded,
		// Token: 0x04002E2F RID: 11823
		ParameterProblem,
		// Token: 0x04002E30 RID: 11824
		EchoRequest = 128,
		// Token: 0x04002E31 RID: 11825
		EchoReply,
		// Token: 0x04002E32 RID: 11826
		MembershipQuery,
		// Token: 0x04002E33 RID: 11827
		MembershipReport,
		// Token: 0x04002E34 RID: 11828
		MembershipReduction,
		// Token: 0x04002E35 RID: 11829
		RouterSolicit,
		// Token: 0x04002E36 RID: 11830
		RouterAdvertisement,
		// Token: 0x04002E37 RID: 11831
		NeighborSolict,
		// Token: 0x04002E38 RID: 11832
		NeighborAdvertisement,
		// Token: 0x04002E39 RID: 11833
		Redirect
	}
}
