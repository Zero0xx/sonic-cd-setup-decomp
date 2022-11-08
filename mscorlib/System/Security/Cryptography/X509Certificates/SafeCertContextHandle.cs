using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008BB RID: 2235
	internal sealed class SafeCertContextHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06005154 RID: 20820 RVA: 0x00123D0A File Offset: 0x00122D0A
		private SafeCertContextHandle() : base(true)
		{
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x00123D13 File Offset: 0x00122D13
		internal SafeCertContextHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06005156 RID: 20822 RVA: 0x00123D23 File Offset: 0x00122D23
		internal static SafeCertContextHandle InvalidHandle
		{
			get
			{
				return new SafeCertContextHandle(IntPtr.Zero);
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06005157 RID: 20823 RVA: 0x00123D2F File Offset: 0x00122D2F
		internal IntPtr pCertContext
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					return IntPtr.Zero;
				}
				return Marshal.ReadIntPtr(this.handle);
			}
		}

		// Token: 0x06005158 RID: 20824
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreePCertContext(IntPtr pCert);

		// Token: 0x06005159 RID: 20825 RVA: 0x00123D54 File Offset: 0x00122D54
		protected override bool ReleaseHandle()
		{
			SafeCertContextHandle._FreePCertContext(this.handle);
			return true;
		}
	}
}
