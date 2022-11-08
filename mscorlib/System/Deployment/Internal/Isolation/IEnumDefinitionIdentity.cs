using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000204 RID: 516
	[Guid("f3549d9c-fc73-4793-9c00-1cd204254c0c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumDefinitionIdentity
	{
		// Token: 0x06001572 RID: 5490
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionIdentity[] DefinitionIdentity);

		// Token: 0x06001573 RID: 5491
		void Skip([In] uint celt);

		// Token: 0x06001574 RID: 5492
		void Reset();

		// Token: 0x06001575 RID: 5493
		IEnumDefinitionIdentity Clone();
	}
}
