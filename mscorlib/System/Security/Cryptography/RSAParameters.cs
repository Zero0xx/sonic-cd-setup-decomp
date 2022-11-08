using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000899 RID: 2201
	[ComVisible(true)]
	[Serializable]
	public struct RSAParameters
	{
		// Token: 0x04002935 RID: 10549
		public byte[] Exponent;

		// Token: 0x04002936 RID: 10550
		public byte[] Modulus;

		// Token: 0x04002937 RID: 10551
		[NonSerialized]
		public byte[] P;

		// Token: 0x04002938 RID: 10552
		[NonSerialized]
		public byte[] Q;

		// Token: 0x04002939 RID: 10553
		[NonSerialized]
		public byte[] DP;

		// Token: 0x0400293A RID: 10554
		[NonSerialized]
		public byte[] DQ;

		// Token: 0x0400293B RID: 10555
		[NonSerialized]
		public byte[] InverseQ;

		// Token: 0x0400293C RID: 10556
		[NonSerialized]
		public byte[] D;
	}
}
