using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000579 RID: 1401
	[Guid("0000010b-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IPersistFile
	{
		// Token: 0x06003408 RID: 13320
		void GetClassID(out Guid pClassID);

		// Token: 0x06003409 RID: 13321
		[PreserveSig]
		int IsDirty();

		// Token: 0x0600340A RID: 13322
		void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

		// Token: 0x0600340B RID: 13323
		void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x0600340C RID: 13324
		void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		// Token: 0x0600340D RID: 13325
		void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
	}
}
