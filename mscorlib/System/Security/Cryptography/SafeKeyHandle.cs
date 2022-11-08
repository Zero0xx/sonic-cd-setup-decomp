using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x020008A8 RID: 2216
	internal sealed class SafeKeyHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06005090 RID: 20624 RVA: 0x0011FD80 File Offset: 0x0011ED80
		private SafeKeyHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06005091 RID: 20625 RVA: 0x0011FD90 File Offset: 0x0011ED90
		internal static SafeKeyHandle InvalidHandle
		{
			get
			{
				return new SafeKeyHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06005092 RID: 20626
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreeHKey(IntPtr pKeyCtx);

		// Token: 0x06005093 RID: 20627 RVA: 0x0011FD9C File Offset: 0x0011ED9C
		protected override bool ReleaseHandle()
		{
			SafeKeyHandle._FreeHKey(this.handle);
			return true;
		}
	}
}
