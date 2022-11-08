using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000571 RID: 1393
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B287-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IEnumConnections
	{
		// Token: 0x060033D4 RID: 13268
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] CONNECTDATA[] rgelt, IntPtr pceltFetched);

		// Token: 0x060033D5 RID: 13269
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x060033D6 RID: 13270
		void Reset();

		// Token: 0x060033D7 RID: 13271
		void Clone(out IEnumConnections ppenum);
	}
}
