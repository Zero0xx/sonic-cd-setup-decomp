using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020008AA RID: 2218
	[ComVisible(true)]
	public abstract class SHA1 : HashAlgorithm
	{
		// Token: 0x06005098 RID: 20632 RVA: 0x0011FDD4 File Offset: 0x0011EDD4
		protected SHA1()
		{
			this.HashSizeValue = 160;
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x0011FDE7 File Offset: 0x0011EDE7
		public new static SHA1 Create()
		{
			return SHA1.Create("System.Security.Cryptography.SHA1");
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x0011FDF3 File Offset: 0x0011EDF3
		public new static SHA1 Create(string hashName)
		{
			return (SHA1)CryptoConfig.CreateFromName(hashName);
		}
	}
}
