using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001FB RID: 507
	[Guid("b840a2f5-a497-4a6d-9038-cd3ec2fbd222")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY
	{
		// Token: 0x06001544 RID: 5444
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY[] rgElements);

		// Token: 0x06001545 RID: 5445
		void Skip([In] uint ulElements);

		// Token: 0x06001546 RID: 5446
		void Reset();

		// Token: 0x06001547 RID: 5447
		IEnumSTORE_CATEGORY Clone();
	}
}
