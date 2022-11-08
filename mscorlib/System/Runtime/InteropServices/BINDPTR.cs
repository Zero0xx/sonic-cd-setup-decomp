using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054B RID: 1355
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.BINDPTR instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	public struct BINDPTR
	{
		// Token: 0x04001A60 RID: 6752
		[FieldOffset(0)]
		public IntPtr lpfuncdesc;

		// Token: 0x04001A61 RID: 6753
		[FieldOffset(0)]
		public IntPtr lpvardesc;

		// Token: 0x04001A62 RID: 6754
		[FieldOffset(0)]
		public IntPtr lptcomp;
	}
}
