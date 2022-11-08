using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000632 RID: 1586
	internal class SystemMulticastIPAddressInformation : MulticastIPAddressInformation
	{
		// Token: 0x06003111 RID: 12561 RVA: 0x000D2FF2 File Offset: 0x000D1FF2
		private SystemMulticastIPAddressInformation()
		{
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x000D2FFA File Offset: 0x000D1FFA
		internal SystemMulticastIPAddressInformation(IpAdapterAddress adapterAddress, IPAddress ipAddress)
		{
			this.innerInfo = new SystemIPAddressInformation(adapterAddress, ipAddress);
			this.adapterAddress = adapterAddress;
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x000D3016 File Offset: 0x000D2016
		public override IPAddress Address
		{
			get
			{
				return this.innerInfo.Address;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x000D3023 File Offset: 0x000D2023
		public override bool IsTransient
		{
			get
			{
				return this.innerInfo.IsTransient;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003115 RID: 12565 RVA: 0x000D3030 File Offset: 0x000D2030
		public override bool IsDnsEligible
		{
			get
			{
				return this.innerInfo.IsDnsEligible;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x000D303D File Offset: 0x000D203D
		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return PrefixOrigin.Other;
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003117 RID: 12567 RVA: 0x000D3057 File Offset: 0x000D2057
		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return SuffixOrigin.Other;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003118 RID: 12568 RVA: 0x000D3071 File Offset: 0x000D2071
		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return DuplicateAddressDetectionState.Invalid;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003119 RID: 12569 RVA: 0x000D308B File Offset: 0x000D208B
		public override long AddressValidLifetime
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return 0L;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x0600311A RID: 12570 RVA: 0x000D30A6 File Offset: 0x000D20A6
		public override long AddressPreferredLifetime
		{
			get
			{
				if (!ComNetOS.IsPostWin2K)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinXPRequired"));
				}
				return 0L;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x0600311B RID: 12571 RVA: 0x000D30C1 File Offset: 0x000D20C1
		public override long DhcpLeaseLifetime
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000D30C8 File Offset: 0x000D20C8
		internal static MulticastIPAddressInformationCollection ToAddressInformationCollection(IntPtr ptr)
		{
			MulticastIPAddressInformationCollection multicastIPAddressInformationCollection = new MulticastIPAddressInformationCollection();
			if (ptr == IntPtr.Zero)
			{
				return multicastIPAddressInformationCollection;
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
			multicastIPAddressInformationCollection.InternalAdd(new SystemMulticastIPAddressInformation(ipAdapterAddress, ipendPoint.Address));
			while (ipAdapterAddress.next != IntPtr.Zero)
			{
				ipAdapterAddress = (IpAdapterAddress)Marshal.PtrToStructure(ipAdapterAddress.next, typeof(IpAdapterAddress));
				addressFamily = ((ipAdapterAddress.address.addressLength > 16) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
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
				multicastIPAddressInformationCollection.InternalAdd(new SystemMulticastIPAddressInformation(ipAdapterAddress, ipendPoint.Address));
			}
			return multicastIPAddressInformationCollection;
		}

		// Token: 0x04002E56 RID: 11862
		private IpAdapterAddress adapterAddress;

		// Token: 0x04002E57 RID: 11863
		private SystemIPAddressInformation innerInfo;
	}
}
