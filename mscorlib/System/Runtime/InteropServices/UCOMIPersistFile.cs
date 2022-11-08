using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000546 RID: 1350
	[Guid("0000010b-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IPersistFile instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[ComImport]
	public interface UCOMIPersistFile
	{
		// Token: 0x06003380 RID: 13184
		void GetClassID(out Guid pClassID);

		// Token: 0x06003381 RID: 13185
		[PreserveSig]
		int IsDirty();

		// Token: 0x06003382 RID: 13186
		void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

		// Token: 0x06003383 RID: 13187
		void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06003384 RID: 13188
		void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		// Token: 0x06003385 RID: 13189
		void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
	}
}
