using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000869 RID: 2153
	[ComVisible(true)]
	public abstract class AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06004E91 RID: 20113
		// (set) Token: 0x06004E92 RID: 20114
		public abstract string Parameters { get; set; }

		// Token: 0x06004E93 RID: 20115
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06004E94 RID: 20116
		public abstract byte[] DecryptKeyExchange(byte[] rgb);
	}
}
