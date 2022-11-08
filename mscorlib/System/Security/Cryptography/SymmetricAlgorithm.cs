using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000877 RID: 2167
	[ComVisible(true)]
	public abstract class SymmetricAlgorithm : IDisposable
	{
		// Token: 0x06004EF6 RID: 20214 RVA: 0x00112C71 File Offset: 0x00111C71
		protected SymmetricAlgorithm()
		{
			this.ModeValue = CipherMode.CBC;
			this.PaddingValue = PaddingMode.PKCS7;
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00112C87 File Offset: 0x00111C87
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00112C96 File Offset: 0x00111C96
		public void Clear()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x00112CA0 File Offset: 0x00111CA0
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.KeyValue != null)
				{
					Array.Clear(this.KeyValue, 0, this.KeyValue.Length);
					this.KeyValue = null;
				}
				if (this.IVValue != null)
				{
					Array.Clear(this.IVValue, 0, this.IVValue.Length);
					this.IVValue = null;
				}
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06004EFA RID: 20218 RVA: 0x00112CF6 File Offset: 0x00111CF6
		// (set) Token: 0x06004EFB RID: 20219 RVA: 0x00112D00 File Offset: 0x00111D00
		public virtual int BlockSize
		{
			get
			{
				return this.BlockSizeValue;
			}
			set
			{
				for (int i = 0; i < this.LegalBlockSizesValue.Length; i++)
				{
					if (this.LegalBlockSizesValue[i].SkipSize == 0)
					{
						if (this.LegalBlockSizesValue[i].MinSize == value)
						{
							this.BlockSizeValue = value;
							this.IVValue = null;
							return;
						}
					}
					else
					{
						for (int j = this.LegalBlockSizesValue[i].MinSize; j <= this.LegalBlockSizesValue[i].MaxSize; j += this.LegalBlockSizesValue[i].SkipSize)
						{
							if (j == value)
							{
								if (this.BlockSizeValue != value)
								{
									this.BlockSizeValue = value;
									this.IVValue = null;
								}
								return;
							}
						}
					}
				}
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidBlockSize"));
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06004EFC RID: 20220 RVA: 0x00112DAC File Offset: 0x00111DAC
		// (set) Token: 0x06004EFD RID: 20221 RVA: 0x00112DB4 File Offset: 0x00111DB4
		public virtual int FeedbackSize
		{
			get
			{
				return this.FeedbackSizeValue;
			}
			set
			{
				if (value <= 0 || value > this.BlockSizeValue || value % 8 != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidFeedbackSize"));
				}
				this.FeedbackSizeValue = value;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06004EFE RID: 20222 RVA: 0x00112DDF File Offset: 0x00111DDF
		// (set) Token: 0x06004EFF RID: 20223 RVA: 0x00112DFF File Offset: 0x00111DFF
		public virtual byte[] IV
		{
			get
			{
				if (this.IVValue == null)
				{
					this.GenerateIV();
				}
				return (byte[])this.IVValue.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length != this.BlockSizeValue / 8)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidIVSize"));
				}
				this.IVValue = (byte[])value.Clone();
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06004F00 RID: 20224 RVA: 0x00112E3D File Offset: 0x00111E3D
		// (set) Token: 0x06004F01 RID: 20225 RVA: 0x00112E60 File Offset: 0x00111E60
		public virtual byte[] Key
		{
			get
			{
				if (this.KeyValue == null)
				{
					this.GenerateKey();
				}
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!this.ValidKeySize(value.Length * 8))
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
				}
				this.KeyValue = (byte[])value.Clone();
				this.KeySizeValue = value.Length * 8;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06004F02 RID: 20226 RVA: 0x00112EB4 File Offset: 0x00111EB4
		public virtual KeySizes[] LegalBlockSizes
		{
			get
			{
				return (KeySizes[])this.LegalBlockSizesValue.Clone();
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06004F03 RID: 20227 RVA: 0x00112EC6 File Offset: 0x00111EC6
		public virtual KeySizes[] LegalKeySizes
		{
			get
			{
				return (KeySizes[])this.LegalKeySizesValue.Clone();
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06004F04 RID: 20228 RVA: 0x00112ED8 File Offset: 0x00111ED8
		// (set) Token: 0x06004F05 RID: 20229 RVA: 0x00112EE0 File Offset: 0x00111EE0
		public virtual int KeySize
		{
			get
			{
				return this.KeySizeValue;
			}
			set
			{
				if (!this.ValidKeySize(value))
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
				}
				this.KeySizeValue = value;
				this.KeyValue = null;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06004F06 RID: 20230 RVA: 0x00112F09 File Offset: 0x00111F09
		// (set) Token: 0x06004F07 RID: 20231 RVA: 0x00112F11 File Offset: 0x00111F11
		public virtual CipherMode Mode
		{
			get
			{
				return this.ModeValue;
			}
			set
			{
				if (value < CipherMode.CBC || CipherMode.CFB < value)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidCipherMode"));
				}
				this.ModeValue = value;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06004F08 RID: 20232 RVA: 0x00112F32 File Offset: 0x00111F32
		// (set) Token: 0x06004F09 RID: 20233 RVA: 0x00112F3A File Offset: 0x00111F3A
		public virtual PaddingMode Padding
		{
			get
			{
				return this.PaddingValue;
			}
			set
			{
				if (value < PaddingMode.None || PaddingMode.ISO10126 < value)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidPaddingMode"));
				}
				this.PaddingValue = value;
			}
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x00112F5C File Offset: 0x00111F5C
		public bool ValidKeySize(int bitLength)
		{
			KeySizes[] legalKeySizes = this.LegalKeySizes;
			if (legalKeySizes == null)
			{
				return false;
			}
			for (int i = 0; i < legalKeySizes.Length; i++)
			{
				if (legalKeySizes[i].SkipSize == 0)
				{
					if (legalKeySizes[i].MinSize == bitLength)
					{
						return true;
					}
				}
				else
				{
					for (int j = legalKeySizes[i].MinSize; j <= legalKeySizes[i].MaxSize; j += legalKeySizes[i].SkipSize)
					{
						if (j == bitLength)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x00112FC2 File Offset: 0x00111FC2
		public static SymmetricAlgorithm Create()
		{
			return SymmetricAlgorithm.Create("System.Security.Cryptography.SymmetricAlgorithm");
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x00112FCE File Offset: 0x00111FCE
		public static SymmetricAlgorithm Create(string algName)
		{
			return (SymmetricAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x00112FDB File Offset: 0x00111FDB
		public virtual ICryptoTransform CreateEncryptor()
		{
			return this.CreateEncryptor(this.Key, this.IV);
		}

		// Token: 0x06004F0E RID: 20238
		public abstract ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV);

		// Token: 0x06004F0F RID: 20239 RVA: 0x00112FEF File Offset: 0x00111FEF
		public virtual ICryptoTransform CreateDecryptor()
		{
			return this.CreateDecryptor(this.Key, this.IV);
		}

		// Token: 0x06004F10 RID: 20240
		public abstract ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV);

		// Token: 0x06004F11 RID: 20241
		public abstract void GenerateKey();

		// Token: 0x06004F12 RID: 20242
		public abstract void GenerateIV();

		// Token: 0x040028D4 RID: 10452
		protected int BlockSizeValue;

		// Token: 0x040028D5 RID: 10453
		protected int FeedbackSizeValue;

		// Token: 0x040028D6 RID: 10454
		protected byte[] IVValue;

		// Token: 0x040028D7 RID: 10455
		protected byte[] KeyValue;

		// Token: 0x040028D8 RID: 10456
		protected KeySizes[] LegalBlockSizesValue;

		// Token: 0x040028D9 RID: 10457
		protected KeySizes[] LegalKeySizesValue;

		// Token: 0x040028DA RID: 10458
		protected int KeySizeValue;

		// Token: 0x040028DB RID: 10459
		protected CipherMode ModeValue;

		// Token: 0x040028DC RID: 10460
		protected PaddingMode PaddingValue;
	}
}
