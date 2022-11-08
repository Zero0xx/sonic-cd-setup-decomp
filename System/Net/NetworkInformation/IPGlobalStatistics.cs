using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005DB RID: 1499
	public abstract class IPGlobalStatistics
	{
		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06002F41 RID: 12097
		public abstract int DefaultTtl { get; }

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06002F42 RID: 12098
		public abstract bool ForwardingEnabled { get; }

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06002F43 RID: 12099
		public abstract int NumberOfInterfaces { get; }

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06002F44 RID: 12100
		public abstract int NumberOfIPAddresses { get; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06002F45 RID: 12101
		public abstract long OutputPacketRequests { get; }

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06002F46 RID: 12102
		public abstract long OutputPacketRoutingDiscards { get; }

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06002F47 RID: 12103
		public abstract long OutputPacketsDiscarded { get; }

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06002F48 RID: 12104
		public abstract long OutputPacketsWithNoRoute { get; }

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06002F49 RID: 12105
		public abstract long PacketFragmentFailures { get; }

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06002F4A RID: 12106
		public abstract long PacketReassembliesRequired { get; }

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06002F4B RID: 12107
		public abstract long PacketReassemblyFailures { get; }

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06002F4C RID: 12108
		public abstract long PacketReassemblyTimeout { get; }

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06002F4D RID: 12109
		public abstract long PacketsFragmented { get; }

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06002F4E RID: 12110
		public abstract long PacketsReassembled { get; }

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06002F4F RID: 12111
		public abstract long ReceivedPackets { get; }

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06002F50 RID: 12112
		public abstract long ReceivedPacketsDelivered { get; }

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06002F51 RID: 12113
		public abstract long ReceivedPacketsDiscarded { get; }

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06002F52 RID: 12114
		public abstract long ReceivedPacketsForwarded { get; }

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06002F53 RID: 12115
		public abstract long ReceivedPacketsWithAddressErrors { get; }

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06002F54 RID: 12116
		public abstract long ReceivedPacketsWithHeadersErrors { get; }

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06002F55 RID: 12117
		public abstract long ReceivedPacketsWithUnknownProtocol { get; }

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06002F56 RID: 12118
		public abstract int NumberOfRoutes { get; }
	}
}
