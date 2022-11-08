using System;
using System.Security;

namespace System.Net
{
	// Token: 0x0200052C RID: 1324
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBufferChannelBinding_SCHANNEL : SafeFreeContextBufferChannelBinding
	{
		// Token: 0x0600288D RID: 10381 RVA: 0x000A7C40 File Offset: 0x000A6C40
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.FreeContextBuffer(this.handle) == 0;
		}
	}
}
