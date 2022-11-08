using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A5 RID: 421
	[Guid("55b2dec1-d0f6-4bf4-91b1-30f73ad8e4df")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMuiResourceTypeIdIntEntry
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600145E RID: 5214
		MuiResourceTypeIdIntEntry AllData { get; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600145F RID: 5215
		object StringIds { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06001460 RID: 5216
		object IntegerIds { [return: MarshalAs(UnmanagedType.Interface)] get; }
	}
}
