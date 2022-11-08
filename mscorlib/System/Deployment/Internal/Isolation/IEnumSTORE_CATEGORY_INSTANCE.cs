using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001FF RID: 511
	[Guid("5ba7cb30-8508-4114-8c77-262fcda4fadb")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY_INSTANCE
	{
		// Token: 0x0600155A RID: 5466
		uint Next([In] uint ulElements, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY_INSTANCE[] rgInstances);

		// Token: 0x0600155B RID: 5467
		void Skip([In] uint ulElements);

		// Token: 0x0600155C RID: 5468
		void Reset();

		// Token: 0x0600155D RID: 5469
		IEnumSTORE_CATEGORY_INSTANCE Clone();
	}
}
