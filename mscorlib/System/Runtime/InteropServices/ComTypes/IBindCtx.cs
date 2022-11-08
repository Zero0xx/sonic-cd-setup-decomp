using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056A RID: 1386
	[Guid("0000000e-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IBindCtx
	{
		// Token: 0x060033BB RID: 13243
		void RegisterObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x060033BC RID: 13244
		void RevokeObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x060033BD RID: 13245
		void ReleaseBoundObjects();

		// Token: 0x060033BE RID: 13246
		void SetBindOptions([In] ref BIND_OPTS pbindopts);

		// Token: 0x060033BF RID: 13247
		void GetBindOptions(ref BIND_OPTS pbindopts);

		// Token: 0x060033C0 RID: 13248
		void GetRunningObjectTable(out IRunningObjectTable pprot);

		// Token: 0x060033C1 RID: 13249
		void RegisterObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x060033C2 RID: 13250
		void GetObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

		// Token: 0x060033C3 RID: 13251
		void EnumObjectParam(out IEnumString ppenum);

		// Token: 0x060033C4 RID: 13252
		[PreserveSig]
		int RevokeObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey);
	}
}
