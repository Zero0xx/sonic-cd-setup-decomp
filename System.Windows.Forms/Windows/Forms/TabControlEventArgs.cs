using System;

namespace System.Windows.Forms
{
	// Token: 0x02000643 RID: 1603
	public class TabControlEventArgs : EventArgs
	{
		// Token: 0x0600547E RID: 21630 RVA: 0x001343A6 File Offset: 0x001333A6
		public TabControlEventArgs(TabPage tabPage, int tabPageIndex, TabControlAction action)
		{
			this.tabPage = tabPage;
			this.tabPageIndex = tabPageIndex;
			this.action = action;
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x0600547F RID: 21631 RVA: 0x001343C3 File Offset: 0x001333C3
		public TabPage TabPage
		{
			get
			{
				return this.tabPage;
			}
		}

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x06005480 RID: 21632 RVA: 0x001343CB File Offset: 0x001333CB
		public int TabPageIndex
		{
			get
			{
				return this.tabPageIndex;
			}
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x06005481 RID: 21633 RVA: 0x001343D3 File Offset: 0x001333D3
		public TabControlAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x040036D4 RID: 14036
		private TabPage tabPage;

		// Token: 0x040036D5 RID: 14037
		private int tabPageIndex;

		// Token: 0x040036D6 RID: 14038
		private TabControlAction action;
	}
}
