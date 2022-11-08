using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000588 RID: 1416
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct PARAMDESC
	{
		// Token: 0x04001B7C RID: 7036
		public IntPtr lpVarValue;

		// Token: 0x04001B7D RID: 7037
		public PARAMFLAG wParamFlags;
	}
}
