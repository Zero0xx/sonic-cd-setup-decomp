using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BD RID: 445
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3903B11B-FBE8-477c-825F-DB828B5FD174")]
	[ComImport]
	internal interface ICOMServerEntry
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600149B RID: 5275
		COMServerEntry AllData { get; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600149C RID: 5276
		Guid Clsid { get; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600149D RID: 5277
		uint Flags { get; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600149E RID: 5278
		Guid ConfiguredGuid { get; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600149F RID: 5279
		Guid ImplementedClsid { get; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060014A0 RID: 5280
		Guid TypeLibrary { get; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060014A1 RID: 5281
		uint ThreadingModel { get; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060014A2 RID: 5282
		string RuntimeVersion { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060014A3 RID: 5283
		string HostFile { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
