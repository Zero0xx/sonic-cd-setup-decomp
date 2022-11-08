using System;
using System.Security;

namespace System.Net
{
	// Token: 0x0200052D RID: 1325
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBufferChannelBinding_SECUR32 : SafeFreeContextBufferChannelBinding
	{
		// Token: 0x0600288F RID: 10383 RVA: 0x000A7C58 File Offset: 0x000A6C58
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECUR32.FreeContextBuffer(this.handle) == 0;
		}
	}
}
