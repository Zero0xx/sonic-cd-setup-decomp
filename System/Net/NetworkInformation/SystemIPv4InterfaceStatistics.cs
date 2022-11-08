using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000630 RID: 1584
	internal class SystemIPv4InterfaceStatistics : IPv4InterfaceStatistics
	{
		// Token: 0x060030E7 RID: 12519 RVA: 0x000D2CDC File Offset: 0x000D1CDC
		private SystemIPv4InterfaceStatistics()
		{
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000D2CF0 File Offset: 0x000D1CF0
		internal SystemIPv4InterfaceStatistics(long index)
		{
			this.GetIfEntry(index);
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x000D2D0B File Offset: 0x000D1D0B
		public override long OutputQueueLength
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutQLen);
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x000D2D19 File Offset: 0x000D1D19
		public override long BytesSent
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutOctets);
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x060030EB RID: 12523 RVA: 0x000D2D27 File Offset: 0x000D1D27
		public override long BytesReceived
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInOctets);
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x000D2D35 File Offset: 0x000D1D35
		public override long UnicastPacketsSent
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutUcastPkts);
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x000D2D43 File Offset: 0x000D1D43
		public override long UnicastPacketsReceived
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInUcastPkts);
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060030EE RID: 12526 RVA: 0x000D2D51 File Offset: 0x000D1D51
		public override long NonUnicastPacketsSent
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutNUcastPkts);
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060030EF RID: 12527 RVA: 0x000D2D5F File Offset: 0x000D1D5F
		public override long NonUnicastPacketsReceived
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInNUcastPkts);
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x000D2D6D File Offset: 0x000D1D6D
		public override long IncomingPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInDiscards);
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060030F1 RID: 12529 RVA: 0x000D2D7B File Offset: 0x000D1D7B
		public override long OutgoingPacketsDiscarded
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutDiscards);
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x000D2D89 File Offset: 0x000D1D89
		public override long IncomingPacketsWithErrors
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInErrors);
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x060030F3 RID: 12531 RVA: 0x000D2D97 File Offset: 0x000D1D97
		public override long OutgoingPacketsWithErrors
		{
			get
			{
				return (long)((ulong)this.ifRow.dwOutErrors);
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x060030F4 RID: 12532 RVA: 0x000D2DA5 File Offset: 0x000D1DA5
		public override long IncomingUnknownProtocolPackets
		{
			get
			{
				return (long)((ulong)this.ifRow.dwInUnknownProtos);
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x060030F5 RID: 12533 RVA: 0x000D2DB3 File Offset: 0x000D1DB3
		internal long Mtu
		{
			get
			{
				return (long)((ulong)this.ifRow.dwMtu);
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x000D2DC4 File Offset: 0x000D1DC4
		internal OperationalStatus OperationalStatus
		{
			get
			{
				switch (this.ifRow.operStatus)
				{
				case OldOperationalStatus.NonOperational:
					return OperationalStatus.Down;
				case OldOperationalStatus.Unreachable:
					return OperationalStatus.Down;
				case OldOperationalStatus.Disconnected:
					return OperationalStatus.Dormant;
				case OldOperationalStatus.Connecting:
					return OperationalStatus.Dormant;
				case OldOperationalStatus.Connected:
					return OperationalStatus.Up;
				case OldOperationalStatus.Operational:
					return OperationalStatus.Up;
				default:
					return OperationalStatus.Unknown;
				}
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x000D2E0A File Offset: 0x000D1E0A
		internal long Speed
		{
			get
			{
				return (long)((ulong)this.ifRow.dwSpeed);
			}
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000D2E18 File Offset: 0x000D1E18
		private void GetIfEntry(long index)
		{
			if (index == 0L)
			{
				return;
			}
			this.ifRow.dwIndex = (uint)index;
			uint ifEntry = UnsafeNetInfoNativeMethods.GetIfEntry(ref this.ifRow);
			if (ifEntry != 0U)
			{
				throw new NetworkInformationException((int)ifEntry);
			}
		}

		// Token: 0x04002E54 RID: 11860
		private MibIfRow ifRow = default(MibIfRow);
	}
}
