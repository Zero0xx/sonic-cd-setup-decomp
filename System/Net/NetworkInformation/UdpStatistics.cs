using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063C RID: 1596
	public abstract class UdpStatistics
	{
		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06003178 RID: 12664
		public abstract long DatagramsReceived { get; }

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06003179 RID: 12665
		public abstract long DatagramsSent { get; }

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x0600317A RID: 12666
		public abstract long IncomingDatagramsDiscarded { get; }

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x0600317B RID: 12667
		public abstract long IncomingDatagramsWithErrors { get; }

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x0600317C RID: 12668
		public abstract int UdpListeners { get; }
	}
}
