using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200018C RID: 396
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000100-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IEnumUnknown
	{
		// Token: 0x06001432 RID: 5170
		[PreserveSig]
		int Next(uint celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown)] [Out] object[] rgelt, ref uint celtFetched);

		// Token: 0x06001433 RID: 5171
		[PreserveSig]
		int Skip(uint celt);

		// Token: 0x06001434 RID: 5172
		[PreserveSig]
		int Reset();

		// Token: 0x06001435 RID: 5173
		[PreserveSig]
		int Clone(out IEnumUnknown enumUnknown);
	}
}
