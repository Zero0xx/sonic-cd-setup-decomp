using System;

namespace System.Windows.Forms
{
	// Token: 0x0200069A RID: 1690
	public class ToolStripItemClickedEventArgs : EventArgs
	{
		// Token: 0x06005910 RID: 22800 RVA: 0x00143E18 File Offset: 0x00142E18
		public ToolStripItemClickedEventArgs(ToolStripItem clickedItem)
		{
			this.clickedItem = clickedItem;
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06005911 RID: 22801 RVA: 0x00143E27 File Offset: 0x00142E27
		public ToolStripItem ClickedItem
		{
			get
			{
				return this.clickedItem;
			}
		}

		// Token: 0x0400383A RID: 14394
		private ToolStripItem clickedItem;
	}
}
