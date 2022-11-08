using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053C RID: 1340
	[Guid("00000102-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumMoniker instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[ComImport]
	public interface UCOMIEnumMoniker
	{
		// Token: 0x06003348 RID: 13128
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] UCOMIMoniker[] rgelt, out int pceltFetched);

		// Token: 0x06003349 RID: 13129
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600334A RID: 13130
		[PreserveSig]
		int Reset();

		// Token: 0x0600334B RID: 13131
		void Clone(out UCOMIEnumMoniker ppenum);
	}
}
