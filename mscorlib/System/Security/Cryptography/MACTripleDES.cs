using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200088D RID: 2189
	[ComVisible(true)]
	public class MACTripleDES : KeyedHashAlgorithm
	{
		// Token: 0x06004FA8 RID: 20392 RVA: 0x001151E4 File Offset: 0x001141E4
		public MACTripleDES()
		{
			this.KeyValue = new byte[24];
			Utils.StaticRandomNumberGenerator.GetBytes(this.KeyValue);
			this.des = TripleDES.Create();
			this.HashSizeValue = this.des.BlockSize;
			this.m_bytesPerBlock = this.des.BlockSize / 8;
			this.des.IV = new byte[this.m_bytesPerBlock];
			this.des.Padding = PaddingMode.Zeros;
			this.m_encryptor = null;
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x0011526C File Offset: 0x0011426C
		public MACTripleDES(byte[] rgbKey) : this("System.Security.Cryptography.TripleDES", rgbKey)
		{
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x0011527C File Offset: 0x0011427C
		public MACTripleDES(string strTripleDES, byte[] rgbKey)
		{
			if (rgbKey == null)
			{
				throw new ArgumentNullException("rgbKey");
			}
			if (strTripleDES == null)
			{
				this.des = TripleDES.Create();
			}
			else
			{
				this.des = TripleDES.Create(strTripleDES);
			}
			this.HashSizeValue = this.des.BlockSize;
			this.KeyValue = (byte[])rgbKey.Clone();
			this.m_bytesPerBlock = this.des.BlockSize / 8;
			this.des.IV = new byte[this.m_bytesPerBlock];
			this.des.Padding = PaddingMode.Zeros;
			this.m_encryptor = null;
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x00115317 File Offset: 0x00114317
		public override void Initialize()
		{
			this.m_encryptor = null;
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06004FAC RID: 20396 RVA: 0x00115320 File Offset: 0x00114320
		// (set) Token: 0x06004FAD RID: 20397 RVA: 0x0011532D File Offset: 0x0011432D
		[ComVisible(false)]
		public PaddingMode Padding
		{
			get
			{
				return this.des.Padding;
			}
			set
			{
				if (value < PaddingMode.None || PaddingMode.ISO10126 < value)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidPaddingMode"));
				}
				this.des.Padding = value;
			}
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x00115354 File Offset: 0x00114354
		protected override void HashCore(byte[] rgbData, int ibStart, int cbSize)
		{
			if (this.m_encryptor == null)
			{
				this.des.Key = this.Key;
				this.m_encryptor = this.des.CreateEncryptor();
				this._ts = new TailStream(this.des.BlockSize / 8);
				this._cs = new CryptoStream(this._ts, this.m_encryptor, CryptoStreamMode.Write);
			}
			this._cs.Write(rgbData, ibStart, cbSize);
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x001153CC File Offset: 0x001143CC
		protected override byte[] HashFinal()
		{
			if (this.m_encryptor == null)
			{
				this.des.Key = this.Key;
				this.m_encryptor = this.des.CreateEncryptor();
				this._ts = new TailStream(this.des.BlockSize / 8);
				this._cs = new CryptoStream(this._ts, this.m_encryptor, CryptoStreamMode.Write);
			}
			this._cs.FlushFinalBlock();
			return this._ts.Buffer;
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x0011544C File Offset: 0x0011444C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.des != null)
				{
					this.des.Clear();
				}
				if (this.m_encryptor != null)
				{
					this.m_encryptor.Dispose();
				}
				if (this._cs != null)
				{
					this._cs.Clear();
				}
				if (this._ts != null)
				{
					this._ts.Clear();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400290D RID: 10509
		private const int m_bitsPerByte = 8;

		// Token: 0x0400290E RID: 10510
		private ICryptoTransform m_encryptor;

		// Token: 0x0400290F RID: 10511
		private CryptoStream _cs;

		// Token: 0x04002910 RID: 10512
		private TailStream _ts;

		// Token: 0x04002911 RID: 10513
		private int m_bytesPerBlock;

		// Token: 0x04002912 RID: 10514
		private TripleDES des;
	}
}
