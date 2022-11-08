using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000573 RID: 1395
	[Guid("00000101-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumString
	{
		// Token: 0x060033DC RID: 13276
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] [Out] string[] rgelt, IntPtr pceltFetched);

		// Token: 0x060033DD RID: 13277
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x060033DE RID: 13278
		void Reset();

		// Token: 0x060033DF RID: 13279
		void Clone(out IEnumString ppenum);
	}
}
