using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000401 RID: 1025
	internal struct ChainParameters
	{
		// Token: 0x04002064 RID: 8292
		public uint cbSize;

		// Token: 0x04002065 RID: 8293
		public CertUsageMatch RequestedUsage;

		// Token: 0x04002066 RID: 8294
		public CertUsageMatch RequestedIssuancePolicy;

		// Token: 0x04002067 RID: 8295
		public uint UrlRetrievalTimeout;

		// Token: 0x04002068 RID: 8296
		public int BoolCheckRevocationFreshnessTime;

		// Token: 0x04002069 RID: 8297
		public uint RevocationFreshnessTime;

		// Token: 0x0400206A RID: 8298
		public static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(ChainParameters));
	}
}
