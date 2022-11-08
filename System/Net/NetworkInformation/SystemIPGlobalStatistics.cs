using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000631 RID: 1585
	internal class SystemIPGlobalStatistics : IPGlobalStatistics
	{
		// Token: 0x060030F9 RID: 12537 RVA: 0x000D2E4E File Offset: 0x000D1E4E
		private SystemIPGlobalStatistics()
		{
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x000D2E64 File Offset: 0x000D1E64
		internal SystemIPGlobalStatistics(AddressFamily family)
		{
			uint num;
			if (!ComNetOS.IsPostWin2K)
			{
				if (family != AddressFamily.InterNetwork)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				num = UnsafeNetInfoNativeMethods.GetIpStatistics(out this.stats);
			}
			else
			{
				num = UnsafeNetInfoNativeMethods.GetIpStatisticsEx(out this.stats, family);
			}
			if (num != 0U)
			{
				throw new NetworkInformationException((int)num);
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x000D2EC3 File Offset: 0x000D1EC3
		public override bool ForwardingEnabled
		{
			get
			{
				return this.stats.forwardingEnabled;
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x060030FC RID: 12540 RVA: 0x000D2ED0 File Offset: 0x000D1ED0
		public override int DefaultTtl
		{
			get
			{
				return (int)this.stats.defaultTtl;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x000D2EDD File Offset: 0x000D1EDD
		public override long ReceivedPackets
		{
			get
			{
				return (long)((ulong)this.stats.packetsReceived);
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x000D2EEB File Offset: 0x000D1EEB
		public override long ReceivedPacketsWithHeadersErrors
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithHeaderErrors);
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x060030FF RID: 12543 RVA: 0x000D2EF9 File Offset: 0x000D1EF9
		public override long ReceivedPacketsWithAddressErrors
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithAddressErrors);
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x000D2F07 File Offset: 0x000D1F07
		public override long ReceivedPacketsForwarded
		{
			get
			{
				return (long)((ulong)this.stats.packetsForwarded);
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x000D2F15 File Offset: 0x000D1F15
		public override long ReceivedPacketsWithUnknownProtocol
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsWithUnknownProtocols);
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x000D2F23 File Offset: 0x000D1F23
		public override long ReceivedPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsDiscarded);
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x000D2F31 File Offset: 0x000D1F31
		public override long ReceivedPacketsDelivered
		{
			get
			{
				return (long)((ulong)this.stats.receivedPacketsDelivered);
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x000D2F3F File Offset: 0x000D1F3F
		public override long OutputPacketRequests
		{
			get
			{
				return (long)((ulong)this.stats.packetOutputRequests);
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x000D2F4D File Offset: 0x000D1F4D
		public override long OutputPacketRoutingDiscards
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketRoutingDiscards);
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x000D2F5B File Offset: 0x000D1F5B
		public override long OutputPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketsDiscarded);
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x000D2F69 File Offset: 0x000D1F69
		public override long OutputPacketsWithNoRoute
		{
			get
			{
				return (long)((ulong)this.stats.outputPacketsWithNoRoute);
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x000D2F77 File Offset: 0x000D1F77
		public override long PacketReassemblyTimeout
		{
			get
			{
				return (long)((ulong)this.stats.packetReassemblyTimeout);
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x000D2F85 File Offset: 0x000D1F85
		public override long PacketReassembliesRequired
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassemblyRequired);
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x0600310A RID: 12554 RVA: 0x000D2F93 File Offset: 0x000D1F93
		public override long PacketsReassembled
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassembled);
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x000D2FA1 File Offset: 0x000D1FA1
		public override long PacketReassemblyFailures
		{
			get
			{
				return (long)((ulong)this.stats.packetsReassemblyFailed);
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x0600310C RID: 12556 RVA: 0x000D2FAF File Offset: 0x000D1FAF
		public override long PacketsFragmented
		{
			get
			{
				return (long)((ulong)this.stats.packetsFragmented);
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x000D2FBD File Offset: 0x000D1FBD
		public override long PacketFragmentFailures
		{
			get
			{
				return (long)((ulong)this.stats.packetsFragmentFailed);
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x0600310E RID: 12558 RVA: 0x000D2FCB File Offset: 0x000D1FCB
		public override int NumberOfInterfaces
		{
			get
			{
				return (int)this.stats.interfaces;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x0600310F RID: 12559 RVA: 0x000D2FD8 File Offset: 0x000D1FD8
		public override int NumberOfIPAddresses
		{
			get
			{
				return (int)this.stats.ipAddresses;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06003110 RID: 12560 RVA: 0x000D2FE5 File Offset: 0x000D1FE5
		public override int NumberOfRoutes
		{
			get
			{
				return (int)this.stats.routes;
			}
		}

		// Token: 0x04002E55 RID: 11861
		private MibIpStats stats = default(MibIpStats);
	}
}
