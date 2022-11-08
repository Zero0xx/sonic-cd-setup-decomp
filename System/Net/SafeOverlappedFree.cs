using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000517 RID: 1303
	[ComVisible(false)]
	internal sealed class SafeOverlappedFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600282B RID: 10283 RVA: 0x000A59C5 File Offset: 0x000A49C5
		private SafeOverlappedFree() : base(true)
		{
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000A59CE File Offset: 0x000A49CE
		private SafeOverlappedFree(bool ownsHandle) : base(ownsHandle)
		{
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000A59D8 File Offset: 0x000A49D8
		public static SafeOverlappedFree Alloc()
		{
			SafeOverlappedFree safeOverlappedFree = UnsafeNclNativeMethods.SafeNetHandlesSafeOverlappedFree.LocalAlloc(64, (UIntPtr)((ulong)((long)Win32.OverlappedSize)));
			if (safeOverlappedFree.IsInvalid)
			{
				safeOverlappedFree.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			return safeOverlappedFree;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000A5A10 File Offset: 0x000A4A10
		public static SafeOverlappedFree Alloc(SafeCloseSocket socketHandle)
		{
			SafeOverlappedFree safeOverlappedFree = SafeOverlappedFree.Alloc();
			safeOverlappedFree._socketHandle = socketHandle;
			return safeOverlappedFree;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000A5A2C File Offset: 0x000A4A2C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Close(bool resetOwner)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (resetOwner)
				{
					this._socketHandle = null;
				}
				base.Close();
			}
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000A5A64 File Offset: 0x000A4A64
		protected override bool ReleaseHandle()
		{
			SafeCloseSocket socketHandle = this._socketHandle;
			if (socketHandle != null && !socketHandle.IsInvalid)
			{
				socketHandle.Dispose();
			}
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x04002767 RID: 10087
		private const int LPTR = 64;

		// Token: 0x04002768 RID: 10088
		internal static readonly SafeOverlappedFree Zero = new SafeOverlappedFree(false);

		// Token: 0x04002769 RID: 10089
		private SafeCloseSocket _socketHandle;
	}
}
