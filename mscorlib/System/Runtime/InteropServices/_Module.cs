using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002FF RID: 767
	[Guid("D002E9BA-D9E3-3749-B1D3-D565A08B13E7")]
	[CLSCompliant(false)]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(Module))]
	public interface _Module
	{
		// Token: 0x06001E27 RID: 7719
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001E28 RID: 7720
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001E29 RID: 7721
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001E2A RID: 7722
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
