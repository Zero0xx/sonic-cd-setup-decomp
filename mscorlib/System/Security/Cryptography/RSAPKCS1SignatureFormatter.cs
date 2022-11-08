using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020008A2 RID: 2210
	[ComVisible(true)]
	public class RSAPKCS1SignatureFormatter : AsymmetricSignatureFormatter
	{
		// Token: 0x06005065 RID: 20581 RVA: 0x00119C19 File Offset: 0x00118C19
		public RSAPKCS1SignatureFormatter()
		{
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x00119C21 File Offset: 0x00118C21
		public RSAPKCS1SignatureFormatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x00119C43 File Offset: 0x00118C43
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x00119C5F File Offset: 0x00118C5F
		public override void SetHashAlgorithm(string strName)
		{
			this._strOID = CryptoConfig.MapNameToOID(strName);
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x00119C70 File Offset: 0x00118C70
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			if (this._strOID == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingOID"));
			}
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				return ((RSACryptoServiceProvider)this._rsaKey).SignHash(rgbHash, this._strOID);
			}
			byte[] rgb = Utils.RsaPkcs1Padding(this._rsaKey, CryptoConfig.EncodeOID(this._strOID), rgbHash);
			return this._rsaKey.DecryptValue(rgb);
		}

		// Token: 0x04002956 RID: 10582
		private RSA _rsaKey;

		// Token: 0x04002957 RID: 10583
		private string _strOID;
	}
}
