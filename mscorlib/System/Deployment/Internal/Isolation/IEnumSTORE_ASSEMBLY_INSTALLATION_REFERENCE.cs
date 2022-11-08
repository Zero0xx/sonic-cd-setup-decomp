using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F2 RID: 498
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d8b1aacb-5142-4abb-bcc1-e9dc9052a89e")]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE
	{
		// Token: 0x06001514 RID: 5396
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] StoreApplicationReference[] rgelt);

		// Token: 0x06001515 RID: 5397
		void Skip([In] uint celt);

		// Token: 0x06001516 RID: 5398
		void Reset();

		// Token: 0x06001517 RID: 5399
		IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE Clone();
	}
}
