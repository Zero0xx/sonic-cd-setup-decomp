using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063A RID: 1594
	public abstract class TcpStatistics
	{
		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06003159 RID: 12633
		public abstract long ConnectionsAccepted { get; }

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x0600315A RID: 12634
		public abstract long ConnectionsInitiated { get; }

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x0600315B RID: 12635
		public abstract long CumulativeConnections { get; }

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600315C RID: 12636
		public abstract long CurrentConnections { get; }

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600315D RID: 12637
		public abstract long ErrorsReceived { get; }

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x0600315E RID: 12638
		public abstract long FailedConnectionAttempts { get; }

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x0600315F RID: 12639
		public abstract long MaximumConnections { get; }

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06003160 RID: 12640
		public abstract long MaximumTransmissionTimeout { get; }

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06003161 RID: 12641
		public abstract long MinimumTransmissionTimeout { get; }

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06003162 RID: 12642
		public abstract long ResetConnections { get; }

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06003163 RID: 12643
		public abstract long SegmentsReceived { get; }

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06003164 RID: 12644
		public abstract long SegmentsResent { get; }

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06003165 RID: 12645
		public abstract long SegmentsSent { get; }

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06003166 RID: 12646
		public abstract long ResetsSent { get; }
	}
}
