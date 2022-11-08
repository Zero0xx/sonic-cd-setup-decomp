using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x0200062B RID: 1579
	public class StatusBarDrawItemEventArgs : DrawItemEventArgs
	{
		// Token: 0x060052D1 RID: 21201 RVA: 0x0012F1A9 File Offset: 0x0012E1A9
		public StatusBarDrawItemEventArgs(Graphics g, Font font, Rectangle r, int itemId, DrawItemState itemState, StatusBarPanel panel) : base(g, font, r, itemId, itemState)
		{
			this.panel = panel;
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x0012F1C0 File Offset: 0x0012E1C0
		public StatusBarDrawItemEventArgs(Graphics g, Font font, Rectangle r, int itemId, DrawItemState itemState, StatusBarPanel panel, Color foreColor, Color backColor) : base(g, font, r, itemId, itemState, foreColor, backColor)
		{
			this.panel = panel;
		}

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x060052D3 RID: 21203 RVA: 0x0012F1DB File Offset: 0x0012E1DB
		public StatusBarPanel Panel
		{
			get
			{
				return this.panel;
			}
		}

		// Token: 0x04003643 RID: 13891
		private readonly StatusBarPanel panel;
	}
}
