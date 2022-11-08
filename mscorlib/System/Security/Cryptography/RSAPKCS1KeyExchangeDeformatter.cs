using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200089F RID: 2207
	[ComVisible(true)]
	public class RSAPKCS1KeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x06005050 RID: 20560 RVA: 0x001198CD File Offset: 0x001188CD
		public RSAPKCS1KeyExchangeDeformatter()
		{
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x001198D5 File Offset: 0x001188D5
		public RSAPKCS1KeyExchangeDeformatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06005052 RID: 20562 RVA: 0x001198F7 File Offset: 0x001188F7
		// (set) Token: 0x06005053 RID: 20563 RVA: 0x001198FF File Offset: 0x001188FF
		public RandomNumberGenerator RNG
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

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06005054 RID: 20564 RVA: 0x00119908 File Offset: 0x00118908
		// (set) Token: 0x06005055 RID: 20565 RVA: 0x0011990B File Offset: 0x0011890B
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

		// Token: 0x06005056 RID: 20566 RVA: 0x00119910 File Offset: 0x00118910
		public override byte[] DecryptKeyExchange(byte[] rgbIn)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_MissingKey"));
			}
			byte[] array;
			if (this._rsaKey is RSACryptoServiceProvider)
			{
				array = ((RSACryptoServiceProvider)this._rsaKey).Decrypt(rgbIn, false);
			}
			else
			{
				byte[] array2 = this._rsaKey.DecryptValue(rgbIn);
				int num = 2;
				while (num < array2.Length && array2[num] != 0)
				{
					num++;
				}
				if (num >= array2.Length)
				{
					throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_PKCS1Decoding"));
				}
				num++;
				array = new byte[array2.Length - num];
				Buffer.InternalBlockCopy(array2, num, array, 0, array.Length);
			}
			return array;
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x001199A9 File Offset: 0x001189A9
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x04002950 RID: 10576
		private RSA _rsaKey;

		// Token: 0x04002951 RID: 10577
		private RandomNumberGenerator RngValue;
	}
}
