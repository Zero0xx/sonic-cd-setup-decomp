using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005DA RID: 1498
	public abstract class IPGlobalProperties
	{
		// Token: 0x06002F2E RID: 12078 RVA: 0x000CEC31 File Offset: 0x000CDC31
		public static IPGlobalProperties GetIPGlobalProperties()
		{
			new NetworkInformationPermission(NetworkInformationAccess.Read).Demand();
			return new SystemIPGlobalProperties();
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000CEC43 File Offset: 0x000CDC43
		internal static IPGlobalProperties InternalGetIPGlobalProperties()
		{
			return new SystemIPGlobalProperties();
		}

		// Token: 0x06002F30 RID: 12080
		public abstract IPEndPoint[] GetActiveUdpListeners();

		// Token: 0x06002F31 RID: 12081
		public abstract IPEndPoint[] GetActiveTcpListeners();

		// Token: 0x06002F32 RID: 12082
		public abstract TcpConnectionInformation[] GetActiveTcpConnections();

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06002F33 RID: 12083
		public abstract string DhcpScopeName { get; }

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06002F34 RID: 12084
		public abstract string DomainName { get; }

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06002F35 RID: 12085
		public abstract string HostName { get; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06002F36 RID: 12086
		public abstract bool IsWinsProxy { get; }

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06002F37 RID: 12087
		public abstract NetBiosNodeType NodeType { get; }

		// Token: 0x06002F38 RID: 12088
		public abstract TcpStatistics GetTcpIPv4Statistics();

		// Token: 0x06002F39 RID: 12089
		public abstract TcpStatistics GetTcpIPv6Statistics();

		// Token: 0x06002F3A RID: 12090
		public abstract UdpStatistics GetUdpIPv4Statistics();

		// Token: 0x06002F3B RID: 12091
		public abstract UdpStatistics GetUdpIPv6Statistics();

		// Token: 0x06002F3C RID: 12092
		public abstract IcmpV4Statistics GetIcmpV4Statistics();

		// Token: 0x06002F3D RID: 12093
		public abstract IcmpV6Statistics GetIcmpV6Statistics();

		// Token: 0x06002F3E RID: 12094
		public abstract IPGlobalStatistics GetIPv4GlobalStatistics();

		// Token: 0x06002F3F RID: 12095
		public abstract IPGlobalStatistics GetIPv6GlobalStatistics();
	}
}
