using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x020008A9 RID: 2217
	internal sealed class SafeHashHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06005094 RID: 20628 RVA: 0x0011FDAA File Offset: 0x0011EDAA
		private SafeHashHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06005095 RID: 20629 RVA: 0x0011FDBA File Offset: 0x0011EDBA
		internal static SafeHashHandle InvalidHandle
		{
			get
			{
				return new SafeHashHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06005096 RID: 20630
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreeHash(IntPtr pHashCtx);

		// Token: 0x06005097 RID: 20631 RVA: 0x0011FDC6 File Offset: 0x0011EDC6
		protected override bool ReleaseHandle()
		{
			SafeHashHandle._FreeHash(this.handle);
			return true;
		}
	}
}
