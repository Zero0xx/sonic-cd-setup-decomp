using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086C RID: 2156
	[ComVisible(true)]
	public abstract class AsymmetricSignatureFormatter
	{
		// Token: 0x06004EA0 RID: 20128
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06004EA1 RID: 20129
		public abstract void SetHashAlgorithm(string strName);

		// Token: 0x06004EA2 RID: 20130 RVA: 0x0011011A File Offset: 0x0010F11A
		public virtual byte[] CreateSignature(HashAlgorithm hash)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			this.SetHashAlgorithm(hash.ToString());
			return this.CreateSignature(hash.Hash);
		}

		// Token: 0x06004EA3 RID: 20131
		public abstract byte[] CreateSignature(byte[] rgbHash);
	}
}
