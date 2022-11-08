using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000884 RID: 2180
	[ComVisible(true)]
	public abstract class HMAC : KeyedHashAlgorithm
	{
		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06004F76 RID: 20342 RVA: 0x00114727 File Offset: 0x00113727
		// (set) Token: 0x06004F77 RID: 20343 RVA: 0x0011472F File Offset: 0x0011372F
		protected int BlockSizeValue
		{
			get
			{
				return this.blockSizeValue;
			}
			set
			{
				this.blockSizeValue = value;
			}
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x00114738 File Offset: 0x00113738
		private void UpdateIOPadBuffers()
		{
			if (this.m_inner == null)
			{
				this.m_inner = new byte[this.BlockSizeValue];
			}
			if (this.m_outer == null)
			{
				this.m_outer = new byte[this.BlockSizeValue];
			}
			for (int i = 0; i < this.BlockSizeValue; i++)
			{
				this.m_inner[i] = 54;
				this.m_outer[i] = 92;
			}
			for (int i = 0; i < this.KeyValue.Length; i++)
			{
				byte[] inner = this.m_inner;
				int num = i;
				inner[num] ^= this.KeyValue[i];
				byte[] outer = this.m_outer;
				int num2 = i;
				outer[num2] ^= this.KeyValue[i];
			}
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x001147F4 File Offset: 0x001137F4
		internal void InitializeKey(byte[] key)
		{
			this.m_inner = null;
			this.m_outer = null;
			if (key.Length > this.BlockSizeValue)
			{
				this.KeyValue = this.m_hash1.ComputeHash(key);
			}
			else
			{
				this.KeyValue = (byte[])key.Clone();
			}
			this.UpdateIOPadBuffers();
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06004F7A RID: 20346 RVA: 0x00114845 File Offset: 0x00113845
		// (set) Token: 0x06004F7B RID: 20347 RVA: 0x00114857 File Offset: 0x00113857
		public override byte[] Key
		{
			get
			{
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (this.m_hashing)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_HashKeySet"));
				}
				this.InitializeKey(value);
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06004F7C RID: 20348 RVA: 0x00114878 File Offset: 0x00113878
		// (set) Token: 0x06004F7D RID: 20349 RVA: 0x00114880 File Offset: 0x00113880
		public string HashName
		{
			get
			{
				return this.m_hashName;
			}
			set
			{
				if (this.m_hashing)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_HashNameSet"));
				}
				this.m_hashName = value;
				this.m_hash1 = HashAlgorithm.Create(this.m_hashName);
				this.m_hash2 = HashAlgorithm.Create(this.m_hashName);
			}
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x001148CE File Offset: 0x001138CE
		public new static HMAC Create()
		{
			return HMAC.Create("System.Security.Cryptography.HMAC");
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x001148DA File Offset: 0x001138DA
		public new static HMAC Create(string algorithmName)
		{
			return (HMAC)CryptoConfig.CreateFromName(algorithmName);
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x001148E7 File Offset: 0x001138E7
		public override void Initialize()
		{
			this.m_hash1.Initialize();
			this.m_hash2.Initialize();
			this.m_hashing = false;
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x00114908 File Offset: 0x00113908
		protected override void HashCore(byte[] rgb, int ib, int cb)
		{
			if (!this.m_hashing)
			{
				this.m_hash1.TransformBlock(this.m_inner, 0, this.m_inner.Length, this.m_inner, 0);
				this.m_hashing = true;
			}
			this.m_hash1.TransformBlock(rgb, ib, cb, rgb, ib);
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x00114958 File Offset: 0x00113958
		protected override byte[] HashFinal()
		{
			if (!this.m_hashing)
			{
				this.m_hash1.TransformBlock(this.m_inner, 0, this.m_inner.Length, this.m_inner, 0);
				this.m_hashing = true;
			}
			this.m_hash1.TransformFinalBlock(new byte[0], 0, 0);
			byte[] hashValue = this.m_hash1.HashValue;
			this.m_hash2.TransformBlock(this.m_outer, 0, this.m_outer.Length, this.m_outer, 0);
			this.m_hash2.TransformBlock(hashValue, 0, hashValue.Length, hashValue, 0);
			this.m_hashing = false;
			this.m_hash2.TransformFinalBlock(new byte[0], 0, 0);
			return this.m_hash2.HashValue;
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x00114A14 File Offset: 0x00113A14
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_hash1 != null)
				{
					this.m_hash1.Clear();
				}
				if (this.m_hash2 != null)
				{
					this.m_hash2.Clear();
				}
				if (this.m_inner != null)
				{
					Array.Clear(this.m_inner, 0, this.m_inner.Length);
				}
				if (this.m_outer != null)
				{
					Array.Clear(this.m_outer, 0, this.m_outer.Length);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x040028FF RID: 10495
		private int blockSizeValue = 64;

		// Token: 0x04002900 RID: 10496
		internal string m_hashName;

		// Token: 0x04002901 RID: 10497
		internal HashAlgorithm m_hash1;

		// Token: 0x04002902 RID: 10498
		internal HashAlgorithm m_hash2;

		// Token: 0x04002903 RID: 10499
		private byte[] m_inner;

		// Token: 0x04002904 RID: 10500
		private byte[] m_outer;

		// Token: 0x04002905 RID: 10501
		private bool m_hashing;
	}
}
