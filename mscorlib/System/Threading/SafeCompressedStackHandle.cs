using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000149 RID: 329
	internal class SafeCompressedStackHandle : SafeHandle
	{
		// Token: 0x060011F7 RID: 4599 RVA: 0x0003241A File Offset: 0x0003141A
		public SafeCompressedStackHandle() : base(IntPtr.Zero, true)
		{
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x00032428 File Offset: 0x00031428
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0003243A File Offset: 0x0003143A
		protected override bool ReleaseHandle()
		{
			CompressedStack.DestroyDelayedCompressedStack(this.handle);
			this.handle = IntPtr.Zero;
			return true;
		}
	}
}
