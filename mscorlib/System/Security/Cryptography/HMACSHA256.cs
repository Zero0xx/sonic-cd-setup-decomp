using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000888 RID: 2184
	[ComVisible(true)]
	public class HMACSHA256 : HMAC
	{
		// Token: 0x06004F8C RID: 20364 RVA: 0x00114BA9 File Offset: 0x00113BA9
		public HMACSHA256() : this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x00114BB8 File Offset: 0x00113BB8
		public HMACSHA256(byte[] key)
		{
			this.m_hashName = "SHA256";
			this.m_hash1 = new SHA256Managed();
			this.m_hash2 = new SHA256Managed();
			this.HashSizeValue = 256;
			base.InitializeKey(key);
		}
	}
}
