using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000544 RID: 1348
	[StructLayout(LayoutKind.Sequential)]
	internal class SslConnectionInfo
	{
		// Token: 0x0600290F RID: 10511 RVA: 0x000AB64C File Offset: 0x000AA64C
		internal unsafe SslConnectionInfo(byte[] nativeBuffer)
		{
			fixed (void* ptr = nativeBuffer)
			{
				IntPtr ptr2 = new IntPtr(ptr);
				this.Protocol = Marshal.ReadInt32(ptr2);
				this.DataCipherAlg = Marshal.ReadInt32(ptr2, 4);
				this.DataKeySize = Marshal.ReadInt32(ptr2, 8);
				this.DataHashAlg = Marshal.ReadInt32(ptr2, 12);
				this.DataHashKeySize = Marshal.ReadInt32(ptr2, 16);
				this.KeyExchangeAlg = Marshal.ReadInt32(ptr2, 20);
				this.KeyExchKeySize = Marshal.ReadInt32(ptr2, 24);
			}
		}

		// Token: 0x0400280C RID: 10252
		public readonly int Protocol;

		// Token: 0x0400280D RID: 10253
		public readonly int DataCipherAlg;

		// Token: 0x0400280E RID: 10254
		public readonly int DataKeySize;

		// Token: 0x0400280F RID: 10255
		public readonly int DataHashAlg;

		// Token: 0x04002810 RID: 10256
		public readonly int DataHashKeySize;

		// Token: 0x04002811 RID: 10257
		public readonly int KeyExchangeAlg;

		// Token: 0x04002812 RID: 10258
		public readonly int KeyExchKeySize;
	}
}
