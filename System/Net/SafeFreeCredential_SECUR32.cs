using System;
using System.Security;

namespace System.Net
{
	// Token: 0x0200051F RID: 1311
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCredential_SECUR32 : SafeFreeCredentials
	{
		// Token: 0x0600284B RID: 10315 RVA: 0x000A5F79 File Offset: 0x000A4F79
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECUR32.FreeCredentialsHandle(ref this._handle) == 0;
		}

		// Token: 0x04002775 RID: 10101
		private const string SECUR32 = "secur32.Dll";
	}
}
