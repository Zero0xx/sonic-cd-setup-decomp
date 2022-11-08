using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006C9 RID: 1737
	public class ToolStripContentPanelRenderEventArgs : EventArgs
	{
		// Token: 0x06005B67 RID: 23399 RVA: 0x0014A84B File Offset: 0x0014984B
		public ToolStripContentPanelRenderEventArgs(Graphics g, ToolStripContentPanel contentPanel)
		{
			this.contentPanel = contentPanel;
			this.graphics = g;
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x06005B68 RID: 23400 RVA: 0x0014A861 File Offset: 0x00149861
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x06005B69 RID: 23401 RVA: 0x0014A869 File Offset: 0x00149869
		// (set) Token: 0x06005B6A RID: 23402 RVA: 0x0014A871 File Offset: 0x00149871
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x06005B6B RID: 23403 RVA: 0x0014A87A File Offset: 0x0014987A
		public ToolStripContentPanel ToolStripContentPanel
		{
			get
			{
				return this.contentPanel;
			}
		}

		// Token: 0x040038E6 RID: 14566
		private ToolStripContentPanel contentPanel;

		// Token: 0x040038E7 RID: 14567
		private Graphics graphics;

		// Token: 0x040038E8 RID: 14568
		private bool handled;
	}
}
