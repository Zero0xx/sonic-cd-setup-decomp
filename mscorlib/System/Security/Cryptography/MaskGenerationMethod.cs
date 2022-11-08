using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000891 RID: 2193
	[ComVisible(true)]
	public abstract class MaskGenerationMethod
	{
		// Token: 0x06004FC8 RID: 20424
		[ComVisible(true)]
		public abstract byte[] GenerateMask(byte[] rgbSeed, int cbReturn);
	}
}
