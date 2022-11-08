using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000513 RID: 1299
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBuffer_SCHANNEL : SafeFreeContextBuffer
	{
		// Token: 0x0600281F RID: 10271 RVA: 0x000A5904 File Offset: 0x000A4904
		internal SafeFreeContextBuffer_SCHANNEL()
		{
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000A590C File Offset: 0x000A490C
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.FreeContextBuffer(this.handle) == 0;
		}

		// Token: 0x04002762 RID: 10082
		private const string SCHANNEL = "schannel.dll";
	}
}
