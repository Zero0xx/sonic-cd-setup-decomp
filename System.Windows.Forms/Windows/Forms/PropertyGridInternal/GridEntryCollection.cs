using System;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007B3 RID: 1971
	internal class GridEntryCollection : GridItemCollection
	{
		// Token: 0x0600685A RID: 26714 RVA: 0x0017E112 File Offset: 0x0017D112
		public GridEntryCollection(GridEntry owner, GridEntry[] entries) : base(entries)
		{
			this.owner = owner;
		}

		// Token: 0x0600685B RID: 26715 RVA: 0x0017E124 File Offset: 0x0017D124
		public void AddRange(GridEntry[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.entries != null)
			{
				GridEntry[] array = new GridEntry[this.entries.Length + value.Length];
				this.entries.CopyTo(array, 0);
				value.CopyTo(array, this.entries.Length);
				this.entries = array;
				return;
			}
			this.entries = (GridEntry[])value.Clone();
		}

		// Token: 0x0600685C RID: 26716 RVA: 0x0017E18E File Offset: 0x0017D18E
		public void Clear()
		{
			this.entries = new GridEntry[0];
		}

		// Token: 0x0600685D RID: 26717 RVA: 0x0017E19C File Offset: 0x0017D19C
		public void CopyTo(Array dest, int index)
		{
			this.entries.CopyTo(dest, index);
		}

		// Token: 0x0600685E RID: 26718 RVA: 0x0017E1AB File Offset: 0x0017D1AB
		internal GridEntry GetEntry(int index)
		{
			return (GridEntry)this.entries[index];
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x0017E1BA File Offset: 0x0017D1BA
		internal int GetEntry(GridEntry child)
		{
			return Array.IndexOf(this.entries, child);
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x0017E1C8 File Offset: 0x0017D1C8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x0017E1D8 File Offset: 0x0017D1D8
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.owner != null && this.entries != null)
			{
				for (int i = 0; i < this.entries.Length; i++)
				{
					if (this.entries[i] != null)
					{
						((GridEntry)this.entries[i]).Dispose();
						this.entries[i] = null;
					}
				}
				this.entries = new GridEntry[0];
			}
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x0017E23C File Offset: 0x0017D23C
		~GridEntryCollection()
		{
			this.Dispose(false);
		}

		// Token: 0x04003D6F RID: 15727
		private GridEntry owner;
	}
}
