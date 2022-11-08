using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000660 RID: 1632
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntryEnumerator : IEnumerator
	{
		// Token: 0x06003AE0 RID: 15072 RVA: 0x000C6C1A File Offset: 0x000C5C1A
		private KeyContainerPermissionAccessEntryEnumerator()
		{
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x000C6C22 File Offset: 0x000C5C22
		internal KeyContainerPermissionAccessEntryEnumerator(KeyContainerPermissionAccessEntryCollection entries)
		{
			this.m_entries = entries;
			this.m_current = -1;
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x000C6C38 File Offset: 0x000C5C38
		public KeyContainerPermissionAccessEntry Current
		{
			get
			{
				return this.m_entries[this.m_current];
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x000C6C4B File Offset: 0x000C5C4B
		object IEnumerator.Current
		{
			get
			{
				return this.m_entries[this.m_current];
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x000C6C5E File Offset: 0x000C5C5E
		public bool MoveNext()
		{
			if (this.m_current == this.m_entries.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x000C6C86 File Offset: 0x000C5C86
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001E81 RID: 7809
		private KeyContainerPermissionAccessEntryCollection m_entries;

		// Token: 0x04001E82 RID: 7810
		private int m_current;
	}
}
