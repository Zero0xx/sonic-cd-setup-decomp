using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000638 RID: 1592
	public abstract class TcpConnectionInformation
	{
		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06003151 RID: 12625
		public abstract IPEndPoint LocalEndPoint { get; }

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06003152 RID: 12626
		public abstract IPEndPoint RemoteEndPoint { get; }

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06003153 RID: 12627
		public abstract TcpState State { get; }
	}
}
