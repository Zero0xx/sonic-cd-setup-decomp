using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000866 RID: 2150
	[ComVisible(true)]
	public abstract class RandomNumberGenerator
	{
		// Token: 0x06004E79 RID: 20089 RVA: 0x0010FF5C File Offset: 0x0010EF5C
		public static RandomNumberGenerator Create()
		{
			return RandomNumberGenerator.Create("System.Security.Cryptography.RandomNumberGenerator");
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x0010FF68 File Offset: 0x0010EF68
		public static RandomNumberGenerator Create(string rngName)
		{
			return (RandomNumberGenerator)CryptoConfig.CreateFromName(rngName);
		}

		// Token: 0x06004E7B RID: 20091
		public abstract void GetBytes(byte[] data);

		// Token: 0x06004E7C RID: 20092
		public abstract void GetNonZeroBytes(byte[] data);
	}
}
