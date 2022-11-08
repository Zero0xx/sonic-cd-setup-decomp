using System;

namespace System.Net
{
	// Token: 0x02000502 RID: 1282
	[Flags]
	internal enum SocketConstructorFlags
	{
		// Token: 0x04002731 RID: 10033
		WSA_FLAG_OVERLAPPED = 1,
		// Token: 0x04002732 RID: 10034
		WSA_FLAG_MULTIPOINT_C_ROOT = 2,
		// Token: 0x04002733 RID: 10035
		WSA_FLAG_MULTIPOINT_C_LEAF = 4,
		// Token: 0x04002734 RID: 10036
		WSA_FLAG_MULTIPOINT_D_ROOT = 8,
		// Token: 0x04002735 RID: 10037
		WSA_FLAG_MULTIPOINT_D_LEAF = 16
	}
}
