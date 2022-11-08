using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200080A RID: 2058
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("AADABA99-895D-3D65-9760-B1F12621FAE8")]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(EventBuilder))]
	public interface _EventBuilder
	{
		// Token: 0x060048E2 RID: 18658
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060048E3 RID: 18659
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060048E4 RID: 18660
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060048E5 RID: 18661
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
