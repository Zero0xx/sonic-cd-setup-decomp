using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000867 RID: 2151
	[ComVisible(true)]
	public sealed class RNGCryptoServiceProvider : RandomNumberGenerator
	{
		// Token: 0x06004E7D RID: 20093 RVA: 0x0010FF75 File Offset: 0x0010EF75
		public RNGCryptoServiceProvider() : this(null)
		{
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x0010FF7E File Offset: 0x0010EF7E
		public RNGCryptoServiceProvider(string str) : this(null)
		{
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x0010FF87 File Offset: 0x0010EF87
		public RNGCryptoServiceProvider(byte[] rgb) : this(null)
		{
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x0010FF90 File Offset: 0x0010EF90
		public RNGCryptoServiceProvider(CspParameters cspParams)
		{
			if (cspParams != null)
			{
				this.m_safeProvHandle = Utils.AcquireProvHandle(cspParams);
				return;
			}
			this.m_safeProvHandle = Utils.StaticProvHandle;
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x0010FFB3 File Offset: 0x0010EFB3
		public override void GetBytes(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			Utils._GetBytes(this.m_safeProvHandle, data);
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x0010FFCF File Offset: 0x0010EFCF
		public override void GetNonZeroBytes(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			Utils._GetNonZeroBytes(this.m_safeProvHandle, data);
		}

		// Token: 0x04002895 RID: 10389
		private SafeProvHandle m_safeProvHandle;
	}
}
