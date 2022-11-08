using System;

namespace System.Net
{
	// Token: 0x020003E8 RID: 1000
	internal static class IntPtrHelper
	{
		// Token: 0x0600206A RID: 8298 RVA: 0x0007FB88 File Offset: 0x0007EB88
		internal static IntPtr Add(IntPtr a, int b)
		{
			return (IntPtr)((long)a + (long)b);
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x0007FB98 File Offset: 0x0007EB98
		internal static long Subtract(IntPtr a, IntPtr b)
		{
			return (long)a - (long)b;
		}
	}
}
