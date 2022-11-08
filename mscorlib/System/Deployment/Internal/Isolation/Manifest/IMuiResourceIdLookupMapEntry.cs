using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019F RID: 415
	[Guid("24abe1f7-a396-4a03-9adf-1d5b86a5569f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMuiResourceIdLookupMapEntry
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001451 RID: 5201
		MuiResourceIdLookupMapEntry AllData { get; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001452 RID: 5202
		uint Count { get; }
	}
}
