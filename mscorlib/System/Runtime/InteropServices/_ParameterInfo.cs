using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002FE RID: 766
	[CLSCompliant(false)]
	[Guid("993634C4-E47A-32CC-BE08-85F567DC27D6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(ParameterInfo))]
	[ComVisible(true)]
	public interface _ParameterInfo
	{
		// Token: 0x06001E23 RID: 7715
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001E24 RID: 7716
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001E25 RID: 7717
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001E26 RID: 7718
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
