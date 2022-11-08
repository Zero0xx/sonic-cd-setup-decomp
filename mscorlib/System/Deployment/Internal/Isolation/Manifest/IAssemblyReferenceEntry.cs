using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C9 RID: 457
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("FD47B733-AFBC-45e4-B7C2-BBEB1D9F766C")]
	[ComImport]
	internal interface IAssemblyReferenceEntry
	{
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060014BD RID: 5309
		AssemblyReferenceEntry AllData { get; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060014BE RID: 5310
		IReferenceIdentity ReferenceIdentity { get; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060014BF RID: 5311
		uint Flags { get; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060014C0 RID: 5312
		IAssemblyReferenceDependentAssemblyEntry DependentAssembly { get; }
	}
}
