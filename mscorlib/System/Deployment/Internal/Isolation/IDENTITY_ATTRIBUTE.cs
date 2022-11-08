using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E7 RID: 487
	internal struct IDENTITY_ATTRIBUTE
	{
		// Token: 0x04000846 RID: 2118
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Namespace;

		// Token: 0x04000847 RID: 2119
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000848 RID: 2120
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
