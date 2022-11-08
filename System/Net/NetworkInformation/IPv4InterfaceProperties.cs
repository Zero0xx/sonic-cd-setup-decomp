using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E9 RID: 1513
	public abstract class IPv4InterfaceProperties
	{
		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06002FB5 RID: 12213
		public abstract bool UsesWins { get; }

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06002FB6 RID: 12214
		public abstract bool IsDhcpEnabled { get; }

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06002FB7 RID: 12215
		public abstract bool IsAutomaticPrivateAddressingActive { get; }

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06002FB8 RID: 12216
		public abstract bool IsAutomaticPrivateAddressingEnabled { get; }

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06002FB9 RID: 12217
		public abstract int Index { get; }

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06002FBA RID: 12218
		public abstract bool IsForwardingEnabled { get; }

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06002FBB RID: 12219
		public abstract int Mtu { get; }
	}
}
