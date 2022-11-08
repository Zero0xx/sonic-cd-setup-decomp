using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001DE RID: 478
	[Guid("CFA3F59F-334D-46bf-A5A5-5D11BB2D7EBC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IDeploymentMetadataEntry
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060014E2 RID: 5346
		DeploymentMetadataEntry AllData { get; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060014E3 RID: 5347
		string DeploymentProviderCodebase { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060014E4 RID: 5348
		string MinimumRequiredVersion { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060014E5 RID: 5349
		ushort MaximumAge { get; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060014E6 RID: 5350
		byte MaximumAge_Unit { get; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060014E7 RID: 5351
		uint DeploymentFlags { get; }
	}
}
