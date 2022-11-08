using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000205 RID: 517
	[Guid("b30352cf-23da-4577-9b3f-b4e6573be53b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumReferenceIdentity
	{
		// Token: 0x06001576 RID: 5494
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IReferenceIdentity[] ReferenceIdentity);

		// Token: 0x06001577 RID: 5495
		void Skip(uint celt);

		// Token: 0x06001578 RID: 5496
		void Reset();

		// Token: 0x06001579 RID: 5497
		IEnumReferenceIdentity Clone();
	}
}
