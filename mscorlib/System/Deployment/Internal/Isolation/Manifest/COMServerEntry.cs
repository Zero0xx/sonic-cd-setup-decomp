using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BB RID: 443
	[StructLayout(LayoutKind.Sequential)]
	internal class COMServerEntry
	{
		// Token: 0x0400079F RID: 1951
		public Guid Clsid;

		// Token: 0x040007A0 RID: 1952
		public uint Flags;

		// Token: 0x040007A1 RID: 1953
		public Guid ConfiguredGuid;

		// Token: 0x040007A2 RID: 1954
		public Guid ImplementedClsid;

		// Token: 0x040007A3 RID: 1955
		public Guid TypeLibrary;

		// Token: 0x040007A4 RID: 1956
		public uint ThreadingModel;

		// Token: 0x040007A5 RID: 1957
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x040007A6 RID: 1958
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostFile;
	}
}
