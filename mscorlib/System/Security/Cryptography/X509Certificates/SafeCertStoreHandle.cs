using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008BC RID: 2236
	internal sealed class SafeCertStoreHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600515A RID: 20826 RVA: 0x00123D62 File Offset: 0x00122D62
		private SafeCertStoreHandle() : base(true)
		{
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x00123D6B File Offset: 0x00122D6B
		internal SafeCertStoreHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x0600515C RID: 20828 RVA: 0x00123D7B File Offset: 0x00122D7B
		internal static SafeCertStoreHandle InvalidHandle
		{
			get
			{
				return new SafeCertStoreHandle(IntPtr.Zero);
			}
		}

		// Token: 0x0600515D RID: 20829
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreeCertStoreContext(IntPtr hCertStore);

		// Token: 0x0600515E RID: 20830 RVA: 0x00123D87 File Offset: 0x00122D87
		protected override bool ReleaseHandle()
		{
			SafeCertStoreHandle._FreeCertStoreContext(this.handle);
			return true;
		}
	}
}
