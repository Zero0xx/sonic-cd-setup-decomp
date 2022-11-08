using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062B RID: 1579
	internal class SystemIcmpV6Statistics : IcmpV6Statistics
	{
		// Token: 0x0600308F RID: 12431 RVA: 0x000D19E8 File Offset: 0x000D09E8
		internal SystemIcmpV6Statistics()
		{
			if (!ComNetOS.IsPostWin2K)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
			}
			uint icmpStatisticsEx = UnsafeNetInfoNativeMethods.GetIcmpStatisticsEx(out this.stats, AddressFamily.InterNetworkV6);
			if (icmpStatisticsEx != 0U)
			{
				throw new NetworkInformationException((int)icmpStatisticsEx);
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x000D1A2A File Offset: 0x000D0A2A
		public override long MessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.dwMsgs);
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x000D1A3D File Offset: 0x000D0A3D
		public override long MessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.dwMsgs);
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003092 RID: 12434 RVA: 0x000D1A50 File Offset: 0x000D0A50
		public override long ErrorsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.dwErrors);
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x000D1A63 File Offset: 0x000D0A63
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.dwErrors);
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06003094 RID: 12436 RVA: 0x000D1A76 File Offset: 0x000D0A76
		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)1L))]);
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x000D1A8D File Offset: 0x000D0A8D
		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)1L))]);
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06003096 RID: 12438 RVA: 0x000D1AA4 File Offset: 0x000D0AA4
		public override long PacketTooBigMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)2L))]);
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x000D1ABB File Offset: 0x000D0ABB
		public override long PacketTooBigMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)2L))]);
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003098 RID: 12440 RVA: 0x000D1AD2 File Offset: 0x000D0AD2
		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)3L))]);
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003099 RID: 12441 RVA: 0x000D1AE9 File Offset: 0x000D0AE9
		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)3L))]);
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x000D1B00 File Offset: 0x000D0B00
		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)4L))]);
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x000D1B17 File Offset: 0x000D0B17
		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)4L))]);
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x0600309C RID: 12444 RVA: 0x000D1B2E File Offset: 0x000D0B2E
		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)128L))]);
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x000D1B49 File Offset: 0x000D0B49
		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)128L))]);
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x000D1B64 File Offset: 0x000D0B64
		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)129L))]);
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x0600309F RID: 12447 RVA: 0x000D1B7F File Offset: 0x000D0B7F
		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)129L))]);
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x060030A0 RID: 12448 RVA: 0x000D1B9A File Offset: 0x000D0B9A
		public override long MembershipQueriesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)130L))]);
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060030A1 RID: 12449 RVA: 0x000D1BB5 File Offset: 0x000D0BB5
		public override long MembershipQueriesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)130L))]);
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060030A2 RID: 12450 RVA: 0x000D1BD0 File Offset: 0x000D0BD0
		public override long MembershipReportsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)131L))]);
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x000D1BEB File Offset: 0x000D0BEB
		public override long MembershipReportsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)131L))]);
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060030A4 RID: 12452 RVA: 0x000D1C06 File Offset: 0x000D0C06
		public override long MembershipReductionsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)132L))]);
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060030A5 RID: 12453 RVA: 0x000D1C21 File Offset: 0x000D0C21
		public override long MembershipReductionsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)132L))]);
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x000D1C3C File Offset: 0x000D0C3C
		public override long RouterAdvertisementsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)134L))]);
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x000D1C57 File Offset: 0x000D0C57
		public override long RouterAdvertisementsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)134L))]);
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060030A8 RID: 12456 RVA: 0x000D1C72 File Offset: 0x000D0C72
		public override long RouterSolicitsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)133L))]);
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060030A9 RID: 12457 RVA: 0x000D1C8D File Offset: 0x000D0C8D
		public override long RouterSolicitsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)133L))]);
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x060030AA RID: 12458 RVA: 0x000D1CA8 File Offset: 0x000D0CA8
		public override long NeighborAdvertisementsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)136L))]);
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x060030AB RID: 12459 RVA: 0x000D1CC3 File Offset: 0x000D0CC3
		public override long NeighborAdvertisementsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)136L))]);
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x060030AC RID: 12460 RVA: 0x000D1CDE File Offset: 0x000D0CDE
		public override long NeighborSolicitsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)135L))]);
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x060030AD RID: 12461 RVA: 0x000D1CF9 File Offset: 0x000D0CF9
		public override long NeighborSolicitsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)135L))]);
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x060030AE RID: 12462 RVA: 0x000D1D14 File Offset: 0x000D0D14
		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.rgdwTypeCount[(int)(checked((IntPtr)137L))]);
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x000D1D2F File Offset: 0x000D0D2F
		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.rgdwTypeCount[(int)(checked((IntPtr)137L))]);
			}
		}

		// Token: 0x04002E3A RID: 11834
		private MibIcmpInfoEx stats;
	}
}
