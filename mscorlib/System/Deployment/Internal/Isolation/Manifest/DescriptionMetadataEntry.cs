using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D9 RID: 473
	[StructLayout(LayoutKind.Sequential)]
	internal class DescriptionMetadataEntry
	{
		// Token: 0x040007F2 RID: 2034
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Publisher;

		// Token: 0x040007F3 RID: 2035
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Product;

		// Token: 0x040007F4 RID: 2036
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x040007F5 RID: 2037
		[MarshalAs(UnmanagedType.LPWStr)]
		public string IconFile;

		// Token: 0x040007F6 RID: 2038
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ErrorReportUrl;

		// Token: 0x040007F7 RID: 2039
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SuiteName;
	}
}
