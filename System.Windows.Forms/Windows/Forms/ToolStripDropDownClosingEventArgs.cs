using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200067F RID: 1663
	public class ToolStripDropDownClosingEventArgs : CancelEventArgs
	{
		// Token: 0x060057CD RID: 22477 RVA: 0x0013D6B7 File Offset: 0x0013C6B7
		public ToolStripDropDownClosingEventArgs(ToolStripDropDownCloseReason reason)
		{
			this.closeReason = reason;
		}

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x060057CE RID: 22478 RVA: 0x0013D6C6 File Offset: 0x0013C6C6
		public ToolStripDropDownCloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
		}

		// Token: 0x040037A2 RID: 14242
		private ToolStripDropDownCloseReason closeReason;
	}
}
