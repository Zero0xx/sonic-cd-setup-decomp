using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E1 RID: 1505
	public abstract class UnicastIPAddressInformation : IPAddressInformation
	{
		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06002F72 RID: 12146
		public abstract long AddressPreferredLifetime { get; }

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06002F73 RID: 12147
		public abstract long AddressValidLifetime { get; }

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06002F74 RID: 12148
		public abstract long DhcpLeaseLifetime { get; }

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002F75 RID: 12149
		public abstract DuplicateAddressDetectionState DuplicateAddressDetectionState { get; }

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06002F76 RID: 12150
		public abstract PrefixOrigin PrefixOrigin { get; }

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06002F77 RID: 12151
		public abstract SuffixOrigin SuffixOrigin { get; }

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06002F78 RID: 12152
		public abstract IPAddress IPv4Mask { get; }
	}
}
