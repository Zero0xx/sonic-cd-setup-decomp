using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000634 RID: 1588
	internal class SystemIPv4InterfaceProperties : IPv4InterfaceProperties
	{
		// Token: 0x0600312B RID: 12587 RVA: 0x000D3570 File Offset: 0x000D2570
		internal SystemIPv4InterfaceProperties(FixedInfo fixedInfo, IpAdapterInfo ipAdapterInfo)
		{
			this.index = ipAdapterInfo.index;
			this.routingEnabled = fixedInfo.EnableRouting;
			this.dhcpEnabled = ipAdapterInfo.dhcpEnabled;
			this.haveWins = ipAdapterInfo.haveWins;
			this.gatewayAddresses = ipAdapterInfo.gatewayList.ToIPGatewayAddressCollection();
			this.dhcpAddresses = ipAdapterInfo.dhcpServer.ToIPAddressCollection();
			IPAddressCollection ipaddressCollection = ipAdapterInfo.primaryWinsServer.ToIPAddressCollection();
			IPAddressCollection ipaddressCollection2 = ipAdapterInfo.secondaryWinsServer.ToIPAddressCollection();
			this.winsServerAddresses = new IPAddressCollection();
			foreach (IPAddress address in ipaddressCollection)
			{
				this.winsServerAddresses.InternalAdd(address);
			}
			foreach (IPAddress address2 in ipaddressCollection2)
			{
				this.winsServerAddresses.InternalAdd(address2);
			}
			SystemIPv4InterfaceStatistics systemIPv4InterfaceStatistics = new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
			this.mtu = (uint)systemIPv4InterfaceStatistics.Mtu;
			if (ComNetOS.IsWin2K)
			{
				this.GetPerAdapterInfo(ipAdapterInfo.index);
				return;
			}
			this.dnsAddresses = fixedInfo.DnsAddresses;
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x000D36C4 File Offset: 0x000D26C4
		internal IPAddressCollection DnsAddresses
		{
			get
			{
				return this.dnsAddresses;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x000D36CC File Offset: 0x000D26CC
		public override bool UsesWins
		{
			get
			{
				return this.haveWins;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x000D36D4 File Offset: 0x000D26D4
		public override bool IsDhcpEnabled
		{
			get
			{
				return this.dhcpEnabled;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x000D36DC File Offset: 0x000D26DC
		public override bool IsForwardingEnabled
		{
			get
			{
				return this.routingEnabled;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x000D36E4 File Offset: 0x000D26E4
		public override bool IsAutomaticPrivateAddressingEnabled
		{
			get
			{
				return this.autoConfigEnabled;
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x000D36EC File Offset: 0x000D26EC
		public override bool IsAutomaticPrivateAddressingActive
		{
			get
			{
				return this.autoConfigActive;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06003132 RID: 12594 RVA: 0x000D36F4 File Offset: 0x000D26F4
		public override int Mtu
		{
			get
			{
				return (int)this.mtu;
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x000D36FC File Offset: 0x000D26FC
		public override int Index
		{
			get
			{
				return (int)this.index;
			}
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000D3704 File Offset: 0x000D2704
		internal GatewayIPAddressInformationCollection GetGatewayAddresses()
		{
			return this.gatewayAddresses;
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x000D370C File Offset: 0x000D270C
		internal IPAddressCollection GetDhcpServerAddresses()
		{
			return this.dhcpAddresses;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000D3714 File Offset: 0x000D2714
		internal IPAddressCollection GetWinsServersAddresses()
		{
			return this.winsServerAddresses;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000D371C File Offset: 0x000D271C
		private void GetPerAdapterInfo(uint index)
		{
			if (index != 0U)
			{
				uint cb = 0U;
				SafeLocalFree safeLocalFree = null;
				uint perAdapterInfo = UnsafeNetInfoNativeMethods.GetPerAdapterInfo(index, SafeLocalFree.Zero, ref cb);
				while (perAdapterInfo == 111U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)cb);
						perAdapterInfo = UnsafeNetInfoNativeMethods.GetPerAdapterInfo(index, safeLocalFree, ref cb);
						if (perAdapterInfo == 0U)
						{
							IpPerAdapterInfo ipPerAdapterInfo = (IpPerAdapterInfo)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpPerAdapterInfo));
							this.autoConfigEnabled = ipPerAdapterInfo.autoconfigEnabled;
							this.autoConfigActive = ipPerAdapterInfo.autoconfigActive;
							this.dnsAddresses = ipPerAdapterInfo.dnsServerList.ToIPAddressCollection();
						}
					}
					finally
					{
						if (this.dnsAddresses == null)
						{
							this.dnsAddresses = new IPAddressCollection();
						}
						if (safeLocalFree != null)
						{
							safeLocalFree.Close();
						}
					}
				}
				if (this.dnsAddresses == null)
				{
					this.dnsAddresses = new IPAddressCollection();
				}
				if (perAdapterInfo != 0U)
				{
					throw new NetworkInformationException((int)perAdapterInfo);
				}
			}
		}

		// Token: 0x04002E5C RID: 11868
		private bool haveWins;

		// Token: 0x04002E5D RID: 11869
		private bool dhcpEnabled;

		// Token: 0x04002E5E RID: 11870
		private bool routingEnabled;

		// Token: 0x04002E5F RID: 11871
		private bool autoConfigEnabled;

		// Token: 0x04002E60 RID: 11872
		private bool autoConfigActive;

		// Token: 0x04002E61 RID: 11873
		private uint index;

		// Token: 0x04002E62 RID: 11874
		private uint mtu;

		// Token: 0x04002E63 RID: 11875
		private GatewayIPAddressInformationCollection gatewayAddresses;

		// Token: 0x04002E64 RID: 11876
		private IPAddressCollection dhcpAddresses;

		// Token: 0x04002E65 RID: 11877
		private IPAddressCollection winsServerAddresses;

		// Token: 0x04002E66 RID: 11878
		internal IPAddressCollection dnsAddresses;
	}
}
