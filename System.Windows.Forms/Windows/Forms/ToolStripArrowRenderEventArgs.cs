using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000673 RID: 1651
	public class ToolStripArrowRenderEventArgs : EventArgs
	{
		// Token: 0x060056A2 RID: 22178 RVA: 0x0013B7BC File Offset: 0x0013A7BC
		public ToolStripArrowRenderEventArgs(Graphics g, ToolStripItem toolStripItem, Rectangle arrowRectangle, Color arrowColor, ArrowDirection arrowDirection)
		{
			this.item = toolStripItem;
			this.graphics = g;
			this.arrowRect = arrowRectangle;
			this.defaultArrowColor = arrowColor;
			this.arrowDirection = arrowDirection;
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x060056A3 RID: 22179 RVA: 0x0013B81D File Offset: 0x0013A81D
		// (set) Token: 0x060056A4 RID: 22180 RVA: 0x0013B825 File Offset: 0x0013A825
		public Rectangle ArrowRectangle
		{
			get
			{
				return this.arrowRect;
			}
			set
			{
				this.arrowRect = value;
			}
		}

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x060056A5 RID: 22181 RVA: 0x0013B82E File Offset: 0x0013A82E
		// (set) Token: 0x060056A6 RID: 22182 RVA: 0x0013B845 File Offset: 0x0013A845
		public Color ArrowColor
		{
			get
			{
				if (this.arrowColorChanged)
				{
					return this.arrowColor;
				}
				return this.DefaultArrowColor;
			}
			set
			{
				this.arrowColor = value;
				this.arrowColorChanged = true;
			}
		}

		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x0013B855 File Offset: 0x0013A855
		// (set) Token: 0x060056A8 RID: 22184 RVA: 0x0013B85D File Offset: 0x0013A85D
		internal Color DefaultArrowColor
		{
			get
			{
				return this.defaultArrowColor;
			}
			set
			{
				this.defaultArrowColor = value;
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x060056A9 RID: 22185 RVA: 0x0013B866 File Offset: 0x0013A866
		// (set) Token: 0x060056AA RID: 22186 RVA: 0x0013B86E File Offset: 0x0013A86E
		public ArrowDirection Direction
		{
			get
			{
				return this.arrowDirection;
			}
			set
			{
				this.arrowDirection = value;
			}
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x0013B877 File Offset: 0x0013A877
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x060056AC RID: 22188 RVA: 0x0013B87F File Offset: 0x0013A87F
		public ToolStripItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x04003778 RID: 14200
		private Graphics graphics;

		// Token: 0x04003779 RID: 14201
		private Rectangle arrowRect = Rectangle.Empty;

		// Token: 0x0400377A RID: 14202
		private Color arrowColor = Color.Empty;

		// Token: 0x0400377B RID: 14203
		private Color defaultArrowColor = Color.Empty;

		// Token: 0x0400377C RID: 14204
		private ArrowDirection arrowDirection = ArrowDirection.Down;

		// Token: 0x0400377D RID: 14205
		private ToolStripItem item;

		// Token: 0x0400377E RID: 14206
		private bool arrowColorChanged;
	}
}
