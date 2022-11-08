using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005DD RID: 1501
	public abstract class IPv4InterfaceStatistics
	{
		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002F65 RID: 12133
		public abstract long BytesReceived { get; }

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06002F66 RID: 12134
		public abstract long BytesSent { get; }

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06002F67 RID: 12135
		public abstract long IncomingPacketsDiscarded { get; }

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06002F68 RID: 12136
		public abstract long IncomingPacketsWithErrors { get; }

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06002F69 RID: 12137
		public abstract long IncomingUnknownProtocolPackets { get; }

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06002F6A RID: 12138
		public abstract long NonUnicastPacketsReceived { get; }

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06002F6B RID: 12139
		public abstract long NonUnicastPacketsSent { get; }

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06002F6C RID: 12140
		public abstract long OutgoingPacketsDiscarded { get; }

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06002F6D RID: 12141
		public abstract long OutgoingPacketsWithErrors { get; }

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06002F6E RID: 12142
		public abstract long OutputQueueLength { get; }

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06002F6F RID: 12143
		public abstract long UnicastPacketsReceived { get; }

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06002F70 RID: 12144
		public abstract long UnicastPacketsSent { get; }
	}
}
