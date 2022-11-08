using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006A2 RID: 1698
	public class ToolStripItemImageRenderEventArgs : ToolStripItemRenderEventArgs
	{
		// Token: 0x06005949 RID: 22857 RVA: 0x00144708 File Offset: 0x00143708
		public ToolStripItemImageRenderEventArgs(Graphics g, ToolStripItem item, Rectangle imageRectangle) : base(g, item)
		{
			this.image = ((item.RightToLeftAutoMirrorImage && item.RightToLeft == RightToLeft.Yes) ? item.MirroredImage : item.Image);
			this.imageRectangle = imageRectangle;
		}

		// Token: 0x0600594A RID: 22858 RVA: 0x00144754 File Offset: 0x00143754
		public ToolStripItemImageRenderEventArgs(Graphics g, ToolStripItem item, Image image, Rectangle imageRectangle) : base(g, item)
		{
			this.image = image;
			this.imageRectangle = imageRectangle;
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x0600594B RID: 22859 RVA: 0x00144778 File Offset: 0x00143778
		public Image Image
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x0600594C RID: 22860 RVA: 0x00144780 File Offset: 0x00143780
		public Rectangle ImageRectangle
		{
			get
			{
				return this.imageRectangle;
			}
		}

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x0600594D RID: 22861 RVA: 0x00144788 File Offset: 0x00143788
		// (set) Token: 0x0600594E RID: 22862 RVA: 0x00144790 File Offset: 0x00143790
		internal bool ShiftOnPress
		{
			get
			{
				return this.shiftOnPress;
			}
			set
			{
				this.shiftOnPress = value;
			}
		}

		// Token: 0x04003852 RID: 14418
		private Image image;

		// Token: 0x04003853 RID: 14419
		private Rectangle imageRectangle = Rectangle.Empty;

		// Token: 0x04003854 RID: 14420
		private bool shiftOnPress;
	}
}
