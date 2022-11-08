using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001DF RID: 479
	[StructLayout(LayoutKind.Sequential)]
	internal class DependentOSMetadataEntry
	{
		// Token: 0x0400080A RID: 2058
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x0400080B RID: 2059
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x0400080C RID: 2060
		public ushort MajorVersion;

		// Token: 0x0400080D RID: 2061
		public ushort MinorVersion;

		// Token: 0x0400080E RID: 2062
		public ushort BuildNumber;

		// Token: 0x0400080F RID: 2063
		public byte ServicePackMajor;

		// Token: 0x04000810 RID: 2064
		public byte ServicePackMinor;
	}
}
