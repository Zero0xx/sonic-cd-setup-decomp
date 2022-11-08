using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000887 RID: 2183
	[ComVisible(true)]
	public class HMACSHA1 : HMAC
	{
		// Token: 0x06004F89 RID: 20361 RVA: 0x00114B2D File Offset: 0x00113B2D
		public HMACSHA1() : this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x00114B3C File Offset: 0x00113B3C
		public HMACSHA1(byte[] key) : this(key, false)
		{
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x00114B48 File Offset: 0x00113B48
		public HMACSHA1(byte[] key, bool useManagedSha1)
		{
			this.m_hashName = "SHA1";
			if (useManagedSha1)
			{
				this.m_hash1 = new SHA1Managed();
				this.m_hash2 = new SHA1Managed();
			}
			else
			{
				this.m_hash1 = new SHA1CryptoServiceProvider();
				this.m_hash2 = new SHA1CryptoServiceProvider();
			}
			this.HashSizeValue = 160;
			base.InitializeKey(key);
		}
	}
}
