using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000809 RID: 2057
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C7BD73DE-9F85-3290-88EE-090B8BDFE2DF")]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(EnumBuilder))]
	[ComVisible(true)]
	public interface _EnumBuilder
	{
		// Token: 0x060048DE RID: 18654
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060048DF RID: 18655
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060048E0 RID: 18656
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060048E1 RID: 18657
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
