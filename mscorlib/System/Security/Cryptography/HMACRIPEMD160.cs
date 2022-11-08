using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000886 RID: 2182
	[ComVisible(true)]
	public class HMACRIPEMD160 : HMAC
	{
		// Token: 0x06004F87 RID: 20359 RVA: 0x00114AE3 File Offset: 0x00113AE3
		public HMACRIPEMD160() : this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x00114AF2 File Offset: 0x00113AF2
		public HMACRIPEMD160(byte[] key)
		{
			this.m_hashName = "RIPEMD160";
			this.m_hash1 = new RIPEMD160Managed();
			this.m_hash2 = new RIPEMD160Managed();
			this.HashSizeValue = 160;
			base.InitializeKey(key);
		}
	}
}
