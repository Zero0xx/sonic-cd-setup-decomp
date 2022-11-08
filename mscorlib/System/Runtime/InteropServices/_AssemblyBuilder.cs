using System;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices
{
	// Token: 0x020007FF RID: 2047
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("BEBB2505-8B54-3443-AEAD-142A16DD9CC7")]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(AssemblyBuilder))]
	public interface _AssemblyBuilder
	{
		// Token: 0x06004861 RID: 18529
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004862 RID: 18530
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004863 RID: 18531
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004864 RID: 18532
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
