using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AF RID: 431
	[StructLayout(LayoutKind.Sequential)]
	internal class FileAssociationEntry
	{
		// Token: 0x04000787 RID: 1927
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Extension;

		// Token: 0x04000788 RID: 1928
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000789 RID: 1929
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgID;

		// Token: 0x0400078A RID: 1930
		[MarshalAs(UnmanagedType.LPWStr)]
		public string DefaultIcon;

		// Token: 0x0400078B RID: 1931
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Parameter;
	}
}
