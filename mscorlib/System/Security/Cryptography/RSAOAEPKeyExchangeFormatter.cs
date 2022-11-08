using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200089E RID: 2206
	[ComVisible(true)]
	public class RSAOAEPKeyExchangeFormatter : AsymmetricKeyExchangeFormatter
	{
		// Token: 0x06005046 RID: 20550 RVA: 0x001197CF File Offset: 0x001187CF
		public RSAOAEPKeyExchangeFormatter()
		{
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x001197D7 File Offset: 0x001187D7
		public RSAOAEPKeyExchangeFormatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06005048 RID: 20552 RVA: 0x001197F9 File Offset: 0x001187F9
		// (set) Token: 0x06005049 RID: 20553 RVA: 0x00119815 File Offset: 0x00118815
		public byte[] Parameter
		{
			get
			{
				if (this.ParameterValue != null)
				{
					return (byte[])this.ParameterValue.Clone();
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.ParameterValue = (byte[])value.Clone();
					return;
				}
				this.ParameterValue = null;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x0600504A RID: 20554 RVA: 0x00119833 File Offset: 0x00118833
		public override string Parameters
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x0600504B RID: 20555 RVA: 0x00119836 File Offset: 0x00118836
		// (set) Token: 0x0600504C RID: 20556 RVA: 0x0011983E File Offset: 0x0011883E
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

		// Token: 0x0600504D RID: 20557 RVA: 0x00119847 File Offset: 0x00118847
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x00119864 File Offset: 0x00118864
		public override byte[] CreateKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				return ((RSACryptoServiceProvider)this._rsaKey).Encrypt(rgbData, true);
			}
			return Utils.RsaOaepEncrypt(this._rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), RandomNumberGenerator.Create(), rgbData);
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x001198C4 File Offset: 0x001188C4
		public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
		{
			return this.CreateKeyExchange(rgbData);
		}

		// Token: 0x0400294D RID: 10573
		private byte[] ParameterValue;

		// Token: 0x0400294E RID: 10574
		private RSA _rsaKey;

		// Token: 0x0400294F RID: 10575
		private RandomNumberGenerator RngValue;
	}
}
