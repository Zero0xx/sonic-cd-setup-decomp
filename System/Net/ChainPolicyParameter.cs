using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020003FB RID: 1019
	internal struct ChainPolicyParameter
	{
		// Token: 0x0400204F RID: 8271
		public uint cbSize;

		// Token: 0x04002050 RID: 8272
		public uint dwFlags;

		// Token: 0x04002051 RID: 8273
		public unsafe SSL_EXTRA_CERT_CHAIN_POLICY_PARA* pvExtraPolicyPara;

		// Token: 0x04002052 RID: 8274
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainPolicyParameter));
	}
}
