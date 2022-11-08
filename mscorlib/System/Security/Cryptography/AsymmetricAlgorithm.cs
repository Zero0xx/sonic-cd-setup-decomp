using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000868 RID: 2152
	[ComVisible(true)]
	public abstract class AsymmetricAlgorithm : IDisposable
	{
		// Token: 0x06004E84 RID: 20100 RVA: 0x0010FFF3 File Offset: 0x0010EFF3
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x00110002 File Offset: 0x0010F002
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004E86 RID: 20102
		protected abstract void Dispose(bool disposing);

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06004E87 RID: 20103 RVA: 0x0011000A File Offset: 0x0010F00A
		// (set) Token: 0x06004E88 RID: 20104 RVA: 0x00110014 File Offset: 0x0010F014
		public virtual int KeySize
		{
			get
			{
				return this.KeySizeValue;
			}
			set
			{
				for (int i = 0; i < this.LegalKeySizesValue.Length; i++)
				{
					if (this.LegalKeySizesValue[i].SkipSize == 0)
					{
						if (this.LegalKeySizesValue[i].MinSize == value)
						{
							this.KeySizeValue = value;
							return;
						}
					}
					else
					{
						for (int j = this.LegalKeySizesValue[i].MinSize; j <= this.LegalKeySizesValue[i].MaxSize; j += this.LegalKeySizesValue[i].SkipSize)
						{
							if (j == value)
							{
								this.KeySizeValue = value;
								return;
							}
						}
					}
				}
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06004E89 RID: 20105 RVA: 0x001100A6 File Offset: 0x0010F0A6
		public virtual KeySizes[] LegalKeySizes
		{
			get
			{
				return (KeySizes[])this.LegalKeySizesValue.Clone();
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06004E8A RID: 20106
		public abstract string SignatureAlgorithm { get; }

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06004E8B RID: 20107
		public abstract string KeyExchangeAlgorithm { get; }

		// Token: 0x06004E8C RID: 20108 RVA: 0x001100B8 File Offset: 0x0010F0B8
		public static AsymmetricAlgorithm Create()
		{
			return AsymmetricAlgorithm.Create("System.Security.Cryptography.AsymmetricAlgorithm");
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x001100C4 File Offset: 0x0010F0C4
		public static AsymmetricAlgorithm Create(string algName)
		{
			return (AsymmetricAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x06004E8E RID: 20110
		public abstract void FromXmlString(string xmlString);

		// Token: 0x06004E8F RID: 20111
		public abstract string ToXmlString(bool includePrivateParameters);

		// Token: 0x04002896 RID: 10390
		protected int KeySizeValue;

		// Token: 0x04002897 RID: 10391
		protected KeySizes[] LegalKeySizesValue;
	}
}
