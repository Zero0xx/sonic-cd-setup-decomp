using System;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006B7 RID: 1719
	internal class ToolStripMenuItemInternalLayout : ToolStripItemInternalLayout
	{
		// Token: 0x06005A17 RID: 23063 RVA: 0x001474F5 File Offset: 0x001464F5
		public ToolStripMenuItemInternalLayout(ToolStripMenuItem ownerItem) : base(ownerItem)
		{
			this.ownerItem = ownerItem;
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06005A18 RID: 23064 RVA: 0x00147508 File Offset: 0x00146508
		public bool ShowCheckMargin
		{
			get
			{
				ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
				return toolStripDropDownMenu != null && toolStripDropDownMenu.ShowCheckMargin;
			}
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06005A19 RID: 23065 RVA: 0x00147534 File Offset: 0x00146534
		public bool ShowImageMargin
		{
			get
			{
				ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
				return toolStripDropDownMenu != null && toolStripDropDownMenu.ShowImageMargin;
			}
		}

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06005A1A RID: 23066 RVA: 0x0014755D File Offset: 0x0014655D
		public bool PaintCheck
		{
			get
			{
				return this.ShowCheckMargin || this.ShowImageMargin;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06005A1B RID: 23067 RVA: 0x0014756F File Offset: 0x0014656F
		public bool PaintImage
		{
			get
			{
				return this.ShowImageMargin;
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06005A1C RID: 23068 RVA: 0x00147578 File Offset: 0x00146578
		public Rectangle ArrowRectangle
		{
			get
			{
				if (this.UseMenuLayout)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						Rectangle arrowRectangle = toolStripDropDownMenu.ArrowRectangle;
						arrowRectangle.Y = LayoutUtils.VAlign(arrowRectangle.Size, this.ownerItem.ClientBounds, ContentAlignment.MiddleCenter).Y;
						return arrowRectangle;
					}
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06005A1D RID: 23069 RVA: 0x001475D8 File Offset: 0x001465D8
		public Rectangle CheckRectangle
		{
			get
			{
				if (this.UseMenuLayout)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						Rectangle checkRectangle = toolStripDropDownMenu.CheckRectangle;
						if (this.ownerItem.CheckedImage != null)
						{
							int height = this.ownerItem.CheckedImage.Height;
							checkRectangle.Y += (checkRectangle.Height - height) / 2;
							checkRectangle.Height = height;
							return checkRectangle;
						}
					}
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06005A1E RID: 23070 RVA: 0x00147650 File Offset: 0x00146650
		public override Rectangle ImageRectangle
		{
			get
			{
				if (this.UseMenuLayout)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						Rectangle imageRectangle = toolStripDropDownMenu.ImageRectangle;
						if (this.ownerItem.ImageScaling == ToolStripItemImageScaling.SizeToFit)
						{
							imageRectangle.Size = toolStripDropDownMenu.ImageScalingSize;
						}
						else
						{
							Image image = this.ownerItem.Image ?? this.ownerItem.CheckedImage;
							imageRectangle.Size = image.Size;
						}
						imageRectangle.Y = LayoutUtils.VAlign(imageRectangle.Size, this.ownerItem.ClientBounds, ContentAlignment.MiddleCenter).Y;
						return imageRectangle;
					}
				}
				return base.ImageRectangle;
			}
		}

		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06005A1F RID: 23071 RVA: 0x001476F8 File Offset: 0x001466F8
		public override Rectangle TextRectangle
		{
			get
			{
				if (this.UseMenuLayout)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						return toolStripDropDownMenu.TextRectangle;
					}
				}
				return base.TextRectangle;
			}
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06005A20 RID: 23072 RVA: 0x0014772E File Offset: 0x0014672E
		public bool UseMenuLayout
		{
			get
			{
				return this.ownerItem.Owner is ToolStripDropDownMenu;
			}
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x00147744 File Offset: 0x00146744
		public override Size GetPreferredSize(Size constrainingSize)
		{
			if (this.UseMenuLayout)
			{
				ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
				if (toolStripDropDownMenu != null)
				{
					return toolStripDropDownMenu.MaxItemSize;
				}
			}
			return base.GetPreferredSize(constrainingSize);
		}

		// Token: 0x040038B0 RID: 14512
		private ToolStripMenuItem ownerItem;
	}
}
