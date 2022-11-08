using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D2 RID: 466
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1583EFE9-832F-4d08-B041-CAC5ACEDB948")]
	[ComImport]
	internal interface IEntryPointEntry
	{
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060014CB RID: 5323
		EntryPointEntry AllData { get; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060014CC RID: 5324
		string Name { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060014CD RID: 5325
		string CommandLine_File { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060014CE RID: 5326
		string CommandLine_Parameters { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060014CF RID: 5327
		IReferenceIdentity Identity { get; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060014D0 RID: 5328
		uint Flags { get; }
	}
}
