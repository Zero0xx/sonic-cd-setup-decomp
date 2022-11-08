using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200062C RID: 1580
	internal class SystemIPAddressInformation : IPAddressInformation
	{
		// Token: 0x060030B0 RID: 12464 RVA: 0x000D1D4A File Offset: 0x000D0D4A
		internal SystemIPAddressInformation(IPAddress address)
		{
			this.address = address;
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				this.dnsEligible = ((address.m_Address & 65193L) <= 0L);
			}
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x000D1D83 File Offset: 0x000D0D83
		internal SystemIPAddressInformation(IpAdapterUnicastAddress adapterAddress, IPAddress address)
		{
			this.address = address;
			this.transient = ((adapterAddress.flags & AdapterAddressFlags.Transient) > (AdapterAddressFlags)0);
			this.dnsEligible = ((adapterAddress.flags & AdapterAddressFlags.DnsEligible) > (AdapterAddressFlags)0);
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x000D1DBD File Offset: 0x000D0DBD
		internal SystemIPAddressInformation(IpAdapterAddress adapterAddress, IPAddress address)
		{
			this.address = address;
			this.transient = ((adapterAddress.flags & AdapterAddressFlags.Transient) > (AdapterAddressFlags)0);
			this.dnsEligible = ((adapterAddress.flags & AdapterAddressFlags.DnsEligible) > (AdapterAddressFlags)0);
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000D1DF7 File Offset: 0x000D0DF7
		public override IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000D1DFF File Offset: 0x000D0DFF
		public override bool IsTransient
		{
			get
			{
				return this.transient;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x000D1E07 File Offset: 0x000D0E07
		public override bool IsDnsEligible
		{
			get
			{
				return this.dnsEligible;
			}
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x000D1E10 File Offset: 0x000D0E10
		internal static IPAddressCollection ToAddressCollection(IntPtr ptr, IPVersion versionSupported)
		{
			IPAddressCollection ipaddressCollection = new IPAddressCollection();
			if (ptr == IntPtr.Zero)
			{
				return ipaddressCollection;
			}
			IpAdapterAddress ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ptr, typeof(IpAdapterAddress));
			AddressFamily addressFamily = (ipAdapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
			SocketAddress socketAddress = new SocketAddress(addressFamily, ipAdapterAddress.address.addressLength);
			Marshal.Copy(ipAdapterAddress.address.address, socketAddress.m_Buffer, 0, ipAdapterAddress.address.addressLength);
			IPEndPoint ipendPoint;
			if (addressFamily == AddressFamily.InterNetwork)
			{
				ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
			}
			else
			{
				ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
			}
			ipaddressCollection.InternalAdd(ipendPoint.Address);
			while (ipAdapterAddress.next != IntPtr.Zero)
			{
				ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ipAdapterAddress.next, typeof(IpAdapterAddress));
				addressFamily = ((ipAdapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
				if ((addressFamily == AddressFamily.InterNetwork && (versionSupported & IPVersion.IPv4) > IPVersion.None) || (addressFamily == AddressFamily.InterNetworkV6 && (versionSupported & IPVersion.IPv6) > IPVersion.None))
				{
					socketAddress = new SocketAddress(addressFamily, ipAdapterAddress.address.addressLength);
					Marshal.Copy(ipAdapterAddress.address.address, socketAddress.m_Buffer, 0, ipAdapterAddress.address.addressLength);
					if (addressFamily == AddressFamily.InterNetwork)
					{
						ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
					}
					else
					{
						ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
					}
					ipaddressCollection.InternalAdd(ipendPoint.Address);
				}
			}
			return ipaddressCollection;
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x000D1FA0 File Offset: 0x000D0FA0
		internal static IPAddressInformationCollection ToAddressInformationCollection(IntPtr ptr, IPVersion versionSupported)
		{
			IPAddressInformationCollection ipaddressInformationCollection = new IPAddressInformationCollection();
			if (ptr == IntPtr.Zero)
			{
				return ipaddressInformationCollection;
			}
			IpAdapterAddress adapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ptr, typeof(IpAdapterAddress));
			AddressFamily addressFamily = (adapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
			SocketAddress socketAddress = new SocketAddress(addressFamily, adapterAddress.address.addressLength);
			Marshal.Copy(adapterAddress.address.address, socketAddress.m_Buffer, 0, adapterAddress.address.addressLength);
			IPEndPoint ipendPoint;
			if (addressFamily == AddressFamily.InterNetwork)
			{
				ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
			}
			else
			{
				ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
			}
			ipaddressInformationCollection.InternalAdd(new SystemIPAddressInformation(adapterAddress, ipendPoint.Address));
			while (adapterAddress.next != IntPtr.Zero)
			{
				adapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(adapterAddress.next, typeof(IpAdapterAddress));
				addressFamily = ((adapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
				if ((addressFamily == AddressFamily.InterNetwork && (versionSupported & IPVersion.IPv4) > IPVersion.None) || (addressFamily == AddressFamily.InterNetworkV6 && (versionSupported & IPVersion.IPv6) > IPVersion.None))
				{
					socketAddress = new SocketAddress(addressFamily, adapterAddress.address.addressLength);
					Marshal.Copy(adapterAddress.address.address, socketAddress.m_Buffer, 0, adapterAddress.address.addressLength);
					if (addressFamily == AddressFamily.InterNetwork)
					{
						ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
					}
					else
					{
						ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
					}
					ipaddressInformationCollection.InternalAdd(new SystemIPAddressInformation(adapterAddress, ipendPoint.Address));
				}
			}
			return ipaddressInformationCollection;
		}

		// Token: 0x04002E3B RID: 11835
		private IPAddress address;

		// Token: 0x04002E3C RID: 11836
		internal bool transient;

		// Token: 0x04002E3D RID: 11837
		internal bool dnsEligible = true;
	}
}
