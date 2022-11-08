using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000045 RID: 69
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(Activator))]
	[ComVisible(true)]
	[Guid("03973551-57A1-3900-A2B5-9083E3FF2943")]
	public interface _Activator
	{
		// Token: 0x060003EE RID: 1006
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060003EF RID: 1007
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060003F0 RID: 1008
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060003F1 RID: 1009
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
