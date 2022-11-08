using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000862 RID: 2146
	[ComVisible(true)]
	public sealed class KeySizes
	{
		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06004E62 RID: 20066 RVA: 0x0010FDDF File Offset: 0x0010EDDF
		public int MinSize
		{
			get
			{
				return this.m_minSize;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06004E63 RID: 20067 RVA: 0x0010FDE7 File Offset: 0x0010EDE7
		public int MaxSize
		{
			get
			{
				return this.m_maxSize;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06004E64 RID: 20068 RVA: 0x0010FDEF File Offset: 0x0010EDEF
		public int SkipSize
		{
			get
			{
				return this.m_skipSize;
			}
		}

		// Token: 0x06004E65 RID: 20069 RVA: 0x0010FDF7 File Offset: 0x0010EDF7
		public KeySizes(int minSize, int maxSize, int skipSize)
		{
			this.m_minSize = minSize;
			this.m_maxSize = maxSize;
			this.m_skipSize = skipSize;
		}

		// Token: 0x0400288F RID: 10383
		private int m_minSize;

		// Token: 0x04002890 RID: 10384
		private int m_maxSize;

		// Token: 0x04002891 RID: 10385
		private int m_skipSize;
	}
}
