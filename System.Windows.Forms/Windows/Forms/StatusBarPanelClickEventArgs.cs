using System;

namespace System.Windows.Forms
{
	// Token: 0x02000630 RID: 1584
	public class StatusBarPanelClickEventArgs : MouseEventArgs
	{
		// Token: 0x060052FF RID: 21247 RVA: 0x0012FA37 File Offset: 0x0012EA37
		public StatusBarPanelClickEventArgs(StatusBarPanel statusBarPanel, MouseButtons button, int clicks, int x, int y) : base(button, clicks, x, y, 0)
		{
			this.statusBarPanel = statusBarPanel;
		}

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x06005300 RID: 21248 RVA: 0x0012FA4D File Offset: 0x0012EA4D
		public StatusBarPanel StatusBarPanel
		{
			get
			{
				return this.statusBarPanel;
			}
		}

		// Token: 0x0400365F RID: 13919
		private readonly StatusBarPanel statusBarPanel;
	}
}
