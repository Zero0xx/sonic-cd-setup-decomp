using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B2 RID: 434
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipDataEntry
	{
		// Token: 0x04000791 RID: 1937
		public uint index;

		// Token: 0x04000792 RID: 1938
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;

		// Token: 0x04000793 RID: 1939
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;
	}
}
