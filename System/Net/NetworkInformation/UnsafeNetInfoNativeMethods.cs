using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060C RID: 1548
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNetInfoNativeMethods
	{
		// Token: 0x06002FC7 RID: 12231
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetAdaptersInfo(SafeLocalFree pAdapterInfo, ref uint pOutBufLen);

		// Token: 0x06002FC8 RID: 12232
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetAdaptersAddresses(AddressFamily family, uint flags, IntPtr pReserved, SafeLocalFree adapterAddresses, ref uint outBufLen);

		// Token: 0x06002FC9 RID: 12233
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetBestInterface(int ipAddress, out int index);

		// Token: 0x06002FCA RID: 12234
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIfEntry(ref MibIfRow pIfRow);

		// Token: 0x06002FCB RID: 12235
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIpStatistics(out MibIpStats statistics);

		// Token: 0x06002FCC RID: 12236
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIpStatisticsEx(out MibIpStats statistics, AddressFamily family);

		// Token: 0x06002FCD RID: 12237
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetTcpStatistics(out MibTcpStats statistics);

		// Token: 0x06002FCE RID: 12238
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetTcpStatisticsEx(out MibTcpStats statistics, AddressFamily family);

		// Token: 0x06002FCF RID: 12239
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetUdpStatistics(out MibUdpStats statistics);

		// Token: 0x06002FD0 RID: 12240
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetUdpStatisticsEx(out MibUdpStats statistics, AddressFamily family);

		// Token: 0x06002FD1 RID: 12241
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIcmpStatistics(out MibIcmpInfo statistics);

		// Token: 0x06002FD2 RID: 12242
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetIcmpStatisticsEx(out MibIcmpInfoEx statistics, AddressFamily family);

		// Token: 0x06002FD3 RID: 12243
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetTcpTable(SafeLocalFree pTcpTable, ref uint dwOutBufLen, bool order);

		// Token: 0x06002FD4 RID: 12244
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetUdpTable(SafeLocalFree pUdpTable, ref uint dwOutBufLen, bool order);

		// Token: 0x06002FD5 RID: 12245
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetNetworkParams(SafeLocalFree pFixedInfo, ref uint pOutBufLen);

		// Token: 0x06002FD6 RID: 12246
		[DllImport("iphlpapi.dll")]
		internal static extern uint GetPerAdapterInfo(uint IfIndex, SafeLocalFree pPerAdapterInfo, ref uint pOutBufLen);

		// Token: 0x06002FD7 RID: 12247
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern SafeCloseIcmpHandle IcmpCreateFile();

		// Token: 0x06002FD8 RID: 12248
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern SafeCloseIcmpHandle Icmp6CreateFile();

		// Token: 0x06002FD9 RID: 12249
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern bool IcmpCloseHandle(IntPtr handle);

		// Token: 0x06002FDA RID: 12250
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, SafeWaitHandle Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FDB RID: 12251
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FDC RID: 12252
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint Icmp6SendEcho2(SafeCloseIcmpHandle icmpHandle, SafeWaitHandle Event, IntPtr apcRoutine, IntPtr apcContext, byte[] sourceSocketAddress, byte[] destSocketAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FDD RID: 12253
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint Icmp6SendEcho2(SafeCloseIcmpHandle icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, byte[] sourceSocketAddress, byte[] destSocketAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FDE RID: 12254
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint IcmpParseReplies(IntPtr replyBuffer, uint replySize);

		// Token: 0x06002FDF RID: 12255
		[DllImport("iphlpapi.dll", SetLastError = true)]
		internal static extern uint Icmp6ParseReplies(IntPtr replyBuffer, uint replySize);

		// Token: 0x04002DB6 RID: 11702
		private const string IPHLPAPI = "iphlpapi.dll";
	}
}
