using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C1 RID: 449
	[StructLayout(LayoutKind.Sequential)]
	internal class CLRSurrogateEntry
	{
		// Token: 0x040007B3 RID: 1971
		public Guid Clsid;

		// Token: 0x040007B4 RID: 1972
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x040007B5 RID: 1973
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;
	}
}
