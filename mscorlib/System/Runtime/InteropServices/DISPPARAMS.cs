using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200055B RID: 1371
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.DISPPARAMS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DISPPARAMS
	{
		// Token: 0x04001AC1 RID: 6849
		public IntPtr rgvarg;

		// Token: 0x04001AC2 RID: 6850
		public IntPtr rgdispidNamedArgs;

		// Token: 0x04001AC3 RID: 6851
		public int cArgs;

		// Token: 0x04001AC4 RID: 6852
		public int cNamedArgs;
	}
}
