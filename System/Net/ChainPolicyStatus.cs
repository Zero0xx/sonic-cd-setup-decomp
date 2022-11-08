using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020003FE RID: 1022
	internal struct ChainPolicyStatus
	{
		// Token: 0x0400205A RID: 8282
		public uint cbSize;

		// Token: 0x0400205B RID: 8283
		public uint dwError;

		// Token: 0x0400205C RID: 8284
		public uint lChainIndex;

		// Token: 0x0400205D RID: 8285
		public uint lElementIndex;

		// Token: 0x0400205E RID: 8286
		public unsafe void* pvExtraPolicyStatus;

		// Token: 0x0400205F RID: 8287
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainPolicyStatus));
	}
}
