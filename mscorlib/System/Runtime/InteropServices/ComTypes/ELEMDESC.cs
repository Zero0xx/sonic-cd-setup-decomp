using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200058A RID: 1418
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ELEMDESC
	{
		// Token: 0x04001B80 RID: 7040
		public TYPEDESC tdesc;

		// Token: 0x04001B81 RID: 7041
		public ELEMDESC.DESCUNION desc;

		// Token: 0x0200058B RID: 1419
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04001B82 RID: 7042
			[FieldOffset(0)]
			public IDLDESC idldesc;

			// Token: 0x04001B83 RID: 7043
			[FieldOffset(0)]
			public PARAMDESC paramdesc;
		}
	}
}
