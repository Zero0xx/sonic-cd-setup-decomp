using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000810 RID: 2064
	[Guid("D05FFA9A-04AF-3519-8EE1-8D93AD73430B")]
	[TypeLibImportClass(typeof(ModuleBuilder))]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	public interface _ModuleBuilder
	{
		// Token: 0x060048FA RID: 18682
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060048FB RID: 18683
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060048FC RID: 18684
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060048FD RID: 18685
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
