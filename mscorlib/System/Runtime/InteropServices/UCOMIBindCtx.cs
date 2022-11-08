using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000537 RID: 1335
	[Guid("0000000e-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IBindCtx instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[ComImport]
	public interface UCOMIBindCtx
	{
		// Token: 0x06003333 RID: 13107
		void RegisterObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x06003334 RID: 13108
		void RevokeObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x06003335 RID: 13109
		void ReleaseBoundObjects();

		// Token: 0x06003336 RID: 13110
		void SetBindOptions([In] ref BIND_OPTS pbindopts);

		// Token: 0x06003337 RID: 13111
		void GetBindOptions(ref BIND_OPTS pbindopts);

		// Token: 0x06003338 RID: 13112
		void GetRunningObjectTable(out UCOMIRunningObjectTable pprot);

		// Token: 0x06003339 RID: 13113
		void RegisterObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] object punk);

		// Token: 0x0600333A RID: 13114
		void GetObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

		// Token: 0x0600333B RID: 13115
		void EnumObjectParam(out UCOMIEnumString ppenum);

		// Token: 0x0600333C RID: 13116
		void RevokeObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey);
	}
}
