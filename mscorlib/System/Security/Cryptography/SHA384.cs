using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020008AF RID: 2223
	[ComVisible(true)]
	public abstract class SHA384 : HashAlgorithm
	{
		// Token: 0x060050BD RID: 20669 RVA: 0x00120EC1 File Offset: 0x0011FEC1
		protected SHA384()
		{
			this.HashSizeValue = 384;
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x00120ED4 File Offset: 0x0011FED4
		public new static SHA384 Create()
		{
			return SHA384.Create("System.Security.Cryptography.SHA384");
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x00120EE0 File Offset: 0x0011FEE0
		public new static SHA384 Create(string hashName)
		{
			return (SHA384)CryptoConfig.CreateFromName(hashName);
		}
	}
}
