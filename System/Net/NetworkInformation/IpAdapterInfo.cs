using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F5 RID: 1525
	internal struct IpAdapterInfo
	{
		// Token: 0x04002CF4 RID: 11508
		internal const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;

		// Token: 0x04002CF5 RID: 11509
		internal const int MAX_ADAPTER_NAME_LENGTH = 256;

		// Token: 0x04002CF6 RID: 11510
		internal const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

		// Token: 0x04002CF7 RID: 11511
		internal IntPtr Next;

		// Token: 0x04002CF8 RID: 11512
		internal uint comboIndex;

		// Token: 0x04002CF9 RID: 11513
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		internal string adapterName;

		// Token: 0x04002CFA RID: 11514
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		internal string description;

		// Token: 0x04002CFB RID: 11515
		internal uint addressLength;

		// Token: 0x04002CFC RID: 11516
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		internal byte[] address;

		// Token: 0x04002CFD RID: 11517
		internal uint index;

		// Token: 0x04002CFE RID: 11518
		internal OldInterfaceType type;

		// Token: 0x04002CFF RID: 11519
		internal bool dhcpEnabled;

		// Token: 0x04002D00 RID: 11520
		internal IntPtr currentIpAddress;

		// Token: 0x04002D01 RID: 11521
		internal IpAddrString ipAddressList;

		// Token: 0x04002D02 RID: 11522
		internal IpAddrString gatewayList;

		// Token: 0x04002D03 RID: 11523
		internal IpAddrString dhcpServer;

		// Token: 0x04002D04 RID: 11524
		[MarshalAs(UnmanagedType.Bool)]
		internal bool haveWins;

		// Token: 0x04002D05 RID: 11525
		internal IpAddrString primaryWinsServer;

		// Token: 0x04002D06 RID: 11526
		internal IpAddrString secondaryWinsServer;

		// Token: 0x04002D07 RID: 11527
		internal uint leaseObtained;

		// Token: 0x04002D08 RID: 11528
		internal uint leaseExpires;
	}
}
