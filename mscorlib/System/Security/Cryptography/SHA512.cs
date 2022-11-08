using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020008B1 RID: 2225
	[ComVisible(true)]
	public abstract class SHA512 : HashAlgorithm
	{
		// Token: 0x060050D1 RID: 20689 RVA: 0x001218E9 File Offset: 0x001208E9
		protected SHA512()
		{
			this.HashSizeValue = 512;
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x001218FC File Offset: 0x001208FC
		public new static SHA512 Create()
		{
			return SHA512.Create("System.Security.Cryptography.SHA512");
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x00121908 File Offset: 0x00120908
		public new static SHA512 Create(string hashName)
		{
			return (SHA512)CryptoConfig.CreateFromName(hashName);
		}
	}
}
