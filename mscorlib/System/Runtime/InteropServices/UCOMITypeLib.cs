using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000566 RID: 1382
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.ITypeLib instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020402-0000-0000-C000-000000000046")]
	[ComImport]
	public interface UCOMITypeLib
	{
		// Token: 0x060033AD RID: 13229
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x060033AE RID: 13230
		void GetTypeInfo(int index, out UCOMITypeInfo ppTI);

		// Token: 0x060033AF RID: 13231
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x060033B0 RID: 13232
		void GetTypeInfoOfGuid(ref Guid guid, out UCOMITypeInfo ppTInfo);

		// Token: 0x060033B1 RID: 13233
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x060033B2 RID: 13234
		void GetTypeComp(out UCOMITypeComp ppTComp);

		// Token: 0x060033B3 RID: 13235
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x060033B4 RID: 13236
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x060033B5 RID: 13237
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] UCOMITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x060033B6 RID: 13238
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);
	}
}
