using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E9 RID: 489
	internal struct STORE_ASSEMBLY
	{
		// Token: 0x0400084D RID: 2125
		public uint Status;

		// Token: 0x0400084E RID: 2126
		public IDefinitionIdentity DefinitionIdentity;

		// Token: 0x0400084F RID: 2127
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x04000850 RID: 2128
		public ulong AssemblySize;

		// Token: 0x04000851 RID: 2129
		public ulong ChangeId;
	}
}
