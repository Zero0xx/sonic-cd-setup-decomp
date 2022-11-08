using System;
using System.Collections;

namespace System.Security.Policy
{
	// Token: 0x020004A1 RID: 1185
	internal sealed class EvidenceEnumerator : IEnumerator
	{
		// Token: 0x06002F12 RID: 12050 RVA: 0x0009F86A File Offset: 0x0009E86A
		public EvidenceEnumerator(Evidence evidence)
		{
			this.m_evidence = evidence;
			this.Reset();
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x0009F880 File Offset: 0x0009E880
		public bool MoveNext()
		{
			if (this.m_enumerator == null)
			{
				return false;
			}
			if (this.m_enumerator.MoveNext())
			{
				return true;
			}
			if (this.m_first)
			{
				this.m_enumerator = this.m_evidence.GetAssemblyEnumerator();
				this.m_first = false;
				return this.m_enumerator != null && this.m_enumerator.MoveNext();
			}
			return false;
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002F14 RID: 12052 RVA: 0x0009F8DD File Offset: 0x0009E8DD
		public object Current
		{
			get
			{
				if (this.m_enumerator == null)
				{
					return null;
				}
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x0009F8F4 File Offset: 0x0009E8F4
		public void Reset()
		{
			this.m_first = true;
			if (this.m_evidence != null)
			{
				this.m_enumerator = this.m_evidence.GetHostEnumerator();
				return;
			}
			this.m_enumerator = null;
		}

		// Token: 0x040017F8 RID: 6136
		private bool m_first;

		// Token: 0x040017F9 RID: 6137
		private Evidence m_evidence;

		// Token: 0x040017FA RID: 6138
		private IEnumerator m_enumerator;
	}
}
