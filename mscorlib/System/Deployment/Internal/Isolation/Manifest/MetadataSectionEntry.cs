using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001E2 RID: 482
	[StructLayout(LayoutKind.Sequential)]
	internal class MetadataSectionEntry : IDisposable
	{
		// Token: 0x060014F1 RID: 5361 RVA: 0x000369E8 File Offset: 0x000359E8
		~MetadataSectionEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00036A18 File Offset: 0x00035A18
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00036A24 File Offset: 0x00035A24
		public void Dispose(bool fDisposing)
		{
			if (this.ManifestHash != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ManifestHash);
				this.ManifestHash = IntPtr.Zero;
			}
			if (this.MvidValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.MvidValue);
				this.MvidValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000819 RID: 2073
		public uint SchemaVersion;

		// Token: 0x0400081A RID: 2074
		public uint ManifestFlags;

		// Token: 0x0400081B RID: 2075
		public uint UsagePatterns;

		// Token: 0x0400081C RID: 2076
		public IDefinitionIdentity CdfIdentity;

		// Token: 0x0400081D RID: 2077
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LocalPath;

		// Token: 0x0400081E RID: 2078
		public uint HashAlgorithm;

		// Token: 0x0400081F RID: 2079
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ManifestHash;

		// Token: 0x04000820 RID: 2080
		public uint ManifestHashSize;

		// Token: 0x04000821 RID: 2081
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ContentType;

		// Token: 0x04000822 RID: 2082
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeImageVersion;

		// Token: 0x04000823 RID: 2083
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr MvidValue;

		// Token: 0x04000824 RID: 2084
		public uint MvidValueSize;

		// Token: 0x04000825 RID: 2085
		public DescriptionMetadataEntry DescriptionData;

		// Token: 0x04000826 RID: 2086
		public DeploymentMetadataEntry DeploymentData;

		// Token: 0x04000827 RID: 2087
		public DependentOSMetadataEntry DependentOSData;

		// Token: 0x04000828 RID: 2088
		[MarshalAs(UnmanagedType.LPWStr)]
		public string defaultPermissionSetID;

		// Token: 0x04000829 RID: 2089
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RequestedExecutionLevel;

		// Token: 0x0400082A RID: 2090
		public bool RequestedExecutionLevelUIAccess;

		// Token: 0x0400082B RID: 2091
		public IReferenceIdentity ResourceTypeResourcesDependency;

		// Token: 0x0400082C RID: 2092
		public IReferenceIdentity ResourceTypeManifestResourcesDependency;

		// Token: 0x0400082D RID: 2093
		[MarshalAs(UnmanagedType.LPWStr)]
		public string KeyInfoElement;
	}
}
