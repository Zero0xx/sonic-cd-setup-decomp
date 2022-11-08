using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CA RID: 458
	[StructLayout(LayoutKind.Sequential)]
	internal class WindowClassEntry
	{
		// Token: 0x040007D6 RID: 2006
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;

		// Token: 0x040007D7 RID: 2007
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostDll;

		// Token: 0x040007D8 RID: 2008
		public bool fVersioned;
	}
}
