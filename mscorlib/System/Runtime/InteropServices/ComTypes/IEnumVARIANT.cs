using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000574 RID: 1396
	[Guid("00020404-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumVARIANT
	{
		// Token: 0x060033E0 RID: 13280
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] object[] rgVar, IntPtr pceltFetched);

		// Token: 0x060033E1 RID: 13281
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x060033E2 RID: 13282
		[PreserveSig]
		int Reset();

		// Token: 0x060033E3 RID: 13283
		IEnumVARIANT Clone();
	}
}
