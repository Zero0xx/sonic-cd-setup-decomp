using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D8 RID: 472
	[Guid("2474ECB4-8EFD-4410-9F31-B3E7C4A07731")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IAssemblyRequestEntry
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060014D6 RID: 5334
		AssemblyRequestEntry AllData { get; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060014D7 RID: 5335
		string Name { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060014D8 RID: 5336
		string permissionSetID { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
