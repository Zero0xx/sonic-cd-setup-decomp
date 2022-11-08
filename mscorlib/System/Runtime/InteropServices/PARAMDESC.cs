using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000555 RID: 1365
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.PARAMDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct PARAMDESC
	{
		// Token: 0x04001AB2 RID: 6834
		public IntPtr lpVarValue;

		// Token: 0x04001AB3 RID: 6835
		public PARAMFLAG wParamFlags;
	}
}
