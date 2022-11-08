using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000885 RID: 2181
	[ComVisible(true)]
	public class HMACMD5 : HMAC
	{
		// Token: 0x06004F85 RID: 20357 RVA: 0x00114A99 File Offset: 0x00113A99
		public HMACMD5() : this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x00114AA8 File Offset: 0x00113AA8
		public HMACMD5(byte[] key)
		{
			this.m_hashName = "MD5";
			this.m_hash1 = new MD5CryptoServiceProvider();
			this.m_hash2 = new MD5CryptoServiceProvider();
			this.HashSizeValue = 128;
			base.InitializeKey(key);
		}
	}
}
