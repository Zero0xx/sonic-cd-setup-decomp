using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053F RID: 1343
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumConnectionPoints instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("B196B285-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface UCOMIEnumConnectionPoints
	{
		// Token: 0x06003350 RID: 13136
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] UCOMIConnectionPoint[] rgelt, out int pceltFetched);

		// Token: 0x06003351 RID: 13137
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06003352 RID: 13138
		[PreserveSig]
		int Reset();

		// Token: 0x06003353 RID: 13139
		void Clone(out UCOMIEnumConnectionPoints ppenum);
	}
}
