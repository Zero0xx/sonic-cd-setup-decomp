using System;
using System.Security;

namespace System.Net
{
	// Token: 0x0200052B RID: 1323
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBufferChannelBinding_SECURITY : SafeFreeContextBufferChannelBinding
	{
		// Token: 0x0600288B RID: 10379 RVA: 0x000A7C28 File Offset: 0x000A6C28
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeContextBuffer(this.handle) == 0;
		}
	}
}
