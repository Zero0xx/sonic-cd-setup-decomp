using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086B RID: 2155
	[ComVisible(true)]
	public abstract class AsymmetricSignatureDeformatter
	{
		// Token: 0x06004E9B RID: 20123
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06004E9C RID: 20124
		public abstract void SetHashAlgorithm(string strName);

		// Token: 0x06004E9D RID: 20125 RVA: 0x001100E9 File Offset: 0x0010F0E9
		public virtual bool VerifySignature(HashAlgorithm hash, byte[] rgbSignature)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			this.SetHashAlgorithm(hash.ToString());
			return this.VerifySignature(hash.Hash, rgbSignature);
		}

		// Token: 0x06004E9E RID: 20126
		public abstract bool VerifySignature(byte[] rgbHash, byte[] rgbSignature);
	}
}
