using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000694 RID: 1684
	public class ToolStripRenderEventArgs : EventArgs
	{
		// Token: 0x060058FE RID: 22782 RVA: 0x00143B74 File Offset: 0x00142B74
		public ToolStripRenderEventArgs(Graphics g, ToolStrip toolStrip)
		{
			this.toolStrip = toolStrip;
			this.graphics = g;
			this.affectedBounds = new Rectangle(Point.Empty, toolStrip.Size);
		}

		// Token: 0x060058FF RID: 22783 RVA: 0x00143BC1 File Offset: 0x00142BC1
		public ToolStripRenderEventArgs(Graphics g, ToolStrip toolStrip, Rectangle affectedBounds, Color backColor)
		{
			this.toolStrip = toolStrip;
			this.affectedBounds = affectedBounds;
			this.graphics = g;
			this.backColor = backColor;
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06005900 RID: 22784 RVA: 0x00143BFC File Offset: 0x00142BFC
		public Rectangle AffectedBounds
		{
			get
			{
				return this.affectedBounds;
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06005901 RID: 22785 RVA: 0x00143C04 File Offset: 0x00142C04
		public Color BackColor
		{
			get
			{
				if (this.backColor == Color.Empty)
				{
					this.backColor = this.toolStrip.RawBackColor;
					if (this.backColor == Color.Empty)
					{
						if (this.toolStrip is ToolStripDropDown)
						{
							this.backColor = SystemColors.Menu;
						}
						else if (this.toolStrip is MenuStrip)
						{
							this.backColor = SystemColors.MenuBar;
						}
						else
						{
							this.backColor = SystemColors.Control;
						}
					}
				}
				return this.backColor;
			}
		}

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06005902 RID: 22786 RVA: 0x00143C8B File Offset: 0x00142C8B
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06005903 RID: 22787 RVA: 0x00143C93 File Offset: 0x00142C93
		public ToolStrip ToolStrip
		{
			get
			{
				return this.toolStrip;
			}
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06005904 RID: 22788 RVA: 0x00143C9C File Offset: 0x00142C9C
		public Rectangle ConnectedArea
		{
			get
			{
				ToolStripDropDown toolStripDropDown = this.toolStrip as ToolStripDropDown;
				if (toolStripDropDown != null)
				{
					ToolStripDropDownItem toolStripDropDownItem = toolStripDropDown.OwnerItem as ToolStripDropDownItem;
					if (toolStripDropDownItem is MdiControlStrip.SystemMenuItem)
					{
						return Rectangle.Empty;
					}
					if (toolStripDropDownItem != null && toolStripDropDownItem.ParentInternal != null && !toolStripDropDownItem.IsOnDropDown)
					{
						Rectangle rect = new Rectangle(this.toolStrip.PointToClient(toolStripDropDownItem.TranslatePoint(Point.Empty, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords)), toolStripDropDownItem.Size);
						Rectangle bounds = this.ToolStrip.Bounds;
						Rectangle clientRectangle = this.ToolStrip.ClientRectangle;
						clientRectangle.Inflate(1, 1);
						if (clientRectangle.IntersectsWith(rect))
						{
							switch (toolStripDropDownItem.DropDownDirection)
							{
							case ToolStripDropDownDirection.AboveLeft:
							case ToolStripDropDownDirection.AboveRight:
								return Rectangle.Empty;
							case ToolStripDropDownDirection.BelowLeft:
							case ToolStripDropDownDirection.BelowRight:
								clientRectangle.Intersect(rect);
								if (clientRectangle.Height == 2)
								{
									return new Rectangle(rect.X + 1, 0, rect.Width - 2, 2);
								}
								return Rectangle.Empty;
							case ToolStripDropDownDirection.Left:
							case ToolStripDropDownDirection.Right:
								return Rectangle.Empty;
							}
						}
					}
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x0400382F RID: 14383
		private ToolStrip toolStrip;

		// Token: 0x04003830 RID: 14384
		private Graphics graphics;

		// Token: 0x04003831 RID: 14385
		private Rectangle affectedBounds = Rectangle.Empty;

		// Token: 0x04003832 RID: 14386
		private Color backColor = Color.Empty;
	}
}
