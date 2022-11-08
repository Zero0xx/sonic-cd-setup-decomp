using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000524 RID: 1316
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeDeleteContext_SCHANNEL : SafeDeleteContext
	{
		// Token: 0x0600285F RID: 10335 RVA: 0x000A7418 File Offset: 0x000A6418
		internal SafeDeleteContext_SCHANNEL()
		{
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000A7420 File Offset: 0x000A6420
		protected override bool ReleaseHandle()
		{
			if (this._EffectiveCredential != null)
			{
				this._EffectiveCredential.DangerousRelease();
			}
			return UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.DeleteSecurityContext(ref this._handle) == 0;
		}

		// Token: 0x0400277D RID: 10109
		private const string SCHANNEL = "schannel.Dll";
	}
}
