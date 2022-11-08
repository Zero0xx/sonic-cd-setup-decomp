using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001DC RID: 476
	[StructLayout(LayoutKind.Sequential)]
	internal class DeploymentMetadataEntry
	{
		// Token: 0x040007FF RID: 2047
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DeploymentProviderCodebase;

		// Token: 0x04000800 RID: 2048
		[MarshalAs(UnmanagedType.LPWStr)]
		public string MinimumRequiredVersion;

		// Token: 0x04000801 RID: 2049
		public ushort MaximumAge;

		// Token: 0x04000802 RID: 2050
		public byte MaximumAge_Unit;

		// Token: 0x04000803 RID: 2051
		public uint DeploymentFlags;
	}
}
