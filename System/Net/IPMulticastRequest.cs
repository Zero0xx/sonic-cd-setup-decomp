using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004F9 RID: 1273
	internal struct IPMulticastRequest
	{
		// Token: 0x04002709 RID: 9993
		internal int MulticastAddress;

		// Token: 0x0400270A RID: 9994
		internal int InterfaceAddress;

		// Token: 0x0400270B RID: 9995
		internal static readonly int Size = Marshal.SizeOf(typeof(IPMulticastRequest));
	}
}
