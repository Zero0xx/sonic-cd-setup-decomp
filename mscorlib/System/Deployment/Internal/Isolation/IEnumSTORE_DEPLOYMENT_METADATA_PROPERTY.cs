using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F5 RID: 501
	[Guid("5fa4f590-a416-4b22-ac79-7c3f0d31f303")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY
	{
		// Token: 0x06001523 RID: 5411
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] StoreOperationMetadataProperty[] AppIds);

		// Token: 0x06001524 RID: 5412
		void Skip([In] uint celt);

		// Token: 0x06001525 RID: 5413
		void Reset();

		// Token: 0x06001526 RID: 5414
		IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY Clone();
	}
}
