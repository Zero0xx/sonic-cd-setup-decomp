using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FE RID: 1534
	internal struct MibTcpStats
	{
		// Token: 0x04002D57 RID: 11607
		internal uint reTransmissionAlgorithm;

		// Token: 0x04002D58 RID: 11608
		internal uint minimumRetransmissionTimeOut;

		// Token: 0x04002D59 RID: 11609
		internal uint maximumRetransmissionTimeOut;

		// Token: 0x04002D5A RID: 11610
		internal uint maximumConnections;

		// Token: 0x04002D5B RID: 11611
		internal uint activeOpens;

		// Token: 0x04002D5C RID: 11612
		internal uint passiveOpens;

		// Token: 0x04002D5D RID: 11613
		internal uint failedConnectionAttempts;

		// Token: 0x04002D5E RID: 11614
		internal uint resetConnections;

		// Token: 0x04002D5F RID: 11615
		internal uint currentConnections;

		// Token: 0x04002D60 RID: 11616
		internal uint segmentsReceived;

		// Token: 0x04002D61 RID: 11617
		internal uint segmentsSent;

		// Token: 0x04002D62 RID: 11618
		internal uint segmentsResent;

		// Token: 0x04002D63 RID: 11619
		internal uint errorsReceived;

		// Token: 0x04002D64 RID: 11620
		internal uint segmentsSentWithReset;

		// Token: 0x04002D65 RID: 11621
		internal uint cumulativeConnections;
	}
}
