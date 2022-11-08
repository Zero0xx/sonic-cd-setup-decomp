using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000750 RID: 1872
	public class EventLogEntryCollection : ICollection, IEnumerable
	{
		// Token: 0x06003964 RID: 14692 RVA: 0x000F3F4B File Offset: 0x000F2F4B
		internal EventLogEntryCollection(EventLog log)
		{
			this.log = log;
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x000F3F5A File Offset: 0x000F2F5A
		public int Count
		{
			get
			{
				return this.log.EntryCount;
			}
		}

		// Token: 0x17000D50 RID: 3408
		public virtual EventLogEntry this[int index]
		{
			get
			{
				return this.log.GetEntryAt(index);
			}
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x000F3F75 File Offset: 0x000F2F75
		public void CopyTo(EventLogEntry[] entries, int index)
		{
			((ICollection)this).CopyTo(entries, index);
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x000F3F7F File Offset: 0x000F2F7F
		public IEnumerator GetEnumerator()
		{
			return new EventLogEntryCollection.EntriesEnumerator(this);
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x000F3F87 File Offset: 0x000F2F87
		internal EventLogEntry GetEntryAtNoThrow(int index)
		{
			return this.log.GetEntryAtNoThrow(index);
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x000F3F95 File Offset: 0x000F2F95
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x000F3F98 File Offset: 0x000F2F98
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x000F3F9C File Offset: 0x000F2F9C
		void ICollection.CopyTo(Array array, int index)
		{
			EventLogEntry[] allEntries = this.log.GetAllEntries();
			Array.Copy(allEntries, 0, array, index, allEntries.Length);
		}

		// Token: 0x040032A7 RID: 12967
		private EventLog log;

		// Token: 0x02000751 RID: 1873
		private class EntriesEnumerator : IEnumerator
		{
			// Token: 0x0600396D RID: 14701 RVA: 0x000F3FC1 File Offset: 0x000F2FC1
			internal EntriesEnumerator(EventLogEntryCollection entries)
			{
				this.entries = entries;
			}

			// Token: 0x17000D53 RID: 3411
			// (get) Token: 0x0600396E RID: 14702 RVA: 0x000F3FD7 File Offset: 0x000F2FD7
			public object Current
			{
				get
				{
					if (this.cachedEntry == null)
					{
						throw new InvalidOperationException(SR.GetString("NoCurrentEntry"));
					}
					return this.cachedEntry;
				}
			}

			// Token: 0x0600396F RID: 14703 RVA: 0x000F3FF7 File Offset: 0x000F2FF7
			public bool MoveNext()
			{
				this.num++;
				this.cachedEntry = this.entries.GetEntryAtNoThrow(this.num);
				return this.cachedEntry != null;
			}

			// Token: 0x06003970 RID: 14704 RVA: 0x000F402A File Offset: 0x000F302A
			public void Reset()
			{
				this.num = -1;
			}

			// Token: 0x040032A8 RID: 12968
			private EventLogEntryCollection entries;

			// Token: 0x040032A9 RID: 12969
			private int num = -1;

			// Token: 0x040032AA RID: 12970
			private EventLogEntry cachedEntry;
		}
	}
}
