using System;

namespace System.Net
{
	// Token: 0x02000559 RID: 1369
	internal static class Win32
	{
		// Token: 0x04002878 RID: 10360
		internal const int OverlappedInternalOffset = 0;

		// Token: 0x04002879 RID: 10361
		internal static int OverlappedInternalHighOffset = IntPtr.Size;

		// Token: 0x0400287A RID: 10362
		internal static int OverlappedOffsetOffset = IntPtr.Size * 2;

		// Token: 0x0400287B RID: 10363
		internal static int OverlappedOffsetHighOffset = IntPtr.Size * 2 + 4;

		// Token: 0x0400287C RID: 10364
		internal static int OverlappedhEventOffset = IntPtr.Size * 2 + 8;

		// Token: 0x0400287D RID: 10365
		internal static int OverlappedSize = IntPtr.Size * 3 + 8;
	}
}
