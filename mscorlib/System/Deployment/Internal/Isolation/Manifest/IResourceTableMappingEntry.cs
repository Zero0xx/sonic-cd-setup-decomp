using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CF RID: 463
	[Guid("70A4ECEE-B195-4c59-85BF-44B6ACA83F07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IResourceTableMappingEntry
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060014C7 RID: 5319
		ResourceTableMappingEntry AllData { get; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060014C8 RID: 5320
		string id { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060014C9 RID: 5321
		string FinalStringMapped { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
