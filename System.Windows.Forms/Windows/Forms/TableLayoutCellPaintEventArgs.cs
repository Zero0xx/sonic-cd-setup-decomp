using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000656 RID: 1622
	public class TableLayoutCellPaintEventArgs : PaintEventArgs
	{
		// Token: 0x06005534 RID: 21812 RVA: 0x001369D7 File Offset: 0x001359D7
		public TableLayoutCellPaintEventArgs(Graphics g, Rectangle clipRectangle, Rectangle cellBounds, int column, int row) : base(g, clipRectangle)
		{
			this.bounds = cellBounds;
			this.row = row;
			this.column = column;
		}

		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x06005535 RID: 21813 RVA: 0x001369F8 File Offset: 0x001359F8
		public Rectangle CellBounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x06005536 RID: 21814 RVA: 0x00136A00 File Offset: 0x00135A00
		public int Row
		{
			get
			{
				return this.row;
			}
		}

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x06005537 RID: 21815 RVA: 0x00136A08 File Offset: 0x00135A08
		public int Column
		{
			get
			{
				return this.column;
			}
		}

		// Token: 0x04003701 RID: 14081
		private Rectangle bounds;

		// Token: 0x04003702 RID: 14082
		private int row;

		// Token: 0x04003703 RID: 14083
		private int column;
	}
}
