using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001E1 RID: 481
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CF168CF4-4E8F-4d92-9D2A-60E5CA21CF85")]
	[ComImport]
	internal interface IDependentOSMetadataEntry
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060014E9 RID: 5353
		DependentOSMetadataEntry AllData { get; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060014EA RID: 5354
		string SupportUrl { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060014EB RID: 5355
		string Description { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060014EC RID: 5356
		ushort MajorVersion { get; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060014ED RID: 5357
		ushort MinorVersion { get; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060014EE RID: 5358
		ushort BuildNumber { get; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060014EF RID: 5359
		byte ServicePackMajor { get; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060014F0 RID: 5360
		byte ServicePackMinor { get; }
	}
}
