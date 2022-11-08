using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000814 RID: 2068
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(TypeBuilder))]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Guid("7E5678EE-48B3-3F83-B076-C58543498A58")]
	public interface _TypeBuilder
	{
		// Token: 0x0600490A RID: 18698
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x0600490B RID: 18699
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x0600490C RID: 18700
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600490D RID: 18701
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
