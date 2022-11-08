using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000333 RID: 819
	public sealed class X509ChainElementEnumerator : IEnumerator
	{
		// Token: 0x060019E8 RID: 6632 RVA: 0x0005A4DE File Offset: 0x000594DE
		private X509ChainElementEnumerator()
		{
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0005A4E6 File Offset: 0x000594E6
		internal X509ChainElementEnumerator(X509ChainElementCollection chainElements)
		{
			this.m_chainElements = chainElements;
			this.m_current = -1;
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x0005A4FC File Offset: 0x000594FC
		public X509ChainElement Current
		{
			get
			{
				return this.m_chainElements[this.m_current];
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x0005A50F File Offset: 0x0005950F
		object IEnumerator.Current
		{
			get
			{
				return this.m_chainElements[this.m_current];
			}
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0005A522 File Offset: 0x00059522
		public bool MoveNext()
		{
			if (this.m_current == this.m_chainElements.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x0005A54A File Offset: 0x0005954A
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001AE3 RID: 6883
		private X509ChainElementCollection m_chainElements;

		// Token: 0x04001AE4 RID: 6884
		private int m_current;
	}
}
