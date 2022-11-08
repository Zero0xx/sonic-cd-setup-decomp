using System;

namespace System.Windows.Forms
{
	// Token: 0x0200067D RID: 1661
	public class ToolStripDropDownClosedEventArgs : EventArgs
	{
		// Token: 0x060057C7 RID: 22471 RVA: 0x0013D6A0 File Offset: 0x0013C6A0
		public ToolStripDropDownClosedEventArgs(ToolStripDropDownCloseReason reason)
		{
			this.closeReason = reason;
		}

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x060057C8 RID: 22472 RVA: 0x0013D6AF File Offset: 0x0013C6AF
		public ToolStripDropDownCloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
		}

		// Token: 0x040037A1 RID: 14241
		private ToolStripDropDownCloseReason closeReason;
	}
}
