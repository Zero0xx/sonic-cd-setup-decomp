using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001FD RID: 509
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("19be1967-b2fc-4dc1-9627-f3cb6305d2a7")]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY_SUBCATEGORY
	{
		// Token: 0x0600154F RID: 5455
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY_SUBCATEGORY[] rgElements);

		// Token: 0x06001550 RID: 5456
		void Skip([In] uint ulElements);

		// Token: 0x06001551 RID: 5457
		void Reset();

		// Token: 0x06001552 RID: 5458
		IEnumSTORE_CATEGORY_SUBCATEGORY Clone();
	}
}
