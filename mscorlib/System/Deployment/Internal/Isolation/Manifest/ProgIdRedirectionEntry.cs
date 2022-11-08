using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BE RID: 446
	[StructLayout(LayoutKind.Sequential)]
	internal class ProgIdRedirectionEntry
	{
		// Token: 0x040007AF RID: 1967
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgId;

		// Token: 0x040007B0 RID: 1968
		public Guid RedirectedGuid;
	}
}
