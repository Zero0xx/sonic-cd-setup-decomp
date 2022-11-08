using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000514 RID: 1300
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBuffer_SECUR32 : SafeFreeContextBuffer
	{
		// Token: 0x06002821 RID: 10273 RVA: 0x000A591C File Offset: 0x000A491C
		internal SafeFreeContextBuffer_SECUR32()
		{
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000A5924 File Offset: 0x000A4924
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECUR32.FreeContextBuffer(this.handle) == 0;
		}

		// Token: 0x04002763 RID: 10083
		private const string SECUR32 = "secur32.dll";
	}
}
