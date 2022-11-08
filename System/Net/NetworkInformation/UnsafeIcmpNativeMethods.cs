using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060D RID: 1549
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeIcmpNativeMethods
	{
		// Token: 0x06002FE0 RID: 12256
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern SafeCloseIcmpHandle IcmpCreateFile();

		// Token: 0x06002FE1 RID: 12257
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern bool IcmpCloseHandle(IntPtr icmpHandle);

		// Token: 0x06002FE2 RID: 12258
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, SafeWaitHandle Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FE3 RID: 12259
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern uint IcmpSendEcho2(SafeCloseIcmpHandle icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, [In] SafeLocalFree data, ushort dataSize, ref IPOptions options, SafeLocalFree replyBuffer, uint replySize, uint timeout);

		// Token: 0x06002FE4 RID: 12260
		[DllImport("icmp.dll", SetLastError = true)]
		internal static extern uint IcmpParseReplies(IntPtr replyBuffer, uint replySize);

		// Token: 0x04002DB7 RID: 11703
		private const string ICMP = "icmp.dll";
	}
}
