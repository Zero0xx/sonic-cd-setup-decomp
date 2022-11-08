using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001EB RID: 491
	internal struct STORE_ASSEMBLY_FILE
	{
		// Token: 0x04000854 RID: 2132
		public uint Size;

		// Token: 0x04000855 RID: 2133
		public uint Flags;

		// Token: 0x04000856 RID: 2134
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FileName;

		// Token: 0x04000857 RID: 2135
		public uint FileStatusFlags;
	}
}
