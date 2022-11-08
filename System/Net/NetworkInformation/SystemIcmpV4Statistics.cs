using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000629 RID: 1577
	internal class SystemIcmpV4Statistics : IcmpV4Statistics
	{
		// Token: 0x06003074 RID: 12404 RVA: 0x000D17D0 File Offset: 0x000D07D0
		internal SystemIcmpV4Statistics()
		{
			uint icmpStatistics = UnsafeNetInfoNativeMethods.GetIcmpStatistics(out this.stats);
			if (icmpStatistics != 0U)
			{
				throw new NetworkInformationException((int)icmpStatistics);
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003075 RID: 12405 RVA: 0x000D17F9 File Offset: 0x000D07F9
		public override long MessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.messages);
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x000D180C File Offset: 0x000D080C
		public override long MessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.messages);
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003077 RID: 12407 RVA: 0x000D181F File Offset: 0x000D081F
		public override long ErrorsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.errors);
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003078 RID: 12408 RVA: 0x000D1832 File Offset: 0x000D0832
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.errors);
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x000D1845 File Offset: 0x000D0845
		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.destinationUnreachables);
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x000D1858 File Offset: 0x000D0858
		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.destinationUnreachables);
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x0600307B RID: 12411 RVA: 0x000D186B File Offset: 0x000D086B
		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timeExceeds);
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x000D187E File Offset: 0x000D087E
		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timeExceeds);
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x0600307D RID: 12413 RVA: 0x000D1891 File Offset: 0x000D0891
		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.parameterProblems);
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x000D18A4 File Offset: 0x000D08A4
		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.parameterProblems);
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x0600307F RID: 12415 RVA: 0x000D18B7 File Offset: 0x000D08B7
		public override long SourceQuenchesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.sourceQuenches);
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x000D18CA File Offset: 0x000D08CA
		public override long SourceQuenchesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.sourceQuenches);
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x000D18DD File Offset: 0x000D08DD
		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.redirects);
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x000D18F0 File Offset: 0x000D08F0
		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.redirects);
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x000D1903 File Offset: 0x000D0903
		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.echoRequests);
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000D1916 File Offset: 0x000D0916
		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.echoRequests);
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003085 RID: 12421 RVA: 0x000D1929 File Offset: 0x000D0929
		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.echoReplies);
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000D193C File Offset: 0x000D093C
		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.echoReplies);
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003087 RID: 12423 RVA: 0x000D194F File Offset: 0x000D094F
		public override long TimestampRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timestampRequests);
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000D1962 File Offset: 0x000D0962
		public override long TimestampRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timestampRequests);
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003089 RID: 12425 RVA: 0x000D1975 File Offset: 0x000D0975
		public override long TimestampRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.timestampReplies);
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x000D1988 File Offset: 0x000D0988
		public override long TimestampRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.timestampReplies);
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x0600308B RID: 12427 RVA: 0x000D199B File Offset: 0x000D099B
		public override long AddressMaskRequestsSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.addressMaskRequests);
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x000D19AE File Offset: 0x000D09AE
		public override long AddressMaskRequestsReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.addressMaskRequests);
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x0600308D RID: 12429 RVA: 0x000D19C1 File Offset: 0x000D09C1
		public override long AddressMaskRepliesSent
		{
			get
			{
				return (long)((ulong)this.stats.outStats.addressMaskReplies);
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x000D19D4 File Offset: 0x000D09D4
		public override long AddressMaskRepliesReceived
		{
			get
			{
				return (long)((ulong)this.stats.inStats.addressMaskReplies);
			}
		}

		// Token: 0x04002E2A RID: 11818
		private MibIcmpInfo stats;
	}
}
