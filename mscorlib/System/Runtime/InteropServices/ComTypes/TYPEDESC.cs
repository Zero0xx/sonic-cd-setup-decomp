using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000589 RID: 1417
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEDESC
	{
		// Token: 0x04001B7E RID: 7038
		public IntPtr lpValue;

		// Token: 0x04001B7F RID: 7039
		public short vt;
	}
}
