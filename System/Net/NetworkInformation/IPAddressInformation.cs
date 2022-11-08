using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005D8 RID: 1496
	public abstract class IPAddressInformation
	{
		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06002F1E RID: 12062
		public abstract IPAddress Address { get; }

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06002F1F RID: 12063
		public abstract bool IsDnsEligible { get; }

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06002F20 RID: 12064
		public abstract bool IsTransient { get; }
	}
}
