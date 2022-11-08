using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D3 RID: 467
	[StructLayout(LayoutKind.Sequential)]
	internal class PermissionSetEntry
	{
		// Token: 0x040007EA RID: 2026
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Id;

		// Token: 0x040007EB RID: 2027
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XmlSegment;
	}
}
