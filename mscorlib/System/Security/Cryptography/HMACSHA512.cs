using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200088A RID: 2186
	[ComVisible(true)]
	public class HMACSHA512 : HMAC
	{
		// Token: 0x06004F93 RID: 20371 RVA: 0x00114CA5 File Offset: 0x00113CA5
		public HMACSHA512() : this(Utils.GenerateRandom(128))
		{
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x00114CB8 File Offset: 0x00113CB8
		public HMACSHA512(byte[] key)
		{
			Utils._ShowLegacyHmacWarning();
			this.m_hashName = "SHA512";
			this.m_hash1 = new SHA512Managed();
			this.m_hash2 = new SHA512Managed();
			this.HashSizeValue = 512;
			base.BlockSizeValue = this.BlockSize;
			base.InitializeKey(key);
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06004F95 RID: 20373 RVA: 0x00114D1A File Offset: 0x00113D1A
		private int BlockSize
		{
			get
			{
				if (!this.m_useLegacyBlockSize)
				{
					return 128;
				}
				return 64;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06004F96 RID: 20374 RVA: 0x00114D2C File Offset: 0x00113D2C
		// (set) Token: 0x06004F97 RID: 20375 RVA: 0x00114D34 File Offset: 0x00113D34
		public bool ProduceLegacyHmacValues
		{
			get
			{
				return this.m_useLegacyBlockSize;
			}
			set
			{
				this.m_useLegacyBlockSize = value;
				base.BlockSizeValue = this.BlockSize;
				base.InitializeKey(this.KeyValue);
			}
		}

		// Token: 0x04002907 RID: 10503
		private bool m_useLegacyBlockSize = Utils._ProduceLegacyHmacValues();
	}
}
