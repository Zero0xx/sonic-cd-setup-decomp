using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000160 RID: 352
	[ComVisible(true)]
	public struct NativeOverlapped
	{
		// Token: 0x04000668 RID: 1640
		public IntPtr InternalLow;

		// Token: 0x04000669 RID: 1641
		public IntPtr InternalHigh;

		// Token: 0x0400066A RID: 1642
		public int OffsetLow;

		// Token: 0x0400066B RID: 1643
		public int OffsetHigh;

		// Token: 0x0400066C RID: 1644
		public IntPtr EventHandle;
	}
}
