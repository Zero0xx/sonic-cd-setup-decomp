using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000586 RID: 1414
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct IDLDESC
	{
		// Token: 0x04001B71 RID: 7025
		public IntPtr dwReserved;

		// Token: 0x04001B72 RID: 7026
		public IDLFLAG wIDLFlags;
	}
}
