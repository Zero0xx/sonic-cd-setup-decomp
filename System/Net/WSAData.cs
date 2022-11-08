using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004FD RID: 1277
	internal struct WSAData
	{
		// Token: 0x04002714 RID: 10004
		internal short wVersion;

		// Token: 0x04002715 RID: 10005
		internal short wHighVersion;

		// Token: 0x04002716 RID: 10006
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
		internal string szDescription;

		// Token: 0x04002717 RID: 10007
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		internal string szSystemStatus;

		// Token: 0x04002718 RID: 10008
		internal short iMaxSockets;

		// Token: 0x04002719 RID: 10009
		internal short iMaxUdpDg;

		// Token: 0x0400271A RID: 10010
		internal IntPtr lpVendorInfo;
	}
}
