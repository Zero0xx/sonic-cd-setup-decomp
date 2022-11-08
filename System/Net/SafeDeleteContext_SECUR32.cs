using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000523 RID: 1315
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeDeleteContext_SECUR32 : SafeDeleteContext
	{
		// Token: 0x0600285D RID: 10333 RVA: 0x000A73ED File Offset: 0x000A63ED
		internal SafeDeleteContext_SECUR32()
		{
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000A73F5 File Offset: 0x000A63F5
		protected override bool ReleaseHandle()
		{
			if (this._EffectiveCredential != null)
			{
				this._EffectiveCredential.DangerousRelease();
			}
			return UnsafeNclNativeMethods.SafeNetHandles_SECUR32.DeleteSecurityContext(ref this._handle) == 0;
		}

		// Token: 0x0400277C RID: 10108
		private const string SECUR32 = "secur32.Dll";
	}
}
