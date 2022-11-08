using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E3 RID: 1507
	public abstract class MulticastIPAddressInformation : IPAddressInformation
	{
		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06002F86 RID: 12166
		public abstract long AddressPreferredLifetime { get; }

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06002F87 RID: 12167
		public abstract long AddressValidLifetime { get; }

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06002F88 RID: 12168
		public abstract long DhcpLeaseLifetime { get; }

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06002F89 RID: 12169
		public abstract DuplicateAddressDetectionState DuplicateAddressDetectionState { get; }

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06002F8A RID: 12170
		public abstract PrefixOrigin PrefixOrigin { get; }

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06002F8B RID: 12171
		public abstract SuffixOrigin SuffixOrigin { get; }
	}
}
