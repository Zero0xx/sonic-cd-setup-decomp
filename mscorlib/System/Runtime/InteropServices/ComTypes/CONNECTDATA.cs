using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000570 RID: 1392
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CONNECTDATA
	{
		// Token: 0x04001B14 RID: 6932
		[MarshalAs(UnmanagedType.Interface)]
		public object pUnk;

		// Token: 0x04001B15 RID: 6933
		public int dwCookie;
	}
}
