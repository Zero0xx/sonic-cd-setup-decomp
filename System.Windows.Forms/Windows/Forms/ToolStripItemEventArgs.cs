using System;

namespace System.Windows.Forms
{
	// Token: 0x0200069E RID: 1694
	public class ToolStripItemEventArgs : EventArgs
	{
		// Token: 0x0600593F RID: 22847 RVA: 0x001446BD File Offset: 0x001436BD
		public ToolStripItemEventArgs(ToolStripItem item)
		{
			this.item = item;
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06005940 RID: 22848 RVA: 0x001446CC File Offset: 0x001436CC
		public ToolStripItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x04003844 RID: 14404
		private ToolStripItem item;
	}
}
