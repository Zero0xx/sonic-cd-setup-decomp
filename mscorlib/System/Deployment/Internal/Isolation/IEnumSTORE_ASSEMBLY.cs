using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F7 RID: 503
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a5c637bf-6eaa-4e5f-b535-55299657e33e")]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY
	{
		// Token: 0x0600152E RID: 5422
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY[] rgelt);

		// Token: 0x0600152F RID: 5423
		void Skip([In] uint celt);

		// Token: 0x06001530 RID: 5424
		void Reset();

		// Token: 0x06001531 RID: 5425
		IEnumSTORE_ASSEMBLY Clone();
	}
}
