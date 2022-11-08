using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000522 RID: 1314
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeDeleteContext_SECURITY : SafeDeleteContext
	{
		// Token: 0x0600285B RID: 10331 RVA: 0x000A73C2 File Offset: 0x000A63C2
		internal SafeDeleteContext_SECURITY()
		{
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000A73CA File Offset: 0x000A63CA
		protected override bool ReleaseHandle()
		{
			if (this._EffectiveCredential != null)
			{
				this._EffectiveCredential.DangerousRelease();
			}
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.DeleteSecurityContext(ref this._handle) == 0;
		}

		// Token: 0x0400277B RID: 10107
		private const string SECURITY = "security.Dll";
	}
}
