using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000557 RID: 1367
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.ELEMDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ELEMDESC
	{
		// Token: 0x04001AB6 RID: 6838
		public TYPEDESC tdesc;

		// Token: 0x04001AB7 RID: 6839
		public ELEMDESC.DESCUNION desc;

		// Token: 0x02000558 RID: 1368
		[ComVisible(false)]
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04001AB8 RID: 6840
			[FieldOffset(0)]
			public IDLDESC idldesc;

			// Token: 0x04001AB9 RID: 6841
			[FieldOffset(0)]
			public PARAMDESC paramdesc;
		}
	}
}
