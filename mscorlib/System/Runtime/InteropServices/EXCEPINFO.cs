using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200055C RID: 1372
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.EXCEPINFO instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EXCEPINFO
	{
		// Token: 0x04001AC5 RID: 6853
		public short wCode;

		// Token: 0x04001AC6 RID: 6854
		public short wReserved;

		// Token: 0x04001AC7 RID: 6855
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrSource;

		// Token: 0x04001AC8 RID: 6856
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrDescription;

		// Token: 0x04001AC9 RID: 6857
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrHelpFile;

		// Token: 0x04001ACA RID: 6858
		public int dwHelpContext;

		// Token: 0x04001ACB RID: 6859
		public IntPtr pvReserved;

		// Token: 0x04001ACC RID: 6860
		public IntPtr pfnDeferredFillIn;
	}
}
