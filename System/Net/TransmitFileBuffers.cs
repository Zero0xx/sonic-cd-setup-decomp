using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004FC RID: 1276
	[StructLayout(LayoutKind.Sequential)]
	internal class TransmitFileBuffers
	{
		// Token: 0x04002710 RID: 10000
		internal IntPtr preBuffer;

		// Token: 0x04002711 RID: 10001
		internal int preBufferLength;

		// Token: 0x04002712 RID: 10002
		internal IntPtr postBuffer;

		// Token: 0x04002713 RID: 10003
		internal int postBufferLength;
	}
}
