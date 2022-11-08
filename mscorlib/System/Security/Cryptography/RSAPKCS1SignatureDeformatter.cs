using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x020008A1 RID: 2209
	[ComVisible(true)]
	public class RSAPKCS1SignatureDeformatter : AsymmetricSignatureDeformatter
	{
		// Token: 0x06005060 RID: 20576 RVA: 0x00119B12 File Offset: 0x00118B12
		public RSAPKCS1SignatureDeformatter()
		{
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x00119B1A File Offset: 0x00118B1A
		public RSAPKCS1SignatureDeformatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x00119B3C File Offset: 0x00118B3C
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x00119B58 File Offset: 0x00118B58
		public override void SetHashAlgorithm(string strName)
		{
			this._strOID = CryptoConfig.MapNameToOID(strName, OidGroup.HashAlgorithm);
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x00119B68 File Offset: 0x00118B68
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
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
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				int calgHash = X509Utils.OidToAlgIdStrict(this._strOID, OidGroup.HashAlgorithm);
				return ((RSACryptoServiceProvider)this._rsaKey).VerifyHash(rgbHash, calgHash, rgbSignature);
			}
			byte[] rhs = Utils.RsaPkcs1Padding(this._rsaKey, CryptoConfig.EncodeOID(this._strOID), rgbHash);
			return Utils.CompareBigIntArrays(this._rsaKey.EncryptValue(rgbSignature), rhs);
		}

		// Token: 0x04002954 RID: 10580
		private RSA _rsaKey;

		// Token: 0x04002955 RID: 10581
		private string _strOID;
	}
}
