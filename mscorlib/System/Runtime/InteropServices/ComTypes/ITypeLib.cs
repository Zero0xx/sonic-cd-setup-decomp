using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200059A RID: 1434
	[Guid("00020402-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface ITypeLib
	{
		// Token: 0x06003435 RID: 13365
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x06003436 RID: 13366
		void GetTypeInfo(int index, out ITypeInfo ppTI);

		// Token: 0x06003437 RID: 13367
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x06003438 RID: 13368
		void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);

		// Token: 0x06003439 RID: 13369
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x0600343A RID: 13370
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x0600343B RID: 13371
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x0600343C RID: 13372
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x0600343D RID: 13373
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x0600343E RID: 13374
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);
	}
}
