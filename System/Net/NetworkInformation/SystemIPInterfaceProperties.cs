using System;
using System.Collections;
using System.Net.Sockets;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062F RID: 1583
	internal class SystemIPInterfaceProperties : IPInterfaceProperties
	{
		// Token: 0x060030D6 RID: 12502 RVA: 0x000D27C3 File Offset: 0x000D17C3
		private SystemIPInterfaceProperties()
		{
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000D27CC File Offset: 0x000D17CC
		internal SystemIPInterfaceProperties(FixedInfo fixedInfo, IpAdapterAddresses ipAdapterAddresses)
		{
			this.dnsEnabled = fixedInfo.EnableDns;
			this.index = ipAdapterAddresses.index;
			this.name = ipAdapterAddresses.AdapterName;
			this.ipv6Index = ipAdapterAddresses.ipv6Index;
			if (this.index > 0U)
			{
				this.versionSupported |= IPVersion.IPv4;
			}
			if (this.ipv6Index > 0U)
			{
				this.versionSupported |= IPVersion.IPv6;
			}
			this.mtu = ipAdapterAddresses.mtu;
			this.adapterFlags = ipAdapterAddresses.flags;
			this.dnsSuffix = ipAdapterAddresses.dnsSuffix;
			this.dynamicDnsEnabled = ((ipAdapterAddresses.flags & AdapterFlags.DnsEnabled) > (AdapterFlags)0);
			this.multicastAddresses = SystemMulticastIPAddressInformation.ToAddressInformationCollection(ipAdapterAddresses.FirstMulticastAddress);
			this.dnsAddresses = SystemIPAddressInformation.ToAddressCollection(ipAdapterAddresses.FirstDnsServerAddress, this.versionSupported);
			this.anycastAddresses = SystemIPAddressInformation.ToAddressInformationCollection(ipAdapterAddresses.FirstAnycastAddress, this.versionSupported);
			this.unicastAddresses = SystemUnicastIPAddressInformation.ToAddressInformationCollection(ipAdapterAddresses.FirstUnicastAddress);
			if (this.ipv6Index > 0U)
			{
				this.ipv6Properties = new SystemIPv6InterfaceProperties(this.ipv6Index, this.mtu);
			}
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000D28F0 File Offset: 0x000D18F0
		internal SystemIPInterfaceProperties(FixedInfo fixedInfo, IpAdapterInfo ipAdapterInfo)
		{
			this.dnsEnabled = fixedInfo.EnableDns;
			this.name = ipAdapterInfo.adapterName;
			this.index = ipAdapterInfo.index;
			this.multicastAddresses = new MulticastIPAddressInformationCollection();
			this.anycastAddresses = new IPAddressInformationCollection();
			if (this.index > 0U)
			{
				this.versionSupported |= IPVersion.IPv4;
			}
			if (ComNetOS.IsWin2K)
			{
				this.ReadRegDnsSuffix();
			}
			this.unicastAddresses = new UnicastIPAddressInformationCollection();
			ArrayList arrayList = ipAdapterInfo.ipAddressList.ToIPExtendedAddressArrayList();
			foreach (object obj in arrayList)
			{
				IPExtendedAddress address = (IPExtendedAddress)obj;
				this.unicastAddresses.InternalAdd(new SystemUnicastIPAddressInformation(ipAdapterInfo, address));
			}
			try
			{
				this.ipv4Properties = new SystemIPv4InterfaceProperties(fixedInfo, ipAdapterInfo);
				if (this.dnsAddresses == null || this.dnsAddresses.Count == 0)
				{
					this.dnsAddresses = this.ipv4Properties.DnsAddresses;
				}
			}
			catch (NetworkInformationException ex)
			{
				if ((long)ex.ErrorCode != 87L)
				{
					throw;
				}
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060030D9 RID: 12505 RVA: 0x000D2A24 File Offset: 0x000D1A24
		public override bool IsDnsEnabled
		{
			get
			{
				return this.dnsEnabled;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x060030DA RID: 12506 RVA: 0x000D2A2C File Offset: 0x000D1A2C
		public override bool IsDynamicDnsEnabled
		{
			get
			{
				return this.dynamicDnsEnabled;
			}
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000D2A34 File Offset: 0x000D1A34
		public override IPv4InterfaceProperties GetIPv4Properties()
		{
			if (this.index == 0U)
			{
				throw new NetworkInformationException(SocketError.ProtocolNotSupported);
			}
			return this.ipv4Properties;
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000D2A4F File Offset: 0x000D1A4F
		public override IPv6InterfaceProperties GetIPv6Properties()
		{
			if (this.ipv6Index == 0U)
			{
				throw new NetworkInformationException(SocketError.ProtocolNotSupported);
			}
			return this.ipv6Properties;
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x000D2A6A File Offset: 0x000D1A6A
		public override string DnsSuffix
		{
			get
			{
				if (!ComNetOS.IsWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("Win2000Required"));
				}
				return this.dnsSuffix;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x000D2A89 File Offset: 0x000D1A89
		public override IPAddressInformationCollection AnycastAddresses
		{
			get
			{
				return this.anycastAddresses;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x000D2A91 File Offset: 0x000D1A91
		public override UnicastIPAddressInformationCollection UnicastAddresses
		{
			get
			{
				return this.unicastAddresses;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060030E0 RID: 12512 RVA: 0x000D2A99 File Offset: 0x000D1A99
		public override MulticastIPAddressInformationCollection MulticastAddresses
		{
			get
			{
				return this.multicastAddresses;
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x060030E1 RID: 12513 RVA: 0x000D2AA1 File Offset: 0x000D1AA1
		public override IPAddressCollection DnsAddresses
		{
			get
			{
				return this.dnsAddresses;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x060030E2 RID: 12514 RVA: 0x000D2AA9 File Offset: 0x000D1AA9
		public override GatewayIPAddressInformationCollection GatewayAddresses
		{
			get
			{
				if (this.ipv4Properties != null)
				{
					return this.ipv4Properties.GetGatewayAddresses();
				}
				return new GatewayIPAddressInformationCollection();
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x060030E3 RID: 12515 RVA: 0x000D2AC4 File Offset: 0x000D1AC4
		public override IPAddressCollection DhcpServerAddresses
		{
			get
			{
				if (this.ipv4Properties != null)
				{
					return this.ipv4Properties.GetDhcpServerAddresses();
				}
				return new IPAddressCollection();
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x060030E4 RID: 12516 RVA: 0x000D2ADF File Offset: 0x000D1ADF
		public override IPAddressCollection WinsServersAddresses
		{
			get
			{
				if (this.ipv4Properties != null)
				{
					return this.ipv4Properties.GetWinsServersAddresses();
				}
				return new IPAddressCollection();
			}
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000D2AFC File Offset: 0x000D1AFC
		internal bool Update(FixedInfo fixedInfo, IpAdapterInfo ipAdapterInfo)
		{
			try
			{
				ArrayList arrayList = ipAdapterInfo.ipAddressList.ToIPExtendedAddressArrayList();
				foreach (object obj in arrayList)
				{
					IPExtendedAddress ipextendedAddress = (IPExtendedAddress)obj;
					foreach (UnicastIPAddressInformation unicastIPAddressInformation in this.unicastAddresses)
					{
						SystemUnicastIPAddressInformation systemUnicastIPAddressInformation = (SystemUnicastIPAddressInformation)unicastIPAddressInformation;
						if (ipextendedAddress.address.Equals(systemUnicastIPAddressInformation.Address))
						{
							systemUnicastIPAddressInformation.ipv4Mask = ipextendedAddress.mask;
						}
					}
				}
				this.ipv4Properties = new SystemIPv4InterfaceProperties(fixedInfo, ipAdapterInfo);
				if (this.dnsAddresses == null || this.dnsAddresses.Count == 0)
				{
					this.dnsAddresses = this.ipv4Properties.DnsAddresses;
				}
			}
			catch (NetworkInformationException ex)
			{
				if ((long)ex.ErrorCode == 87L || (long)ex.ErrorCode == 13L || (long)ex.ErrorCode == 232L || (long)ex.ErrorCode == 1L || (long)ex.ErrorCode == 2L)
				{
					return false;
				}
				throw;
			}
			return true;
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000D2C48 File Offset: 0x000D1C48
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces")]
		private void ReadRegDnsSuffix()
		{
			RegistryKey registryKey = null;
			try
			{
				string text = "SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces\\" + this.name;
				registryKey = Registry.LocalMachine.OpenSubKey(text);
				if (registryKey != null)
				{
					this.dnsSuffix = (string)registryKey.GetValue("DhcpDomain");
					if (this.dnsSuffix == null)
					{
						this.dnsSuffix = (string)registryKey.GetValue("Domain");
						if (this.dnsSuffix == null)
						{
							this.dnsSuffix = string.Empty;
						}
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
		}

		// Token: 0x04002E45 RID: 11845
		private uint mtu;

		// Token: 0x04002E46 RID: 11846
		internal uint index;

		// Token: 0x04002E47 RID: 11847
		internal uint ipv6Index;

		// Token: 0x04002E48 RID: 11848
		internal IPVersion versionSupported;

		// Token: 0x04002E49 RID: 11849
		private bool dnsEnabled;

		// Token: 0x04002E4A RID: 11850
		private bool dynamicDnsEnabled;

		// Token: 0x04002E4B RID: 11851
		private IPAddressCollection dnsAddresses;

		// Token: 0x04002E4C RID: 11852
		private UnicastIPAddressInformationCollection unicastAddresses;

		// Token: 0x04002E4D RID: 11853
		private MulticastIPAddressInformationCollection multicastAddresses;

		// Token: 0x04002E4E RID: 11854
		private IPAddressInformationCollection anycastAddresses;

		// Token: 0x04002E4F RID: 11855
		private AdapterFlags adapterFlags;

		// Token: 0x04002E50 RID: 11856
		private string dnsSuffix;

		// Token: 0x04002E51 RID: 11857
		private string name;

		// Token: 0x04002E52 RID: 11858
		private SystemIPv4InterfaceProperties ipv4Properties;

		// Token: 0x04002E53 RID: 11859
		private SystemIPv6InterfaceProperties ipv6Properties;
	}
}
