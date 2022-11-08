using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200057E RID: 1406
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	public struct BINDPTR
	{
		// Token: 0x04001B2A RID: 6954
		[FieldOffset(0)]
		public IntPtr lpfuncdesc;

		// Token: 0x04001B2B RID: 6955
		[FieldOffset(0)]
		public IntPtr lpvardesc;

		// Token: 0x04001B2C RID: 6956
		[FieldOffset(0)]
		public IntPtr lptcomp;
	}
}
