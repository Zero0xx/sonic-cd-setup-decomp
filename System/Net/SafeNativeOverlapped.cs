using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000525 RID: 1317
	internal class SafeNativeOverlapped : SafeHandle
	{
		// Token: 0x06002861 RID: 10337 RVA: 0x000A7443 File Offset: 0x000A6443
		internal SafeNativeOverlapped() : this(IntPtr.Zero)
		{
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000A7450 File Offset: 0x000A6450
		internal unsafe SafeNativeOverlapped(NativeOverlapped* handle) : this((IntPtr)((void*)handle))
		{
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000A745E File Offset: 0x000A645E
		internal SafeNativeOverlapped(IntPtr handle) : base(IntPtr.Zero, true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002864 RID: 10340 RVA: 0x000A7473 File Offset: 0x000A6473
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000A7488 File Offset: 0x000A6488
		protected unsafe override bool ReleaseHandle()
		{
			IntPtr intPtr = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
			if (intPtr != IntPtr.Zero && !NclUtilities.HasShutdownStarted)
			{
				Overlapped.Free((NativeOverlapped*)((void*)intPtr));
			}
			return true;
		}

		// Token: 0x0400277E RID: 10110
		internal static readonly SafeNativeOverlapped Zero = new SafeNativeOverlapped();
	}
}
