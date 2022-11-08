using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200080B RID: 2059
	[Guid("CE1A3BF5-975E-30CC-97C9-1EF70F8F3993")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(FieldBuilder))]
	public interface _FieldBuilder
	{
		// Token: 0x060048E6 RID: 18662
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060048E7 RID: 18663
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060048E8 RID: 18664
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060048E9 RID: 18665
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
