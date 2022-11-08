using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063B RID: 1595
	internal class SystemTcpStatistics : TcpStatistics
	{
		// Token: 0x06003168 RID: 12648 RVA: 0x000D4100 File Offset: 0x000D3100
		private SystemTcpStatistics()
		{
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x000D4108 File Offset: 0x000D3108
		internal SystemTcpStatistics(AddressFamily family)
		{
			uint num;
			if (!ComNetOS.IsPostWin2K)
			{
				if (family != AddressFamily.InterNetwork)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				num = UnsafeNetInfoNativeMethods.GetTcpStatistics(out this.stats);
			}
			else
			{
				num = UnsafeNetInfoNativeMethods.GetTcpStatisticsEx(out this.stats, family);
			}
			if (num != 0U)
			{
				throw new NetworkInformationException((int)num);
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x0600316A RID: 12650 RVA: 0x000D415B File Offset: 0x000D315B
		public override long MinimumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.stats.minimumRetransmissionTimeOut);
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x000D4169 File Offset: 0x000D3169
		public override long MaximumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.stats.maximumRetransmissionTimeOut);
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x0600316C RID: 12652 RVA: 0x000D4177 File Offset: 0x000D3177
		public override long MaximumConnections
		{
			get
			{
				return (long)((ulong)this.stats.maximumConnections);
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x000D4185 File Offset: 0x000D3185
		public override long ConnectionsInitiated
		{
			get
			{
				return (long)((ulong)this.stats.activeOpens);
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x000D4193 File Offset: 0x000D3193
		public override long ConnectionsAccepted
		{
			get
			{
				return (long)((ulong)this.stats.passiveOpens);
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x000D41A1 File Offset: 0x000D31A1
		public override long FailedConnectionAttempts
		{
			get
			{
				return (long)((ulong)this.stats.failedConnectionAttempts);
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x000D41AF File Offset: 0x000D31AF
		public override long ResetConnections
		{
			get
			{
				return (long)((ulong)this.stats.resetConnections);
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x000D41BD File Offset: 0x000D31BD
		public override long CurrentConnections
		{
			get
			{
				return (long)((ulong)this.stats.currentConnections);
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x000D41CB File Offset: 0x000D31CB
		public override long SegmentsReceived
		{
			get
			{
				return (long)((ulong)this.stats.segmentsReceived);
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x000D41D9 File Offset: 0x000D31D9
		public override long SegmentsSent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsSent);
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x000D41E7 File Offset: 0x000D31E7
		public override long SegmentsResent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsResent);
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06003175 RID: 12661 RVA: 0x000D41F5 File Offset: 0x000D31F5
		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.stats.errorsReceived);
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x000D4203 File Offset: 0x000D3203
		public override long ResetsSent
		{
			get
			{
				return (long)((ulong)this.stats.segmentsSentWithReset);
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x000D4211 File Offset: 0x000D3211
		public override long CumulativeConnections
		{
			get
			{
				return (long)((ulong)this.stats.cumulativeConnections);
			}
		}

		// Token: 0x04002E7C RID: 11900
		private MibTcpStats stats;
	}
}
