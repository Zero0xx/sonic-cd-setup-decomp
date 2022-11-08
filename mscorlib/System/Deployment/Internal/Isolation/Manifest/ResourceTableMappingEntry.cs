using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CD RID: 461
	[StructLayout(LayoutKind.Sequential)]
	internal class ResourceTableMappingEntry
	{
		// Token: 0x040007DC RID: 2012
		[MarshalAs(UnmanagedType.LPWStr)]
		public string id;

		// Token: 0x040007DD RID: 2013
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FinalStringMapped;
	}
}
