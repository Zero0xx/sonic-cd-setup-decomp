using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200057A RID: 1402
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000010-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IRunningObjectTable
	{
		// Token: 0x0600340E RID: 13326
		int Register(int grfFlags, [MarshalAs(UnmanagedType.Interface)] object punkObject, IMoniker pmkObjectName);

		// Token: 0x0600340F RID: 13327
		void Revoke(int dwRegister);

		// Token: 0x06003410 RID: 13328
		[PreserveSig]
		int IsRunning(IMoniker pmkObjectName);

		// Token: 0x06003411 RID: 13329
		[PreserveSig]
		int GetObject(IMoniker pmkObjectName, [MarshalAs(UnmanagedType.Interface)] out object ppunkObject);

		// Token: 0x06003412 RID: 13330
		void NoteChangeTime(int dwRegister, ref FILETIME pfiletime);

		// Token: 0x06003413 RID: 13331
		[PreserveSig]
		int GetTimeOfLastChange(IMoniker pmkObjectName, out FILETIME pfiletime);

		// Token: 0x06003414 RID: 13332
		void EnumRunning(out IEnumMoniker ppenumMoniker);
	}
}
