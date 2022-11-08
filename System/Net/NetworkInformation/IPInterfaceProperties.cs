using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005DC RID: 1500
	public abstract class IPInterfaceProperties
	{
		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06002F58 RID: 12120
		public abstract bool IsDnsEnabled { get; }

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06002F59 RID: 12121
		public abstract string DnsSuffix { get; }

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06002F5A RID: 12122
		public abstract bool IsDynamicDnsEnabled { get; }

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06002F5B RID: 12123
		public abstract UnicastIPAddressInformationCollection UnicastAddresses { get; }

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06002F5C RID: 12124
		public abstract MulticastIPAddressInformationCollection MulticastAddresses { get; }

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06002F5D RID: 12125
		public abstract IPAddressInformationCollection AnycastAddresses { get; }

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06002F5E RID: 12126
		public abstract IPAddressCollection DnsAddresses { get; }

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06002F5F RID: 12127
		public abstract GatewayIPAddressInformationCollection GatewayAddresses { get; }

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002F60 RID: 12128
		public abstract IPAddressCollection DhcpServerAddresses { get; }

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002F61 RID: 12129
		public abstract IPAddressCollection WinsServersAddresses { get; }

		// Token: 0x06002F62 RID: 12130
		public abstract IPv4InterfaceProperties GetIPv4Properties();

		// Token: 0x06002F63 RID: 12131
		public abstract IPv6InterfaceProperties GetIPv6Properties();
	}
}
