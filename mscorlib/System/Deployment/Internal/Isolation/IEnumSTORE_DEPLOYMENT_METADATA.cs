using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F3 RID: 499
	[Guid("f9fd4090-93db-45c0-af87-624940f19cff")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_DEPLOYMENT_METADATA
	{
		// Token: 0x06001518 RID: 5400
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionAppId[] AppIds);

		// Token: 0x06001519 RID: 5401
		void Skip([In] uint celt);

		// Token: 0x0600151A RID: 5402
		void Reset();

		// Token: 0x0600151B RID: 5403
		IEnumSTORE_DEPLOYMENT_METADATA Clone();
	}
}
