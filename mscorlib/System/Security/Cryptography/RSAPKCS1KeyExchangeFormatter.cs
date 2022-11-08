using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020008A0 RID: 2208
	[ComVisible(true)]
	public class RSAPKCS1KeyExchangeFormatter : AsymmetricKeyExchangeFormatter
	{
		// Token: 0x06005058 RID: 20568 RVA: 0x001199C5 File Offset: 0x001189C5
		public RSAPKCS1KeyExchangeFormatter()
		{
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001199CD File Offset: 0x001189CD
		public RSAPKCS1KeyExchangeFormatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x0600505A RID: 20570 RVA: 0x001199EF File Offset: 0x001189EF
		public override string Parameters
		{
			get
			{
				return "<enc:KeyEncryptionMethod enc:Algorithm=\"http://www.microsoft.com/xml/security/algorithm/PKCS1-v1.5-KeyEx\" xmlns:enc=\"http://www.microsoft.com/xml/security/encryption/v1.0\" />";
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x0600505B RID: 20571 RVA: 0x001199F6 File Offset: 0x001189F6
		// (set) Token: 0x0600505C RID: 20572 RVA: 0x001199FE File Offset: 0x001189FE
		public RandomNumberGenerator Rng
		{
			get
			{
				return this.RngValue;
			}
			set
			{
				this.RngValue = value;
			}
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x00119A07 File Offset: 0x00118A07
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x00119A24 File Offset: 0x00118A24
		public override byte[] CreateKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			byte[] result;
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				result = ((RSACryptoServiceProvider)this._rsaKey).Encrypt(rgbData, false);
			}
			else
			{
				int num = this._rsaKey.KeySize / 8;
				if (rgbData.Length + 11 > num)
				{
					throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_Padding_EncDataTooBig"), new object[]
					{
						num - 11
					}));
				}
				byte[] array = new byte[num];
				if (this.RngValue == null)
				{
					this.RngValue = RandomNumberGenerator.Create();
				}
				this.Rng.GetNonZeroBytes(array);
				array[0] = 0;
				array[1] = 2;
				array[num - rgbData.Length - 1] = 0;
				Buffer.InternalBlockCopy(rgbData, 0, array, num - rgbData.Length, rgbData.Length);
				result = this._rsaKey.EncryptValue(array);
			}
			return result;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x00119B09 File Offset: 0x00118B09
		public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
		{
			return this.CreateKeyExchange(rgbData);
		}

		// Token: 0x04002952 RID: 10578
		private RandomNumberGenerator RngValue;

		// Token: 0x04002953 RID: 10579
		private RSA _rsaKey;
	}
}
