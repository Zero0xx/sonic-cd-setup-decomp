using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A8 RID: 424
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("397927f5-10f2-4ecb-bfe1-3c264212a193")]
	[ComImport]
	internal interface IMuiResourceMapEntry
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06001465 RID: 5221
		MuiResourceMapEntry AllData { get; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001466 RID: 5222
		object ResourceTypeIdInt { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001467 RID: 5223
		object ResourceTypeIdString { [return: MarshalAs(UnmanagedType.Interface)] get; }
	}
}
