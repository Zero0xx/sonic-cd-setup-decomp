using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FF RID: 1535
	internal struct MibIpStats
	{
		// Token: 0x04002D66 RID: 11622
		internal bool forwardingEnabled;

		// Token: 0x04002D67 RID: 11623
		internal uint defaultTtl;

		// Token: 0x04002D68 RID: 11624
		internal uint packetsReceived;

		// Token: 0x04002D69 RID: 11625
		internal uint receivedPacketsWithHeaderErrors;

		// Token: 0x04002D6A RID: 11626
		internal uint receivedPacketsWithAddressErrors;

		// Token: 0x04002D6B RID: 11627
		internal uint packetsForwarded;

		// Token: 0x04002D6C RID: 11628
		internal uint receivedPacketsWithUnknownProtocols;

		// Token: 0x04002D6D RID: 11629
		internal uint receivedPacketsDiscarded;

		// Token: 0x04002D6E RID: 11630
		internal uint receivedPacketsDelivered;

		// Token: 0x04002D6F RID: 11631
		internal uint packetOutputRequests;

		// Token: 0x04002D70 RID: 11632
		internal uint outputPacketRoutingDiscards;

		// Token: 0x04002D71 RID: 11633
		internal uint outputPacketsDiscarded;

		// Token: 0x04002D72 RID: 11634
		internal uint outputPacketsWithNoRoute;

		// Token: 0x04002D73 RID: 11635
		internal uint packetReassemblyTimeout;

		// Token: 0x04002D74 RID: 11636
		internal uint packetsReassemblyRequired;

		// Token: 0x04002D75 RID: 11637
		internal uint packetsReassembled;

		// Token: 0x04002D76 RID: 11638
		internal uint packetsReassemblyFailed;

		// Token: 0x04002D77 RID: 11639
		internal uint packetsFragmented;

		// Token: 0x04002D78 RID: 11640
		internal uint packetsFragmentFailed;

		// Token: 0x04002D79 RID: 11641
		internal uint packetsFragmentCreated;

		// Token: 0x04002D7A RID: 11642
		internal uint interfaces;

		// Token: 0x04002D7B RID: 11643
		internal uint ipAddresses;

		// Token: 0x04002D7C RID: 11644
		internal uint routes;
	}
}
