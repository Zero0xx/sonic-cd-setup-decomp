using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053D RID: 1341
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.CONNECTDATA instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CONNECTDATA
	{
		// Token: 0x04001A4A RID: 6730
		[MarshalAs(UnmanagedType.Interface)]
		public object pUnk;

		// Token: 0x04001A4B RID: 6731
		public int dwCookie;
	}
}
