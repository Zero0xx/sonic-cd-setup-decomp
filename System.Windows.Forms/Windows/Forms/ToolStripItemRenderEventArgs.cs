using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006A1 RID: 1697
	public class ToolStripItemRenderEventArgs : EventArgs
	{
		// Token: 0x06005945 RID: 22853 RVA: 0x001446D4 File Offset: 0x001436D4
		public ToolStripItemRenderEventArgs(Graphics g, ToolStripItem item)
		{
			this.item = item;
			this.graphics = g;
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06005946 RID: 22854 RVA: 0x001446EA File Offset: 0x001436EA
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06005947 RID: 22855 RVA: 0x001446F2 File Offset: 0x001436F2
		public ToolStripItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06005948 RID: 22856 RVA: 0x001446FA File Offset: 0x001436FA
		public ToolStrip ToolStrip
		{
			get
			{
				return this.item.ParentInternal;
			}
		}

		// Token: 0x04003850 RID: 14416
		private ToolStripItem item;

		// Token: 0x04003851 RID: 14417
		private Graphics graphics;
	}
}
