using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000520 RID: 1312
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCredential_SCHANNEL : SafeFreeCredentials
	{
		// Token: 0x0600284D RID: 10317 RVA: 0x000A5F91 File Offset: 0x000A4F91
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.FreeCredentialsHandle(ref this._handle) == 0;
		}

		// Token: 0x04002776 RID: 10102
		private const string SCHANNEL = "schannel.Dll";
	}
}
