using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D5 RID: 469
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("EBE5A1ED-FEBC-42c4-A9E1-E087C6E36635")]
	[ComImport]
	internal interface IPermissionSetEntry
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060014D2 RID: 5330
		PermissionSetEntry AllData { get; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060014D3 RID: 5331
		string Id { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060014D4 RID: 5332
		string XmlSegment { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
