using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F2 RID: 1522
	internal struct IpAddrString
	{
		// Token: 0x06002FC3 RID: 12227 RVA: 0x000CEF68 File Offset: 0x000CDF68
		internal IPAddressCollection ToIPAddressCollection()
		{
			IpAddrString ipAddrString = this;
			IPAddressCollection ipaddressCollection = new IPAddressCollection();
			if (ipAddrString.IpAddress.Length != 0)
			{
				ipaddressCollection.InternalAdd(IPAddress.Parse(ipAddrString.IpAddress));
			}
			while (ipAddrString.Next != IntPtr.Zero)
			{
				ipAddrString = (IpAddrString)Marshal.PtrToStructure(ipAddrString.Next, typeof(IpAddrString));
				if (ipAddrString.IpAddress.Length != 0)
				{
					ipaddressCollection.InternalAdd(IPAddress.Parse(ipAddrString.IpAddress));
				}
			}
			return ipaddressCollection;
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000CEFF4 File Offset: 0x000CDFF4
		internal ArrayList ToIPExtendedAddressArrayList()
		{
			IpAddrString ipAddrString = this;
			ArrayList arrayList = new ArrayList();
			if (ipAddrString.IpAddress.Length != 0)
			{
				arrayList.Add(new IPExtendedAddress(IPAddress.Parse(ipAddrString.IpAddress), IPAddress.Parse(ipAddrString.IpMask)));
			}
			while (ipAddrString.Next != IntPtr.Zero)
			{
				ipAddrString = (IpAddrString)Marshal.PtrToStructure(ipAddrString.Next, typeof(IpAddrString));
				if (ipAddrString.IpAddress.Length != 0)
				{
					arrayList.Add(new IPExtendedAddress(IPAddress.Parse(ipAddrString.IpAddress), IPAddress.Parse(ipAddrString.IpMask)));
				}
			}
			return arrayList;
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000CF0B0 File Offset: 0x000CE0B0
		internal GatewayIPAddressInformationCollection ToIPGatewayAddressCollection()
		{
			IpAddrString ipAddrString = this;
			GatewayIPAddressInformationCollection gatewayIPAddressInformationCollection = new GatewayIPAddressInformationCollection();
			if (ipAddrString.IpAddress.Length != 0)
			{
				gatewayIPAddressInformationCollection.InternalAdd(new SystemGatewayIPAddressInformation(IPAddress.Parse(ipAddrString.IpAddress)));
			}
			while (ipAddrString.Next != IntPtr.Zero)
			{
				ipAddrString = (IpAddrString)Marshal.PtrToStructure(ipAddrString.Next, typeof(IpAddrString));
				if (ipAddrString.IpAddress.Length != 0)
				{
					gatewayIPAddressInformationCollection.InternalAdd(new SystemGatewayIPAddressInformation(IPAddress.Parse(ipAddrString.IpAddress)));
				}
			}
			return gatewayIPAddressInformationCollection;
		}

		// Token: 0x04002CE1 RID: 11489
		internal IntPtr Next;

		// Token: 0x04002CE2 RID: 11490
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string IpAddress;

		// Token: 0x04002CE3 RID: 11491
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		internal string IpMask;

		// Token: 0x04002CE4 RID: 11492
		internal uint Context;
	}
}
