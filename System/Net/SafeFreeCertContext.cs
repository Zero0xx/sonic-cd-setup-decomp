using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200051A RID: 1306
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCertContext : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600283A RID: 10298 RVA: 0x000A5B5C File Offset: 0x000A4B5C
		internal SafeFreeCertContext() : base(true)
		{
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000A5B65 File Offset: 0x000A4B65
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Set(IntPtr value)
		{
			this.handle = value;
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000A5B6E File Offset: 0x000A4B6E
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandles.CertFreeCertificateContext(this.handle);
			return true;
		}

		// Token: 0x0400276D RID: 10093
		private const string CRYPT32 = "crypt32.dll";

		// Token: 0x0400276E RID: 10094
		private const string ADVAPI32 = "advapi32.dll";

		// Token: 0x0400276F RID: 10095
		private const uint CRYPT_ACQUIRE_SILENT_FLAG = 64U;
	}
}
