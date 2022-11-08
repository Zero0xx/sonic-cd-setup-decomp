using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000501 RID: 1281
	internal struct IPv6MulticastRequest
	{
		// Token: 0x0400272D RID: 10029
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] MulticastAddress;

		// Token: 0x0400272E RID: 10030
		internal int InterfaceIndex;

		// Token: 0x0400272F RID: 10031
		internal static readonly int Size = Marshal.SizeOf(typeof(IPv6MulticastRequest));
	}
}
