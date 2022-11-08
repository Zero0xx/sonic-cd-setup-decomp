using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000889 RID: 2185
	[ComVisible(true)]
	public class HMACSHA384 : HMAC
	{
		// Token: 0x06004F8E RID: 20366 RVA: 0x00114BF3 File Offset: 0x00113BF3
		public HMACSHA384() : this(Utils.GenerateRandom(128))
		{
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x00114C08 File Offset: 0x00113C08
		public HMACSHA384(byte[] key)
		{
			Utils._ShowLegacyHmacWarning();
			this.m_hashName = "SHA384";
			this.m_hash1 = new SHA384Managed();
			this.m_hash2 = new SHA384Managed();
			this.HashSizeValue = 384;
			base.BlockSizeValue = this.BlockSize;
			base.InitializeKey(key);
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06004F90 RID: 20368 RVA: 0x00114C6A File Offset: 0x00113C6A
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

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06004F91 RID: 20369 RVA: 0x00114C7C File Offset: 0x00113C7C
		// (set) Token: 0x06004F92 RID: 20370 RVA: 0x00114C84 File Offset: 0x00113C84
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

		// Token: 0x04002906 RID: 10502
		private bool m_useLegacyBlockSize = Utils._ProduceLegacyHmacValues();
	}
}
