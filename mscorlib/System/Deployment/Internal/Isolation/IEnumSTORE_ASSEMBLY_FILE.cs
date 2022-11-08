using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F9 RID: 505
	[Guid("a5c6aaa3-03e4-478d-b9f5-2e45908d5e4f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_FILE
	{
		// Token: 0x06001539 RID: 5433
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY_FILE[] rgelt);

		// Token: 0x0600153A RID: 5434
		void Skip([In] uint celt);

		// Token: 0x0600153B RID: 5435
		void Reset();

		// Token: 0x0600153C RID: 5436
		IEnumSTORE_ASSEMBLY_FILE Clone();
	}
}
