using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200061A RID: 1562
	public abstract class NetworkInterface
	{
		// Token: 0x06003017 RID: 12311 RVA: 0x000CFCCB File Offset: 0x000CECCB
		public static NetworkInterface[] GetAllNetworkInterfaces()
		{
			new NetworkInformationPermission(NetworkInformationAccess.Read).Demand();
			return SystemNetworkInterface.GetNetworkInterfaces();
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000CFCDD File Offset: 0x000CECDD
		public static bool GetIsNetworkAvailable()
		{
			return SystemNetworkInterface.InternalGetIsNetworkAvailable();
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000CFCE4 File Offset: 0x000CECE4
		public static int LoopbackInterfaceIndex
		{
			get
			{
				return SystemNetworkInterface.InternalLoopbackInterfaceIndex;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x0600301A RID: 12314
		public abstract string Id { get; }

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x0600301B RID: 12315
		public abstract string Name { get; }

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x0600301C RID: 12316
		public abstract string Description { get; }

		// Token: 0x0600301D RID: 12317
		public abstract IPInterfaceProperties GetIPProperties();

		// Token: 0x0600301E RID: 12318
		public abstract IPv4InterfaceStatistics GetIPv4Statistics();

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x0600301F RID: 12319
		public abstract OperationalStatus OperationalStatus { get; }

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003020 RID: 12320
		public abstract long Speed { get; }

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003021 RID: 12321
		public abstract bool IsReceiveOnly { get; }

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06003022 RID: 12322
		public abstract bool SupportsMulticast { get; }

		// Token: 0x06003023 RID: 12323
		public abstract PhysicalAddress GetPhysicalAddress();

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003024 RID: 12324
		public abstract NetworkInterfaceType NetworkInterfaceType { get; }

		// Token: 0x06003025 RID: 12325
		public abstract bool Supports(NetworkInterfaceComponent networkInterfaceComponent);
	}
}
