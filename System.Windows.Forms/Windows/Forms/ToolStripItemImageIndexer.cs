using System;

namespace System.Windows.Forms
{
	// Token: 0x02000698 RID: 1688
	internal class ToolStripItemImageIndexer : ImageList.Indexer
	{
		// Token: 0x0600590D RID: 22797 RVA: 0x00143DDE File Offset: 0x00142DDE
		public ToolStripItemImageIndexer(ToolStripItem item)
		{
			this.item = item;
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x0600590E RID: 22798 RVA: 0x00143DED File Offset: 0x00142DED
		// (set) Token: 0x0600590F RID: 22799 RVA: 0x00143E16 File Offset: 0x00142E16
		public override ImageList ImageList
		{
			get
			{
				if (this.item != null && this.item.Owner != null)
				{
					return this.item.Owner.ImageList;
				}
				return null;
			}
			set
			{
			}
		}

		// Token: 0x04003836 RID: 14390
		private ToolStripItem item;
	}
}
