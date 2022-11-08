using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A2 RID: 418
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("11df5cad-c183-479b-9a44-3842b71639ce")]
	[ComImport]
	internal interface IMuiResourceTypeIdStringEntry
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001457 RID: 5207
		MuiResourceTypeIdStringEntry AllData { get; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001458 RID: 5208
		object StringIds { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001459 RID: 5209
		object IntegerIds { [return: MarshalAs(UnmanagedType.Interface)] get; }
	}
}
