using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200058F RID: 1423
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DISPPARAMS
	{
		// Token: 0x04001B91 RID: 7057
		public IntPtr rgvarg;

		// Token: 0x04001B92 RID: 7058
		public IntPtr rgdispidNamedArgs;

		// Token: 0x04001B93 RID: 7059
		public int cArgs;

		// Token: 0x04001B94 RID: 7060
		public int cNamedArgs;
	}
}
