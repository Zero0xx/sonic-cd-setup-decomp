using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D6 RID: 470
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyRequestEntry
	{
		// Token: 0x040007EE RID: 2030
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040007EF RID: 2031
		[MarshalAs(UnmanagedType.LPWStr)]
		public string permissionSetID;
	}
}
