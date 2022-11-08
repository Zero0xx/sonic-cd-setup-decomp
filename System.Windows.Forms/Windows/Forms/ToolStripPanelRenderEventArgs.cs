using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006C7 RID: 1735
	public class ToolStripPanelRenderEventArgs : EventArgs
	{
		// Token: 0x06005B5E RID: 23390 RVA: 0x0014A814 File Offset: 0x00149814
		public ToolStripPanelRenderEventArgs(Graphics g, ToolStripPanel toolStripPanel)
		{
			this.toolStripPanel = toolStripPanel;
			this.graphics = g;
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x0014A82A File Offset: 0x0014982A
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x0014A832 File Offset: 0x00149832
		public ToolStripPanel ToolStripPanel
		{
			get
			{
				return this.toolStripPanel;
			}
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x0014A83A File Offset: 0x0014983A
		// (set) Token: 0x06005B62 RID: 23394 RVA: 0x0014A842 File Offset: 0x00149842
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

		// Token: 0x040038E3 RID: 14563
		private ToolStripPanel toolStripPanel;

		// Token: 0x040038E4 RID: 14564
		private Graphics graphics;

		// Token: 0x040038E5 RID: 14565
		private bool handled;
	}
}
