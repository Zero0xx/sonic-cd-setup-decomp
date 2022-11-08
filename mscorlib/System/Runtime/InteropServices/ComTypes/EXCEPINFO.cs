using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000590 RID: 1424
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EXCEPINFO
	{
		// Token: 0x04001B95 RID: 7061
		public short wCode;

		// Token: 0x04001B96 RID: 7062
		public short wReserved;

		// Token: 0x04001B97 RID: 7063
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrSource;

		// Token: 0x04001B98 RID: 7064
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrDescription;

		// Token: 0x04001B99 RID: 7065
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrHelpFile;

		// Token: 0x04001B9A RID: 7066
		public int dwHelpContext;

		// Token: 0x04001B9B RID: 7067
		public IntPtr pvReserved;

		// Token: 0x04001B9C RID: 7068
		public IntPtr pfnDeferredFillIn;

		// Token: 0x04001B9D RID: 7069
		public int scode;
	}
}
