using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002F0 RID: 752
	[TypeLibImportClass(typeof(AssemblyName))]
	[Guid("B42B6AAC-317E-34D5-9FA9-093BB4160C50")]
	[ComVisible(true)]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface _AssemblyName
	{
		// Token: 0x06001D21 RID: 7457
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001D22 RID: 7458
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001D23 RID: 7459
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001D24 RID: 7460
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
