using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000553 RID: 1363
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IDLDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct IDLDESC
	{
		// Token: 0x04001AA7 RID: 6823
		public int dwReserved;

		// Token: 0x04001AA8 RID: 6824
		public IDLFLAG wIDLFlags;
	}
}
