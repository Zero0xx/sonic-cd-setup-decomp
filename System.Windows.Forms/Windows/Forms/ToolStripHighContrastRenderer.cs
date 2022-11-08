using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
	// Token: 0x02000690 RID: 1680
	internal class ToolStripHighContrastRenderer : ToolStripSystemRenderer
	{
		// Token: 0x060058CD RID: 22733 RVA: 0x00142C8D File Offset: 0x00141C8D
		public ToolStripHighContrastRenderer(bool systemRenderMode)
		{
			this.options[ToolStripHighContrastRenderer.optionsDottedBorder | ToolStripHighContrastRenderer.optionsDottedGrip | ToolStripHighContrastRenderer.optionsFillWhenSelected] = !systemRenderMode;
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x060058CE RID: 22734 RVA: 0x00142CC1 File Offset: 0x00141CC1
		public bool DottedBorder
		{
			get
			{
				return this.options[ToolStripHighContrastRenderer.optionsDottedBorder];
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x060058CF RID: 22735 RVA: 0x00142CD3 File Offset: 0x00141CD3
		public bool DottedGrip
		{
			get
			{
				return this.options[ToolStripHighContrastRenderer.optionsDottedGrip];
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x060058D0 RID: 22736 RVA: 0x00142CE5 File Offset: 0x00141CE5
		public bool FillWhenSelected
		{
			get
			{
				return this.options[ToolStripHighContrastRenderer.optionsFillWhenSelected];
			}
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x060058D1 RID: 22737 RVA: 0x00142CF7 File Offset: 0x00141CF7
		internal override ToolStripRenderer RendererOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060058D2 RID: 22738 RVA: 0x00142CFA File Offset: 0x00141CFA
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			base.OnRenderArrow(e);
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x00142D04 File Offset: 0x00141D04
		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			if (this.DottedGrip)
			{
				Graphics graphics = e.Graphics;
				Rectangle gripBounds = e.GripBounds;
				ToolStrip toolStrip = e.ToolStrip;
				int num = (toolStrip.Orientation == Orientation.Horizontal) ? gripBounds.Height : gripBounds.Width;
				int num2 = (toolStrip.Orientation == Orientation.Horizontal) ? gripBounds.Width : gripBounds.Height;
				int num3 = (num - 8) / 4;
				if (num3 > 0)
				{
					Rectangle[] array = new Rectangle[num3];
					int num4 = 4;
					int num5 = num2 / 2;
					for (int i = 0; i < num3; i++)
					{
						array[i] = ((toolStrip.Orientation == Orientation.Horizontal) ? new Rectangle(num5, num4, 2, 2) : new Rectangle(num4, num5, 2, 2));
						num4 += 4;
					}
					graphics.FillRectangles(SystemBrushes.ControlLight, array);
					return;
				}
			}
			else
			{
				base.OnRenderGrip(e);
			}
		}

		// Token: 0x060058D4 RID: 22740 RVA: 0x00142DDC File Offset: 0x00141DDC
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.FillWhenSelected)
			{
				this.RenderItemInternalFilled(e, false);
				return;
			}
			base.OnRenderDropDownButtonBackground(e);
			if (e.Item.Pressed)
			{
				e.Graphics.DrawRectangle(SystemPens.ButtonHighlight, new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1));
			}
		}

		// Token: 0x060058D5 RID: 22741 RVA: 0x00142E3F File Offset: 0x00141E3F
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			base.OnRenderItemCheck(e);
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x00142E48 File Offset: 0x00141E48
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
		}

		// Token: 0x060058D7 RID: 22743 RVA: 0x00142E4A File Offset: 0x00141E4A
		protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
		{
			base.OnRenderItemBackground(e);
		}

		// Token: 0x060058D8 RID: 22744 RVA: 0x00142E54 File Offset: 0x00141E54
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripSplitButton toolStripSplitButton = e.Item as ToolStripSplitButton;
			Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
			Graphics graphics = e.Graphics;
			if (toolStripSplitButton != null)
			{
				Rectangle dropDownButtonBounds = toolStripSplitButton.DropDownButtonBounds;
				if (toolStripSplitButton.Pressed)
				{
					graphics.DrawRectangle(SystemPens.ButtonHighlight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				}
				else if (toolStripSplitButton.Selected)
				{
					graphics.FillRectangle(SystemBrushes.Highlight, rect);
					graphics.DrawRectangle(SystemPens.ButtonHighlight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
					graphics.DrawRectangle(SystemPens.ButtonHighlight, dropDownButtonBounds);
				}
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, dropDownButtonBounds, SystemColors.ControlText, ArrowDirection.Down));
			}
		}

		// Token: 0x060058D9 RID: 22745 RVA: 0x00142F2D File Offset: 0x00141F2D
		protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
		{
			base.OnRenderStatusStripSizingGrip(e);
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x00142F36 File Offset: 0x00141F36
		protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.FillWhenSelected)
			{
				this.RenderItemInternalFilled(e);
				return;
			}
			base.OnRenderLabelBackground(e);
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x00142F50 File Offset: 0x00141F50
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			base.OnRenderMenuItemBackground(e);
			if (!e.Item.IsOnDropDown && e.Item.Pressed)
			{
				e.Graphics.DrawRectangle(SystemPens.ButtonHighlight, 0, 0, e.Item.Width - 1, e.Item.Height - 1);
			}
		}

		// Token: 0x060058DC RID: 22748 RVA: 0x00142FAC File Offset: 0x00141FAC
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.FillWhenSelected)
			{
				this.RenderItemInternalFilled(e, false);
				ToolStripItem item = e.Item;
				Graphics graphics = e.Graphics;
				Color arrowColor = item.Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, item, new Rectangle(Point.Empty, item.Size), arrowColor, ArrowDirection.Down));
				return;
			}
			base.OnRenderOverflowButtonBackground(e);
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x00143014 File Offset: 0x00142014
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (e.TextColor != SystemColors.HighlightText || e.TextColor != SystemColors.ControlText)
			{
				e.DefaultTextColor = SystemColors.ControlText;
			}
			base.OnRenderItemText(e);
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x0014304C File Offset: 0x0014204C
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x00143050 File Offset: 0x00142050
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			Rectangle rectangle = new Rectangle(Point.Empty, e.ToolStrip.Size);
			Graphics graphics = e.Graphics;
			if (e.ToolStrip is ToolStripDropDown)
			{
				graphics.DrawRectangle(SystemPens.ButtonHighlight, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				if (!(e.ToolStrip is ToolStripOverflow))
				{
					graphics.FillRectangle(SystemBrushes.Control, e.ConnectedArea);
					return;
				}
			}
			else
			{
				if (e.ToolStrip is MenuStrip)
				{
					return;
				}
				if (e.ToolStrip is StatusStrip)
				{
					graphics.DrawRectangle(SystemPens.ButtonShadow, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					return;
				}
				this.RenderToolStripBackgroundInternal(e);
			}
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x00143120 File Offset: 0x00142120
		private void RenderToolStripBackgroundInternal(ToolStripRenderEventArgs e)
		{
			Rectangle rect = new Rectangle(Point.Empty, e.ToolStrip.Size);
			Graphics graphics = e.Graphics;
			if (this.DottedBorder)
			{
				using (Pen pen = new Pen(SystemColors.ButtonShadow))
				{
					pen.DashStyle = DashStyle.Dot;
					bool flag = (rect.Width & 1) == 1;
					bool flag2 = (rect.Height & 1) == 1;
					int num = 2;
					graphics.DrawLine(pen, rect.X + num, rect.Y, rect.Width - 1, rect.Y);
					graphics.DrawLine(pen, rect.X + num, rect.Height - 1, rect.Width - 1, rect.Height - 1);
					graphics.DrawLine(pen, rect.X, rect.Y + num, rect.X, rect.Height - 1);
					graphics.DrawLine(pen, rect.Width - 1, rect.Y + num, rect.Width - 1, rect.Height - 1);
					graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(1, 1, 1, 1));
					if (flag)
					{
						graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(rect.Width - 2, 1, 1, 1));
					}
					if (flag2)
					{
						graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(1, rect.Height - 2, 1, 1));
					}
					if (flag2 && flag)
					{
						graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(rect.Width - 2, rect.Height - 2, 1, 1));
					}
					return;
				}
			}
			rect.Width--;
			rect.Height--;
			graphics.DrawRectangle(SystemPens.ButtonShadow, rect);
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x00143300 File Offset: 0x00142300
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			Pen buttonShadow = SystemPens.ButtonShadow;
			Graphics graphics = e.Graphics;
			Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
			if (e.Vertical)
			{
				if (rectangle.Height >= 8)
				{
					rectangle.Inflate(0, -4);
				}
				int num = rectangle.Width / 2;
				graphics.DrawLine(buttonShadow, num, rectangle.Top, num, rectangle.Bottom - 1);
				return;
			}
			if (rectangle.Width >= 4)
			{
				rectangle.Inflate(-2, 0);
			}
			int num2 = rectangle.Height / 2;
			graphics.DrawLine(buttonShadow, rectangle.Left, num2, rectangle.Right - 1, num2);
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x001433AC File Offset: 0x001423AC
		internal static bool IsHighContrastWhiteOnBlack()
		{
			return SystemColors.Control.ToArgb() == Color.Black.ToArgb();
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x001433D8 File Offset: 0x001423D8
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			Image image = e.Image;
			if (image != null)
			{
				if (Image.GetPixelFormatSize(image.PixelFormat) > 16)
				{
					base.OnRenderItemImage(e);
					return;
				}
				Graphics graphics = e.Graphics;
				ToolStripItem item = e.Item;
				Rectangle imageRectangle = e.ImageRectangle;
				using (ImageAttributes imageAttributes = new ImageAttributes())
				{
					if (ToolStripHighContrastRenderer.IsHighContrastWhiteOnBlack() && (!this.FillWhenSelected || (!e.Item.Pressed && !e.Item.Selected)))
					{
						ColorMap colorMap = new ColorMap();
						ColorMap colorMap2 = new ColorMap();
						ColorMap colorMap3 = new ColorMap();
						colorMap.OldColor = Color.Black;
						colorMap.NewColor = Color.White;
						colorMap2.OldColor = Color.White;
						colorMap2.NewColor = Color.Black;
						colorMap3.OldColor = Color.FromArgb(0, 0, 128);
						colorMap3.NewColor = Color.White;
						imageAttributes.SetRemapTable(new ColorMap[]
						{
							colorMap,
							colorMap2,
							colorMap3
						}, ColorAdjustType.Bitmap);
					}
					if (item.ImageScaling == ToolStripItemImageScaling.None)
					{
						graphics.DrawImage(image, imageRectangle, 0, 0, imageRectangle.Width, imageRectangle.Height, GraphicsUnit.Pixel, imageAttributes);
					}
					else
					{
						graphics.DrawImage(image, imageRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
					}
				}
			}
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x0014353C File Offset: 0x0014253C
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (!this.FillWhenSelected)
			{
				base.OnRenderButtonBackground(e);
				return;
			}
			ToolStripButton toolStripButton = e.Item as ToolStripButton;
			if (toolStripButton != null && toolStripButton.Checked)
			{
				Graphics graphics = e.Graphics;
				Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
				if (toolStripButton.CheckState == CheckState.Checked)
				{
					graphics.FillRectangle(SystemBrushes.Highlight, rect);
				}
				graphics.DrawRectangle(SystemPens.ControlLight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				return;
			}
			this.RenderItemInternalFilled(e);
		}

		// Token: 0x060058E5 RID: 22757 RVA: 0x001435D6 File Offset: 0x001425D6
		private void RenderItemInternalFilled(ToolStripItemRenderEventArgs e)
		{
			this.RenderItemInternalFilled(e, true);
		}

		// Token: 0x060058E6 RID: 22758 RVA: 0x001435E0 File Offset: 0x001425E0
		private void RenderItemInternalFilled(ToolStripItemRenderEventArgs e, bool pressFill)
		{
			Graphics graphics = e.Graphics;
			Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
			if (!e.Item.Pressed)
			{
				if (e.Item.Selected)
				{
					graphics.FillRectangle(SystemBrushes.Highlight, rect);
					graphics.DrawRectangle(SystemPens.ControlLight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				}
				return;
			}
			if (pressFill)
			{
				graphics.FillRectangle(SystemBrushes.Highlight, rect);
				return;
			}
			graphics.DrawRectangle(SystemPens.ControlLight, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
		}

		// Token: 0x04003820 RID: 14368
		private const int GRIP_PADDING = 4;

		// Token: 0x04003821 RID: 14369
		private BitVector32 options = default(BitVector32);

		// Token: 0x04003822 RID: 14370
		private static readonly int optionsDottedBorder = BitVector32.CreateMask();

		// Token: 0x04003823 RID: 14371
		private static readonly int optionsDottedGrip = BitVector32.CreateMask(ToolStripHighContrastRenderer.optionsDottedBorder);

		// Token: 0x04003824 RID: 14372
		private static readonly int optionsFillWhenSelected = BitVector32.CreateMask(ToolStripHighContrastRenderer.optionsDottedGrip);
	}
}
