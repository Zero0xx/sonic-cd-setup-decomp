using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056F RID: 1391
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000102-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IEnumMoniker
	{
		// Token: 0x060033D0 RID: 13264
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] IMoniker[] rgelt, IntPtr pceltFetched);

		// Token: 0x060033D1 RID: 13265
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x060033D2 RID: 13266
		void Reset();

		// Token: 0x060033D3 RID: 13267
		void Clone(out IEnumMoniker ppenum);
	}
}
