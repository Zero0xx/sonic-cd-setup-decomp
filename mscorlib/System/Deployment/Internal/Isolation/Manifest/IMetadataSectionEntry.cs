using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001E4 RID: 484
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("AB1ED79F-943E-407d-A80B-0744E3A95B28")]
	[ComImport]
	internal interface IMetadataSectionEntry
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060014F5 RID: 5365
		MetadataSectionEntry AllData { get; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060014F6 RID: 5366
		uint SchemaVersion { get; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060014F7 RID: 5367
		uint ManifestFlags { get; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060014F8 RID: 5368
		uint UsagePatterns { get; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060014F9 RID: 5369
		IDefinitionIdentity CdfIdentity { get; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060014FA RID: 5370
		string LocalPath { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060014FB RID: 5371
		uint HashAlgorithm { get; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060014FC RID: 5372
		object ManifestHash { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060014FD RID: 5373
		string ContentType { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060014FE RID: 5374
		string RuntimeImageVersion { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060014FF RID: 5375
		object MvidValue { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001500 RID: 5376
		IDescriptionMetadataEntry DescriptionData { get; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001501 RID: 5377
		IDeploymentMetadataEntry DeploymentData { get; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001502 RID: 5378
		IDependentOSMetadataEntry DependentOSData { get; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06001503 RID: 5379
		string defaultPermissionSetID { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001504 RID: 5380
		string RequestedExecutionLevel { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001505 RID: 5381
		bool RequestedExecutionLevelUIAccess { get; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001506 RID: 5382
		IReferenceIdentity ResourceTypeResourcesDependency { get; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001507 RID: 5383
		IReferenceIdentity ResourceTypeManifestResourcesDependency { get; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001508 RID: 5384
		string KeyInfoElement { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
