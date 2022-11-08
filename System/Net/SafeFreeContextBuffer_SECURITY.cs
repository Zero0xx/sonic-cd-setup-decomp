using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000512 RID: 1298
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBuffer_SECURITY : SafeFreeContextBuffer
	{
		// Token: 0x0600281D RID: 10269 RVA: 0x000A58EC File Offset: 0x000A48EC
		internal SafeFreeContextBuffer_SECURITY()
		{
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000A58F4 File Offset: 0x000A48F4
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeContextBuffer(this.handle) == 0;
		}

		// Token: 0x04002761 RID: 10081
		private const string SECURITY = "security.dll";
	}
}
