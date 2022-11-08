using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000540 RID: 1344
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumString instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("00000101-0000-0000-C000-000000000046")]
	[ComImport]
	public interface UCOMIEnumString
	{
		// Token: 0x06003354 RID: 13140
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] [Out] string[] rgelt, out int pceltFetched);

		// Token: 0x06003355 RID: 13141
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06003356 RID: 13142
		[PreserveSig]
		int Reset();

		// Token: 0x06003357 RID: 13143
		void Clone(out UCOMIEnumString ppenum);
	}
}
