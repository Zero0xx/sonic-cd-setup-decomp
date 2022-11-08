using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200059B RID: 1435
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020411-0000-0000-C000-000000000046")]
	[ComImport]
	public interface ITypeLib2 : ITypeLib
	{
		// Token: 0x0600343F RID: 13375
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x06003440 RID: 13376
		void GetTypeInfo(int index, out ITypeInfo ppTI);

		// Token: 0x06003441 RID: 13377
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x06003442 RID: 13378
		void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);

		// Token: 0x06003443 RID: 13379
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x06003444 RID: 13380
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x06003445 RID: 13381
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x06003446 RID: 13382
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x06003447 RID: 13383
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x06003448 RID: 13384
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);

		// Token: 0x06003449 RID: 13385
		void GetCustData(ref Guid guid, out object pVarVal);

		// Token: 0x0600344A RID: 13386
		[LCIDConversion(1)]
		void GetDocumentation2(int index, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);

		// Token: 0x0600344B RID: 13387
		void GetLibStatistics(IntPtr pcUniqueNames, out int pcchUniqueNames);

		// Token: 0x0600344C RID: 13388
		void GetAllCustData(IntPtr pCustData);
	}
}
