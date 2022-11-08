using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020008AD RID: 2221
	[ComVisible(true)]
	public abstract class SHA256 : HashAlgorithm
	{
		// Token: 0x060050A9 RID: 20649 RVA: 0x00120664 File Offset: 0x0011F664
		protected SHA256()
		{
			this.HashSizeValue = 256;
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x00120677 File Offset: 0x0011F677
		public new static SHA256 Create()
		{
			return SHA256.Create("System.Security.Cryptography.SHA256");
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x00120683 File Offset: 0x0011F683
		public new static SHA256 Create(string hashName)
		{
			return (SHA256)CryptoConfig.CreateFromName(hashName);
		}
	}
}
