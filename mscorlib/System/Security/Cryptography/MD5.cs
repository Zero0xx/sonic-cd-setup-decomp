using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200088F RID: 2191
	[ComVisible(true)]
	public abstract class MD5 : HashAlgorithm
	{
		// Token: 0x06004FC0 RID: 20416 RVA: 0x001156C6 File Offset: 0x001146C6
		protected MD5()
		{
			this.HashSizeValue = 128;
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x001156D9 File Offset: 0x001146D9
		public new static MD5 Create()
		{
			return MD5.Create("System.Security.Cryptography.MD5");
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x001156E5 File Offset: 0x001146E5
		public new static MD5 Create(string algName)
		{
			return (MD5)CryptoConfig.CreateFromName(algName);
		}
	}
}
