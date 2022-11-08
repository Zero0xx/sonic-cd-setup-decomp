using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053E RID: 1342
	[Guid("B196B287-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumConnections instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[ComImport]
	public interface UCOMIEnumConnections
	{
		// Token: 0x0600334C RID: 13132
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] CONNECTDATA[] rgelt, out int pceltFetched);

		// Token: 0x0600334D RID: 13133
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600334E RID: 13134
		[PreserveSig]
		void Reset();

		// Token: 0x0600334F RID: 13135
		void Clone(out UCOMIEnumConnections ppenum);
	}
}
