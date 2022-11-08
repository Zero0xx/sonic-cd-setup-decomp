using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x0200049C RID: 1180
	[ComVisible(true)]
	public sealed class ApplicationTrustEnumerator : IEnumerator
	{
		// Token: 0x06002EC9 RID: 11977 RVA: 0x0009E1A1 File Offset: 0x0009D1A1
		private ApplicationTrustEnumerator()
		{
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x0009E1A9 File Offset: 0x0009D1A9
		internal ApplicationTrustEnumerator(ApplicationTrustCollection trusts)
		{
			this.m_trusts = trusts;
			this.m_current = -1;
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002ECB RID: 11979 RVA: 0x0009E1BF File Offset: 0x0009D1BF
		public ApplicationTrust Current
		{
			get
			{
				return this.m_trusts[this.m_current];
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002ECC RID: 11980 RVA: 0x0009E1D2 File Offset: 0x0009D1D2
		object IEnumerator.Current
		{
			get
			{
				return this.m_trusts[this.m_current];
			}
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x0009E1E5 File Offset: 0x0009D1E5
		public bool MoveNext()
		{
			if (this.m_current == this.m_trusts.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x0009E20D File Offset: 0x0009D20D
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x040017E9 RID: 6121
		private ApplicationTrustCollection m_trusts;

		// Token: 0x040017EA RID: 6122
		private int m_current;
	}
}
