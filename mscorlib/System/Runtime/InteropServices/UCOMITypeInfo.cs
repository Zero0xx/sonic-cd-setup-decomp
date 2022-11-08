using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000562 RID: 1378
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.ITypeInfo instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("00020401-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMITypeInfo
	{
		// Token: 0x0600339A RID: 13210
		void GetTypeAttr(out IntPtr ppTypeAttr);

		// Token: 0x0600339B RID: 13211
		void GetTypeComp(out UCOMITypeComp ppTComp);

		// Token: 0x0600339C RID: 13212
		void GetFuncDesc(int index, out IntPtr ppFuncDesc);

		// Token: 0x0600339D RID: 13213
		void GetVarDesc(int index, out IntPtr ppVarDesc);

		// Token: 0x0600339E RID: 13214
		void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] string[] rgBstrNames, int cMaxNames, out int pcNames);

		// Token: 0x0600339F RID: 13215
		void GetRefTypeOfImplType(int index, out int href);

		// Token: 0x060033A0 RID: 13216
		void GetImplTypeFlags(int index, out int pImplTypeFlags);

		// Token: 0x060033A1 RID: 13217
		void GetIDsOfNames([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] [In] string[] rgszNames, int cNames, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] int[] pMemId);

		// Token: 0x060033A2 RID: 13218
		void Invoke([MarshalAs(UnmanagedType.IUnknown)] object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, out object pVarResult, out EXCEPINFO pExcepInfo, out int puArgErr);

		// Token: 0x060033A3 RID: 13219
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x060033A4 RID: 13220
		void GetDllEntry(int memid, INVOKEKIND invKind, out string pBstrDllName, out string pBstrName, out short pwOrdinal);

		// Token: 0x060033A5 RID: 13221
		void GetRefTypeInfo(int hRef, out UCOMITypeInfo ppTI);

		// Token: 0x060033A6 RID: 13222
		void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

		// Token: 0x060033A7 RID: 13223
		void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

		// Token: 0x060033A8 RID: 13224
		void GetMops(int memid, out string pBstrMops);

		// Token: 0x060033A9 RID: 13225
		void GetContainingTypeLib(out UCOMITypeLib ppTLB, out int pIndex);

		// Token: 0x060033AA RID: 13226
		void ReleaseTypeAttr(IntPtr pTypeAttr);

		// Token: 0x060033AB RID: 13227
		void ReleaseFuncDesc(IntPtr pFuncDesc);

		// Token: 0x060033AC RID: 13228
		void ReleaseVarDesc(IntPtr pVarDesc);
	}
}
