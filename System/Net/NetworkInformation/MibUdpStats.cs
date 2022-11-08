using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FD RID: 1533
	internal struct MibUdpStats
	{
		// Token: 0x04002D52 RID: 11602
		internal uint datagramsReceived;

		// Token: 0x04002D53 RID: 11603
		internal uint incomingDatagramsDiscarded;

		// Token: 0x04002D54 RID: 11604
		internal uint incomingDatagramsWithErrors;

		// Token: 0x04002D55 RID: 11605
		internal uint datagramsSent;

		// Token: 0x04002D56 RID: 11606
		internal uint udpListeners;
	}
}
