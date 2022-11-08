using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C7 RID: 455
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceEntry
	{
		// Token: 0x040007D0 RID: 2000
		public IReferenceIdentity ReferenceIdentity;

		// Token: 0x040007D1 RID: 2001
		public uint Flags;

		// Token: 0x040007D2 RID: 2002
		public AssemblyReferenceDependentAssemblyEntry DependentAssembly;
	}
}
