using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086A RID: 2154
	[ComVisible(true)]
	public abstract class AsymmetricKeyExchangeFormatter
	{
		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06004E96 RID: 20118
		public abstract string Parameters { get; }

		// Token: 0x06004E97 RID: 20119
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06004E98 RID: 20120
		public abstract byte[] CreateKeyExchange(byte[] data);

		// Token: 0x06004E99 RID: 20121
		public abstract byte[] CreateKeyExchange(byte[] data, Type symAlgType);
	}
}
