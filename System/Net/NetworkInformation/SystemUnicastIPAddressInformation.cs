using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000633 RID: 1587
	internal class SystemUnicastIPAddressInformation : UnicastIPAddressInformation
	{
		// Token: 0x0600311D RID: 12573 RVA: 0x000D324C File Offset: 0x000D224C
		private SystemUnicastIPAddressInformation()
		{
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000D3254 File Offset: 0x000D2254
		internal SystemUnicastIPAddressInformation(IpAdapterInfo ipAdapterInfo, IPExtendedAddress address)
		{
			this.innerInfo = new SystemIPAddressInformation(address.address);
			DateTime d = new DateTime(1970, 1, 1);
			d = d.AddSeconds(ipAdapterInfo.leaseExpires);
			this.dhcpLeaseLifetime = (long)(d - DateTime.UtcNow).TotalSeconds;
			this.ipv4Mask = address.mask;
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000D32BF File Offset: 0x000D22BF
		internal SystemUnicastIPAddressInformation(IpAdapterUnicastAddress adapterAddress, IPAddress ipAddress)
		{
			this.innerInfo = new SystemIPAddressInformation(adapterAddress, ipAddress);
			this.adapterAddress = adapterAddress;
			this.dhcpLeaseLifetime = (long)((ulong)adapterAddress.leaseLifetime);
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003120 RID: 12576 RVA: 0x000D32E9 File Offset: 0x000D22E9
		public override IPAddress Address
		{
			get
			{
				return this.innerInfo.Address;
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003121 RID: 12577 RVA: 0x000D32F6 File Offset: 0x000D22F6
		public override IPAddress IPv4Mask
		{
			get
			{
				if (this.Address.AddressFamily != AddressFamily.InterNetwork)
				{
					return new IPAddress(0);
				}
				return this.ipv4Mask;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06003122 RID: 12578 RVA: 0x000D3313 File Offset: 0x000D2313
		public override bool IsTransient
		{
			get
			{
				return this.innerInfo.IsTransient;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06003123 RID: 12579 RVA: 0x000D3320 File Offset: 0x000D2320
		public override bool IsDnsEligible
		{
			get
			{
				return this.innerInfo.IsDnsEligible;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x000D332D File Offset: 0x000D232D
		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return this.adapterAddress.prefixOrigin;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x000D3351 File Offset: 0x000D2351
		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return this.adapterAddress.suffixOrigin;
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06003126 RID: 12582 RVA: 0x000D3375 File Offset: 0x000D2375
		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return this.adapterAddress.dadState;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06003127 RID: 12583 RVA: 0x000D3399 File Offset: 0x000D2399
		public override long AddressValidLifetime
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return (long)((ulong)this.adapterAddress.validLifetime);
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06003128 RID: 12584 RVA: 0x000D33BE File Offset: 0x000D23BE
		public override long AddressPreferredLifetime
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return (long)((ulong)this.adapterAddress.preferredLifetime);
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x000D33E3 File Offset: 0x000D23E3
		public override long DhcpLeaseLifetime
		{
			get
			{
				return this.dhcpLeaseLifetime;
			}
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000D33EC File Offset: 0x000D23EC
		internal static UnicastIPAddressInformationCollection ToAddressInformationCollection(IntPtr ptr)
		{
			UnicastIPAddressInformationCollection unicastIPAddressInformationCollection = new UnicastIPAddressInformationCollection();
			if (ptr == IntPtr.Zero)
			{
				return unicastIPAddressInformationCollection;
			}
			IpAdapterUnicastAddress ipAdapterUnicastAddress = (IpAdapterUnicastAddress)Marshal.PtrToStructure(ptr, typeof(IpAdapterUnicastAddress));
			AddressFamily addressFamily = (ipAdapterUnicastAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
			SocketAddress socketAddress = new SocketAddress(addressFamily, ipAdapterUnicastAddress.address.addressLength);
			Marshal.Copy(ipAdapterUnicastAddress.address.address, socketAddress.m_Buffer, 0, ipAdapterUnicastAddress.address.addressLength);
			IPEndPoint ipendPoint;
			if (addressFamily == AddressFamily.InterNetwork)
			{
				ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
			}
			else
			{
				ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
			}
			unicastIPAddressInformationCollection.InternalAdd(new SystemUnicastIPAddressInformation(ipAdapterUnicastAddress, ipendPoint.Address));
			while (ipAdapterUnicastAddress.next != IntPtr.Zero)
			{
				ipAdapterUnicastAddress = (IpAdapterUnicastAddress)Marshal.PtrToStructure(ipAdapterUnicastAddress.next, typeof(IpAdapterUnicastAddress));
				addressFamily = ((ipAdapterUnicastAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
				socketAddress = new SocketAddress(addressFamily, ipAdapterUnicastAddress.address.addressLength);
				Marshal.Copy(ipAdapterUnicastAddress.address.address, socketAddress.m_Buffer, 0, ipAdapterUnicastAddress.address.addressLength);
				if (addressFamily == AddressFamily.InterNetwork)
				{
					ipendPoint = (IPEndPoint)IPEndPoint.Any.Create(socketAddress);
				}
				else
				{
					ipendPoint = (IPEndPoint)IPEndPoint.IPv6Any.Create(socketAddress);
				}
				unicastIPAddressInformationCollection.InternalAdd(new SystemUnicastIPAddressInformation(ipAdapterUnicastAddress, ipendPoint.Address));
			}
			return unicastIPAddressInformationCollection;
		}

		// Token: 0x04002E58 RID: 11864
		private IpAdapterUnicastAddress adapterAddress;

		// Token: 0x04002E59 RID: 11865
		private long dhcpLeaseLifetime;

		// Token: 0x04002E5A RID: 11866
		private SystemIPAddressInformation innerInfo;

		// Token: 0x04002E5B RID: 11867
		internal IPAddress ipv4Mask;
	}
}
