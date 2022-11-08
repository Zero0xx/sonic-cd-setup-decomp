using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000519 RID: 1305
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCertChain : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002837 RID: 10295 RVA: 0x000A5B12 File Offset: 0x000A4B12
		internal SafeFreeCertChain(IntPtr handle) : base(false)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000A5B24 File Offset: 0x000A4B24
		public override string ToString()
		{
			return "0x" + base.DangerousGetHandle().ToString("x");
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000A5B4E File Offset: 0x000A4B4E
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandles.CertFreeCertificateChain(this.handle);
			return true;
		}

		// Token: 0x0400276C RID: 10092
		private const string CRYPT32 = "crypt32.dll";
	}
}
