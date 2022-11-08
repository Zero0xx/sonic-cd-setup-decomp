using System;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000087 RID: 135
	[TypeLibImportClass(typeof(Thread))]
	[Guid("C281C7F1-4AA9-3517-961A-463CFED57E75")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	public interface _Thread
	{
		// Token: 0x0600076E RID: 1902
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x0600076F RID: 1903
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06000770 RID: 1904
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06000771 RID: 1905
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}
