using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000556 RID: 1366
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEDESC
	{
		// Token: 0x04001AB4 RID: 6836
		public IntPtr lpValue;

		// Token: 0x04001AB5 RID: 6837
		public short vt;
	}
}
