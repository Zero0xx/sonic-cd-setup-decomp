using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062D RID: 1581
	internal class SystemIPGlobalProperties : IPGlobalProperties
	{
		// Token: 0x060030B8 RID: 12472 RVA: 0x000D2139 File Offset: 0x000D1139
		internal SystemIPGlobalProperties()
		{
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x000D2144 File Offset: 0x000D1144
		internal static FixedInfo GetFixedInfo()
		{
			uint cb = 0U;
			SafeLocalFree safeLocalFree = null;
			FixedInfo result = default(FixedInfo);
			uint networkParams = UnsafeNetInfoNativeMethods.GetNetworkParams(SafeLocalFree.Zero, ref cb);
			while (networkParams == 111U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)cb);
					networkParams = UnsafeNetInfoNativeMethods.GetNetworkParams(safeLocalFree, ref cb);
					if (networkParams == 0U)
					{
						result = new FixedInfo((FIXED_INFO)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(FIXED_INFO)));
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
				}
			}
			if (networkParams != 0U)
			{
				throw new NetworkInformationException((int)networkParams);
			}
			return result;
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x000D21CC File Offset: 0x000D11CC
		internal FixedInfo FixedInfo
		{
			get
			{
				if (!this.fixedInfoInitialized)
				{
					lock (this)
					{
						if (!this.fixedInfoInitialized)
						{
							this.fixedInfo = SystemIPGlobalProperties.GetFixedInfo();
							this.fixedInfoInitialized = true;
						}
					}
				}
				return this.fixedInfo;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x060030BB RID: 12475 RVA: 0x000D2224 File Offset: 0x000D1224
		public override string HostName
		{
			get
			{
				if (SystemIPGlobalProperties.hostName == null)
				{
					lock (SystemIPGlobalProperties.syncObject)
					{
						if (SystemIPGlobalProperties.hostName == null)
						{
							SystemIPGlobalProperties.hostName = this.FixedInfo.HostName;
							SystemIPGlobalProperties.domainName = this.FixedInfo.DomainName;
						}
					}
				}
				return SystemIPGlobalProperties.hostName;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x060030BC RID: 12476 RVA: 0x000D2290 File Offset: 0x000D1290
		public override string DomainName
		{
			get
			{
				if (SystemIPGlobalProperties.domainName == null)
				{
					lock (SystemIPGlobalProperties.syncObject)
					{
						if (SystemIPGlobalProperties.domainName == null)
						{
							SystemIPGlobalProperties.hostName = this.FixedInfo.HostName;
							SystemIPGlobalProperties.domainName = this.FixedInfo.DomainName;
						}
					}
				}
				return SystemIPGlobalProperties.domainName;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x060030BD RID: 12477 RVA: 0x000D22FC File Offset: 0x000D12FC
		public override NetBiosNodeType NodeType
		{
			get
			{
				return this.FixedInfo.NodeType;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x000D2318 File Offset: 0x000D1318
		public override string DhcpScopeName
		{
			get
			{
				return this.FixedInfo.ScopeId;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x060030BF RID: 12479 RVA: 0x000D2334 File Offset: 0x000D1334
		public override bool IsWinsProxy
		{
			get
			{
				return this.FixedInfo.EnableProxy;
			}
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x000D2350 File Offset: 0x000D1350
		public override TcpConnectionInformation[] GetActiveTcpConnections()
		{
			ArrayList arrayList = new ArrayList();
			TcpConnectionInformation[] array = this.GetAllTcpConnections();
			foreach (TcpConnectionInformation tcpConnectionInformation in array)
			{
				if (tcpConnectionInformation.State != TcpState.Listen)
				{
					arrayList.Add(tcpConnectionInformation);
				}
			}
			array = new TcpConnectionInformation[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				array[j] = (TcpConnectionInformation)arrayList[j];
			}
			return array;
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000D23C4 File Offset: 0x000D13C4
		public override IPEndPoint[] GetActiveTcpListeners()
		{
			ArrayList arrayList = new ArrayList();
			TcpConnectionInformation[] allTcpConnections = this.GetAllTcpConnections();
			foreach (TcpConnectionInformation tcpConnectionInformation in allTcpConnections)
			{
				if (tcpConnectionInformation.State == TcpState.Listen)
				{
					arrayList.Add(tcpConnectionInformation.LocalEndPoint);
				}
			}
			IPEndPoint[] array2 = new IPEndPoint[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				array2[j] = (IPEndPoint)arrayList[j];
			}
			return array2;
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x000D2444 File Offset: 0x000D1444
		private TcpConnectionInformation[] GetAllTcpConnections()
		{
			uint cb = 0U;
			SafeLocalFree safeLocalFree = null;
			SystemTcpConnectionInformation[] array = null;
			uint tcpTable = UnsafeNetInfoNativeMethods.GetTcpTable(SafeLocalFree.Zero, ref cb, true);
			while (tcpTable == 122U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)cb);
					tcpTable = UnsafeNetInfoNativeMethods.GetTcpTable(safeLocalFree, ref cb, true);
					if (tcpTable == 0U)
					{
						IntPtr intPtr = safeLocalFree.DangerousGetHandle();
						MibTcpTable mibTcpTable = (MibTcpTable)Marshal.PtrToStructure(intPtr, typeof(MibTcpTable));
						if (mibTcpTable.numberOfEntries > 0U)
						{
							array = new SystemTcpConnectionInformation[mibTcpTable.numberOfEntries];
							intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibTcpTable.numberOfEntries));
							int num = 0;
							while ((long)num < (long)((ulong)mibTcpTable.numberOfEntries))
							{
								MibTcpRow mibTcpRow = (MibTcpRow)Marshal.PtrToStructure(intPtr, typeof(MibTcpRow));
								array[num] = new SystemTcpConnectionInformation(mibTcpRow);
								intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibTcpRow));
								num++;
							}
						}
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
				}
			}
			if (tcpTable != 0U && tcpTable != 232U)
			{
				throw new NetworkInformationException((int)tcpTable);
			}
			if (array == null)
			{
				return new SystemTcpConnectionInformation[0];
			}
			return array;
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x000D2578 File Offset: 0x000D1578
		public override IPEndPoint[] GetActiveUdpListeners()
		{
			uint cb = 0U;
			SafeLocalFree safeLocalFree = null;
			IPEndPoint[] array = null;
			uint udpTable = UnsafeNetInfoNativeMethods.GetUdpTable(SafeLocalFree.Zero, ref cb, true);
			while (udpTable == 122U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)cb);
					udpTable = UnsafeNetInfoNativeMethods.GetUdpTable(safeLocalFree, ref cb, true);
					if (udpTable == 0U)
					{
						IntPtr intPtr = safeLocalFree.DangerousGetHandle();
						MibUdpTable mibUdpTable = (MibUdpTable)Marshal.PtrToStructure(intPtr, typeof(MibUdpTable));
						if (mibUdpTable.numberOfEntries > 0U)
						{
							array = new IPEndPoint[mibUdpTable.numberOfEntries];
							intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibUdpTable.numberOfEntries));
							int num = 0;
							while ((long)num < (long)((ulong)mibUdpTable.numberOfEntries))
							{
								MibUdpRow mibUdpRow = (MibUdpRow)Marshal.PtrToStructure(intPtr, typeof(MibUdpRow));
								int port = (int)mibUdpRow.localPort3 << 24 | (int)mibUdpRow.localPort4 << 16 | (int)mibUdpRow.localPort1 << 8 | (int)mibUdpRow.localPort2;
								array[num] = new IPEndPoint((long)((ulong)mibUdpRow.localAddr), port);
								intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(mibUdpRow));
								num++;
							}
						}
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
				}
			}
			if (udpTable != 0U && udpTable != 232U)
			{
				throw new NetworkInformationException((int)udpTable);
			}
			if (array == null)
			{
				return new IPEndPoint[0];
			}
			return array;
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x000D26EC File Offset: 0x000D16EC
		public override IPGlobalStatistics GetIPv4GlobalStatistics()
		{
			return new SystemIPGlobalStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x000D26F4 File Offset: 0x000D16F4
		public override IPGlobalStatistics GetIPv6GlobalStatistics()
		{
			return new SystemIPGlobalStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x000D26FD File Offset: 0x000D16FD
		public override TcpStatistics GetTcpIPv4Statistics()
		{
			return new SystemTcpStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x000D2705 File Offset: 0x000D1705
		public override TcpStatistics GetTcpIPv6Statistics()
		{
			return new SystemTcpStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x000D270E File Offset: 0x000D170E
		public override UdpStatistics GetUdpIPv4Statistics()
		{
			return new SystemUdpStatistics(AddressFamily.InterNetwork);
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000D2716 File Offset: 0x000D1716
		public override UdpStatistics GetUdpIPv6Statistics()
		{
			return new SystemUdpStatistics(AddressFamily.InterNetworkV6);
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000D271F File Offset: 0x000D171F
		public override IcmpV4Statistics GetIcmpV4Statistics()
		{
			return new SystemIcmpV4Statistics();
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x000D2726 File Offset: 0x000D1726
		public override IcmpV6Statistics GetIcmpV6Statistics()
		{
			return new SystemIcmpV6Statistics();
		}

		// Token: 0x04002E3E RID: 11838
		private FixedInfo fixedInfo;

		// Token: 0x04002E3F RID: 11839
		private bool fixedInfoInitialized;

		// Token: 0x04002E40 RID: 11840
		private static string hostName = null;

		// Token: 0x04002E41 RID: 11841
		private static string domainName = null;

		// Token: 0x04002E42 RID: 11842
		private static object syncObject = new object();
	}
}
