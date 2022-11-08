using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000516 RID: 1302
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeGlobalFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002828 RID: 10280 RVA: 0x000A599C File Offset: 0x000A499C
		private SafeGlobalFree() : base(true)
		{
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x000A59A5 File Offset: 0x000A49A5
		private SafeGlobalFree(bool ownsHandle) : base(ownsHandle)
		{
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000A59AE File Offset: 0x000A49AE
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.GlobalFree(this.handle) == IntPtr.Zero;
		}
	}
}
