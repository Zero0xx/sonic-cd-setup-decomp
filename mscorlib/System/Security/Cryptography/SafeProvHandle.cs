using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x020008A7 RID: 2215
	internal sealed class SafeProvHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600508C RID: 20620 RVA: 0x0011FD56 File Offset: 0x0011ED56
		private SafeProvHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x0600508D RID: 20621 RVA: 0x0011FD66 File Offset: 0x0011ED66
		internal static SafeProvHandle InvalidHandle
		{
			get
			{
				return new SafeProvHandle(IntPtr.Zero);
			}
		}

		// Token: 0x0600508E RID: 20622
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreeCSP(IntPtr pProvCtx);

		// Token: 0x0600508F RID: 20623 RVA: 0x0011FD72 File Offset: 0x0011ED72
		protected override bool ReleaseHandle()
		{
			SafeProvHandle._FreeCSP(this.handle);
			return true;
		}
	}
}
