using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063D RID: 1597
	internal class SystemUdpStatistics : UdpStatistics
	{
		// Token: 0x0600317E RID: 12670 RVA: 0x000D4227 File Offset: 0x000D3227
		private SystemUdpStatistics()
		{
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x000D4230 File Offset: 0x000D3230
		internal SystemUdpStatistics(AddressFamily family)
		{
			uint num;
			if (!ComNetOS.IsPostWin2K)
			{
				if (family != AddressFamily.InterNetwork)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				num = UnsafeNetInfoNativeMethods.GetUdpStatistics(out this.stats);
			}
			else
			{
				num = UnsafeNetInfoNativeMethods.GetUdpStatisticsEx(out this.stats, family);
			}
			if (num != 0U)
			{
				throw new NetworkInformationException((int)num);
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000D4283 File Offset: 0x000D3283
		public override long DatagramsReceived
		{
			get
			{
				return (long)((ulong)this.stats.datagramsReceived);
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x000D4291 File Offset: 0x000D3291
		public override long IncomingDatagramsDiscarded
		{
			get
			{
				return (long)((ulong)this.stats.incomingDatagramsDiscarded);
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000D429F File Offset: 0x000D329F
		public override long IncomingDatagramsWithErrors
		{
			get
			{
				return (long)((ulong)this.stats.incomingDatagramsWithErrors);
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x000D42AD File Offset: 0x000D32AD
		public override long DatagramsSent
		{
			get
			{
				return (long)((ulong)this.stats.datagramsSent);
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06003184 RID: 12676 RVA: 0x000D42BB File Offset: 0x000D32BB
		public override int UdpListeners
		{
			get
			{
				return (int)this.stats.udpListeners;
			}
		}

		// Token: 0x04002E7D RID: 11901
		private MibUdpStats stats;
	}
}
