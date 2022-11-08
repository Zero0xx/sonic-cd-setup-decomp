using System;
using System.Security;

namespace System.Net
{
	// Token: 0x0200051E RID: 1310
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCredential_SECURITY : SafeFreeCredentials
	{
		// Token: 0x06002849 RID: 10313 RVA: 0x000A5F61 File Offset: 0x000A4F61
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeCredentialsHandle(ref this._handle) == 0;
		}

		// Token: 0x04002774 RID: 10100
		private const string SECURITY = "security.Dll";
	}
}
