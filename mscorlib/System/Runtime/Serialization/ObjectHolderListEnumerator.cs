using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036A RID: 874
	internal class ObjectHolderListEnumerator
	{
		// Token: 0x0600227C RID: 8828 RVA: 0x00057052 File Offset: 0x00056052
		internal ObjectHolderListEnumerator(ObjectHolderList list, bool isFixupEnumerator)
		{
			this.m_list = list;
			this.m_startingVersion = this.m_list.Version;
			this.m_currPos = -1;
			this.m_isFixupEnumerator = isFixupEnumerator;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x00057080 File Offset: 0x00056080
		internal bool MoveNext()
		{
			if (this.m_isFixupEnumerator)
			{
				while (++this.m_currPos < this.m_list.Count && this.m_list.m_values[this.m_currPos].CompletelyFixed)
				{
				}
				return this.m_currPos != this.m_list.Count;
			}
			this.m_currPos++;
			return this.m_currPos != this.m_list.Count;
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x00057107 File Offset: 0x00056107
		internal ObjectHolder Current
		{
			get
			{
				return this.m_list.m_values[this.m_currPos];
			}
		}

		// Token: 0x04000E83 RID: 3715
		private bool m_isFixupEnumerator;

		// Token: 0x04000E84 RID: 3716
		private ObjectHolderList m_list;

		// Token: 0x04000E85 RID: 3717
		private int m_startingVersion;

		// Token: 0x04000E86 RID: 3718
		private int m_currPos;
	}
}
