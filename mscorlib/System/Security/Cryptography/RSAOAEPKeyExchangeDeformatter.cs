using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200089D RID: 2205
	[ComVisible(true)]
	public class RSAOAEPKeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x06005040 RID: 20544 RVA: 0x00119726 File Offset: 0x00118726
		public RSAOAEPKeyExchangeDeformatter()
		{
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x0011972E File Offset: 0x0011872E
		public RSAOAEPKeyExchangeDeformatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06005042 RID: 20546 RVA: 0x00119750 File Offset: 0x00118750
		// (set) Token: 0x06005043 RID: 20547 RVA: 0x00119753 File Offset: 0x00118753
		public override string Parameters
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x00119758 File Offset: 0x00118758
		public override byte[] DecryptKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				return ((RSACryptoServiceProvider)this._rsaKey).Decrypt(rgbData, true);
			}
			return Utils.RsaOaepDecrypt(this._rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), rgbData);
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x001197B3 File Offset: 0x001187B3
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x0400294C RID: 10572
		private RSA _rsaKey;
	}
}
