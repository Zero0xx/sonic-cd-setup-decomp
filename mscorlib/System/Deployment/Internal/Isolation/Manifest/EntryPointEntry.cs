using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D0 RID: 464
	[StructLayout(LayoutKind.Sequential)]
	internal class EntryPointEntry
	{
		// Token: 0x040007E0 RID: 2016
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040007E1 RID: 2017
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_File;

		// Token: 0x040007E2 RID: 2018
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_Parameters;

		// Token: 0x040007E3 RID: 2019
		public IReferenceIdentity Identity;

		// Token: 0x040007E4 RID: 2020
		public uint Flags;
	}
}
