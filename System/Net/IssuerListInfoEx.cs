using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000403 RID: 1027
	internal struct IssuerListInfoEx
	{
		// Token: 0x060020AA RID: 8362 RVA: 0x00080C20 File Offset: 0x0007FC20
		public unsafe IssuerListInfoEx(SafeHandle handle, byte[] nativeBuffer)
		{
			this.aIssuers = handle;
			fixed (byte* ptr = nativeBuffer)
			{
				this.cIssuers = ((uint*)ptr)[IntPtr.Size / 4];
			}
		}

		// Token: 0x0400206D RID: 8301
		public SafeHandle aIssuers;

		// Token: 0x0400206E RID: 8302
		public uint cIssuers;
	}
}
