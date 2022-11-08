using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006D2 RID: 1746
	public class ToolStripProfessionalRenderer : ToolStripRenderer
	{
		// Token: 0x06005BF3 RID: 23539 RVA: 0x0014D6DB File Offset: 0x0014C6DB
		public ToolStripProfessionalRenderer()
		{
		}

		// Token: 0x06005BF4 RID: 23540 RVA: 0x0014D6F9 File Offset: 0x0014C6F9
		internal ToolStripProfessionalRenderer(bool isDefault) : base(isDefault)
		{
		}

		// Token: 0x06005BF5 RID: 23541 RVA: 0x0014D718 File Offset: 0x0014C718
		public ToolStripProfessionalRenderer(ProfessionalColorTable professionalColorTable)
		{
			this.professionalColorTable = professionalColorTable;
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x06005BF6 RID: 23542 RVA: 0x0014D73D File Offset: 0x0014C73D
		public ProfessionalColorTable ColorTable
		{
			get
			{
				if (this.professionalColorTable == null)
				{
					return ProfessionalColors.ColorTable;
				}
				return this.professionalColorTable;
			}
		}

		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x06005BF7 RID: 23543 RVA: 0x0014D753 File Offset: 0x0014C753
		internal override ToolStripRenderer RendererOverride
		{
			get
			{
				if (DisplayInformation.HighContrast)
				{
					return this.HighContrastRenderer;
				}
				if (DisplayInformation.LowResolution)
				{
					return this.LowResolutionRenderer;
				}
				return null;
			}
		}

		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x0014D772 File Offset: 0x0014C772
		internal ToolStripRenderer HighContrastRenderer
		{
			get
			{
				if (this.toolStripHighContrastRenderer == null)
				{
					this.toolStripHighContrastRenderer = new ToolStripHighContrastRenderer(false);
				}
				return this.toolStripHighContrastRenderer;
			}
		}

		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x0014D78E File Offset: 0x0014C78E
		internal ToolStripRenderer LowResolutionRenderer
		{
			get
			{
				if (this.toolStripLowResolutionRenderer == null)
				{
					this.toolStripLowResolutionRenderer = new ToolStripProfessionalLowResolutionRenderer();
				}
				return this.toolStripLowResolutionRenderer;
			}
		}

		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x06005BFA RID: 23546 RVA: 0x0014D7A9 File Offset: 0x0014C7A9
		// (set) Token: 0x06005BFB RID: 23547 RVA: 0x0014D7B1 File Offset: 0x0014C7B1
		public bool RoundedEdges
		{
			get
			{
				return this.roundedEdges;
			}
			set
			{
				this.roundedEdges = value;
			}
		}

		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x06005BFC RID: 23548 RVA: 0x0014D7BA File Offset: 0x0014C7BA
		private bool UseSystemColors
		{
			get
			{
				return this.ColorTable.UseSystemColors || !ToolStripManager.VisualStylesEnabled;
			}
		}

		// Token: 0x06005BFD RID: 23549 RVA: 0x0014D7D4 File Offset: 0x0014C7D4
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripBackground(e);
				return;
			}
			ToolStrip toolStrip = e.ToolStrip;
			if (!base.ShouldPaintBackground(toolStrip))
			{
				return;
			}
			if (toolStrip is ToolStripDropDown)
			{
				this.RenderToolStripDropDownBackground(e);
				return;
			}
			if (toolStrip is MenuStrip)
			{
				this.RenderMenuStripBackground(e);
				return;
			}
			if (toolStrip is StatusStrip)
			{
				this.RenderStatusStripBackground(e);
				return;
			}
			this.RenderToolStripBackgroundInternal(e);
		}

		// Token: 0x06005BFE RID: 23550 RVA: 0x0014D83C File Offset: 0x0014C83C
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderOverflowButtonBackground(e);
				return;
			}
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			bool flag = item.RightToLeft == RightToLeft.Yes;
			this.RenderOverflowBackground(e, flag);
			bool flag2 = e.ToolStrip.Orientation == Orientation.Horizontal;
			Rectangle empty = Rectangle.Empty;
			if (flag)
			{
				empty = new Rectangle(0, item.Height - 8, 9, 5);
			}
			else
			{
				empty = new Rectangle(item.Width - 12, item.Height - 8, 9, 5);
			}
			ArrowDirection direction = flag2 ? ArrowDirection.Down : ArrowDirection.Right;
			int num = (flag && flag2) ? -1 : 1;
			empty.Offset(num, 1);
			this.RenderArrowInternal(graphics, empty, direction, SystemBrushes.ButtonHighlight);
			empty.Offset(-1 * num, -1);
			this.RenderArrowInternal(graphics, empty, direction, SystemBrushes.ControlText);
			if (flag2)
			{
				num = (flag ? -2 : 0);
				graphics.DrawLine(SystemPens.ControlText, empty.Right - 6, empty.Y - 2, empty.Right - 2, empty.Y - 2);
				graphics.DrawLine(SystemPens.ButtonHighlight, empty.Right - 5 + num, empty.Y - 1, empty.Right - 1 + num, empty.Y - 1);
				return;
			}
			graphics.DrawLine(SystemPens.ControlText, empty.X, empty.Y, empty.X, empty.Bottom - 1);
			graphics.DrawLine(SystemPens.ButtonHighlight, empty.X + 1, empty.Y + 1, empty.X + 1, empty.Bottom);
		}

		// Token: 0x06005BFF RID: 23551 RVA: 0x0014D9D8 File Offset: 0x0014C9D8
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderDropDownButtonBackground(e);
				return;
			}
			ToolStripDropDownItem toolStripDropDownItem = e.Item as ToolStripDropDownItem;
			if (toolStripDropDownItem != null && toolStripDropDownItem.Pressed && toolStripDropDownItem.HasDropDownItems)
			{
				Rectangle bounds = new Rectangle(Point.Empty, toolStripDropDownItem.Size);
				this.RenderPressedGradient(e.Graphics, bounds);
				return;
			}
			this.RenderItemInternal(e, true);
		}

		// Token: 0x06005C00 RID: 23552 RVA: 0x0014DA3C File Offset: 0x0014CA3C
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderSeparator(e);
				return;
			}
			this.RenderSeparatorInternal(e.Graphics, e.Item, new Rectangle(Point.Empty, e.Item.Size), e.Vertical);
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x0014DA7C File Offset: 0x0014CA7C
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderSplitButtonBackground(e);
				return;
			}
			ToolStripSplitButton toolStripSplitButton = e.Item as ToolStripSplitButton;
			Graphics graphics = e.Graphics;
			if (toolStripSplitButton != null)
			{
				Rectangle rectangle = new Rectangle(Point.Empty, toolStripSplitButton.Size);
				if (toolStripSplitButton.BackgroundImage != null)
				{
					Rectangle clipRect = toolStripSplitButton.Selected ? toolStripSplitButton.ContentRectangle : rectangle;
					ControlPaint.DrawBackgroundImage(graphics, toolStripSplitButton.BackgroundImage, toolStripSplitButton.BackColor, toolStripSplitButton.BackgroundImageLayout, rectangle, clipRect);
				}
				bool flag = toolStripSplitButton.Pressed || toolStripSplitButton.ButtonPressed || toolStripSplitButton.Selected || toolStripSplitButton.ButtonSelected;
				if (flag)
				{
					this.RenderItemInternal(e, true);
				}
				if (toolStripSplitButton.ButtonPressed)
				{
					Rectangle rectangle2 = toolStripSplitButton.ButtonBounds;
					Padding padding = (toolStripSplitButton.RightToLeft == RightToLeft.Yes) ? new Padding(0, 1, 1, 1) : new Padding(1, 1, 0, 1);
					rectangle2 = LayoutUtils.DeflateRect(rectangle2, padding);
					this.RenderPressedButtonFill(graphics, rectangle2);
				}
				else if (toolStripSplitButton.Pressed)
				{
					this.RenderPressedGradient(e.Graphics, rectangle);
				}
				Rectangle dropDownButtonBounds = toolStripSplitButton.DropDownButtonBounds;
				if (flag && !toolStripSplitButton.Pressed)
				{
					using (Brush brush = new SolidBrush(this.ColorTable.ButtonSelectedBorder))
					{
						graphics.FillRectangle(brush, toolStripSplitButton.SplitterBounds);
					}
				}
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, dropDownButtonBounds, SystemColors.ControlText, ArrowDirection.Down));
			}
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x0014DBEC File Offset: 0x0014CBEC
		protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripStatusLabelBackground(e);
				return;
			}
			ToolStripProfessionalRenderer.RenderLabelInternal(e);
			ToolStripStatusLabel toolStripStatusLabel = e.Item as ToolStripStatusLabel;
			ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0, 0, toolStripStatusLabel.Width, toolStripStatusLabel.Height), toolStripStatusLabel.BorderStyle, (Border3DSide)toolStripStatusLabel.BorderSides);
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x0014DC45 File Offset: 0x0014CC45
		protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderLabelBackground(e);
				return;
			}
			ToolStripProfessionalRenderer.RenderLabelInternal(e);
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x0014DC60 File Offset: 0x0014CC60
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderButtonBackground(e);
				return;
			}
			ToolStripButton toolStripButton = e.Item as ToolStripButton;
			Graphics graphics = e.Graphics;
			Rectangle rectangle = new Rectangle(Point.Empty, toolStripButton.Size);
			if (toolStripButton.CheckState == CheckState.Unchecked)
			{
				this.RenderItemInternal(e, true);
				return;
			}
			Rectangle clipRect = toolStripButton.Selected ? toolStripButton.ContentRectangle : rectangle;
			if (toolStripButton.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, toolStripButton.BackgroundImage, toolStripButton.BackColor, toolStripButton.BackgroundImageLayout, rectangle, clipRect);
			}
			if (this.UseSystemColors)
			{
				if (toolStripButton.Selected)
				{
					this.RenderPressedButtonFill(graphics, rectangle);
				}
				else
				{
					this.RenderCheckedButtonFill(graphics, rectangle);
				}
				using (Pen pen = new Pen(this.ColorTable.ButtonSelectedBorder))
				{
					graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					return;
				}
			}
			if (toolStripButton.Selected)
			{
				this.RenderPressedButtonFill(graphics, rectangle);
			}
			else
			{
				this.RenderCheckedButtonFill(graphics, rectangle);
			}
			using (Pen pen2 = new Pen(this.ColorTable.ButtonSelectedBorder))
			{
				graphics.DrawRectangle(pen2, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
			}
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x0014DDD0 File Offset: 0x0014CDD0
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripBorder(e);
				return;
			}
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			if (toolStrip is ToolStripDropDown)
			{
				this.RenderToolStripDropDownBorder(e);
				return;
			}
			if (toolStrip is MenuStrip)
			{
				return;
			}
			if (toolStrip is StatusStrip)
			{
				this.RenderStatusStripBorder(e);
				return;
			}
			Rectangle rectangle = new Rectangle(Point.Empty, toolStrip.Size);
			using (Pen pen = new Pen(this.ColorTable.ToolStripBorder))
			{
				if (toolStrip.Orientation == Orientation.Horizontal)
				{
					graphics.DrawLine(pen, rectangle.Left, rectangle.Height - 1, rectangle.Right, rectangle.Height - 1);
					if (this.RoundedEdges)
					{
						graphics.DrawLine(pen, rectangle.Width - 2, rectangle.Height - 2, rectangle.Width - 1, rectangle.Height - 3);
					}
				}
				else
				{
					graphics.DrawLine(pen, rectangle.Width - 1, 0, rectangle.Width - 1, rectangle.Height - 1);
					if (this.RoundedEdges)
					{
						graphics.DrawLine(pen, rectangle.Width - 2, rectangle.Height - 2, rectangle.Width - 1, rectangle.Height - 3);
					}
				}
			}
			if (this.RoundedEdges)
			{
				if (toolStrip.OverflowButton.Visible)
				{
					this.RenderOverflowButtonEffectsOverBorder(e);
					return;
				}
				Rectangle empty = Rectangle.Empty;
				if (toolStrip.Orientation == Orientation.Horizontal)
				{
					empty = new Rectangle(rectangle.Width - 1, 3, 1, rectangle.Height - 3);
				}
				else
				{
					empty = new Rectangle(3, rectangle.Height - 1, rectangle.Width - 3, rectangle.Height - 1);
				}
				this.FillWithDoubleGradient(this.ColorTable.OverflowButtonGradientBegin, this.ColorTable.OverflowButtonGradientMiddle, this.ColorTable.OverflowButtonGradientEnd, e.Graphics, empty, 12, 12, LinearGradientMode.Vertical, false);
				this.RenderToolStripCurve(e);
			}
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x0014DFCC File Offset: 0x0014CFCC
		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderGrip(e);
				return;
			}
			Graphics graphics = e.Graphics;
			Rectangle gripBounds = e.GripBounds;
			ToolStrip toolStrip = e.ToolStrip;
			bool flag = e.ToolStrip.RightToLeft == RightToLeft.Yes;
			int num = (toolStrip.Orientation == Orientation.Horizontal) ? gripBounds.Height : gripBounds.Width;
			int num2 = (toolStrip.Orientation == Orientation.Horizontal) ? gripBounds.Width : gripBounds.Height;
			int num3 = (num - 8) / 4;
			if (num3 > 0)
			{
				int num4 = (toolStrip is MenuStrip) ? 2 : 0;
				Rectangle[] array = new Rectangle[num3];
				int num5 = 5 + num4;
				int num6 = num2 / 2;
				for (int i = 0; i < num3; i++)
				{
					array[i] = ((toolStrip.Orientation == Orientation.Horizontal) ? new Rectangle(num6, num5, 2, 2) : new Rectangle(num5, num6, 2, 2));
					num5 += 4;
				}
				int num7 = flag ? 1 : -1;
				if (flag)
				{
					for (int j = 0; j < num3; j++)
					{
						array[j].Offset(-num7, 0);
					}
				}
				using (Brush brush = new SolidBrush(this.ColorTable.GripLight))
				{
					graphics.FillRectangles(brush, array);
				}
				for (int k = 0; k < num3; k++)
				{
					array[k].Offset(num7, -1);
				}
				using (Brush brush2 = new SolidBrush(this.ColorTable.GripDark))
				{
					graphics.FillRectangles(brush2, array);
				}
			}
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x0014E17C File Offset: 0x0014D17C
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderMenuItemBackground(e);
				return;
			}
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
			if (rectangle.Width == 0 || rectangle.Height == 0)
			{
				return;
			}
			if (item is MdiControlStrip.SystemMenuItem)
			{
				return;
			}
			if (item.IsOnDropDown)
			{
				rectangle = LayoutUtils.DeflateRect(rectangle, this.dropDownMenuItemPaintPadding);
				if (item.Selected)
				{
					Color color = this.ColorTable.MenuItemBorder;
					if (item.Enabled)
					{
						if (this.UseSystemColors)
						{
							color = SystemColors.Highlight;
							this.RenderSelectedButtonFill(graphics, rectangle);
						}
						else
						{
							using (Brush brush = new SolidBrush(this.ColorTable.MenuItemSelected))
							{
								graphics.FillRectangle(brush, rectangle);
							}
						}
					}
					using (Pen pen = new Pen(color))
					{
						graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
						return;
					}
				}
				Rectangle rectangle2 = rectangle;
				if (item.BackgroundImage != null)
				{
					ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, rectangle, rectangle2);
					return;
				}
				if (item.Owner == null || !(item.BackColor != item.Owner.BackColor))
				{
					return;
				}
				using (Brush brush2 = new SolidBrush(item.BackColor))
				{
					graphics.FillRectangle(brush2, rectangle2);
					return;
				}
			}
			if (item.Pressed)
			{
				this.RenderPressedGradient(graphics, rectangle);
				return;
			}
			if (item.Selected)
			{
				Color color2 = this.ColorTable.MenuItemBorder;
				if (item.Enabled)
				{
					if (this.UseSystemColors)
					{
						color2 = SystemColors.Highlight;
						this.RenderSelectedButtonFill(graphics, rectangle);
					}
					else
					{
						using (Brush brush3 = new LinearGradientBrush(rectangle, this.ColorTable.MenuItemSelectedGradientBegin, this.ColorTable.MenuItemSelectedGradientEnd, LinearGradientMode.Vertical))
						{
							graphics.FillRectangle(brush3, rectangle);
						}
					}
				}
				using (Pen pen2 = new Pen(color2))
				{
					graphics.DrawRectangle(pen2, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					return;
				}
			}
			Rectangle rectangle3 = rectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, rectangle, rectangle3);
				return;
			}
			if (item.Owner != null && item.BackColor != item.Owner.BackColor)
			{
				using (Brush brush4 = new SolidBrush(item.BackColor))
				{
					graphics.FillRectangle(brush4, rectangle3);
				}
			}
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x0014E47C File Offset: 0x0014D47C
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderArrow(e);
				return;
			}
			ToolStripItem item = e.Item;
			if (item is ToolStripDropDownItem)
			{
				e.DefaultArrowColor = (item.Enabled ? SystemColors.ControlText : SystemColors.ControlDark);
			}
			base.OnRenderArrow(e);
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x0014E4CC File Offset: 0x0014D4CC
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderImageMargin(e);
				return;
			}
			Graphics graphics = e.Graphics;
			Rectangle affectedBounds = e.AffectedBounds;
			affectedBounds.Y += 2;
			affectedBounds.Height -= 4;
			RightToLeft rightToLeft = e.ToolStrip.RightToLeft;
			Color beginColor = (rightToLeft == RightToLeft.No) ? this.ColorTable.ImageMarginGradientBegin : this.ColorTable.ImageMarginGradientEnd;
			Color endColor = (rightToLeft == RightToLeft.No) ? this.ColorTable.ImageMarginGradientEnd : this.ColorTable.ImageMarginGradientBegin;
			this.FillWithDoubleGradient(beginColor, this.ColorTable.ImageMarginGradientMiddle, endColor, e.Graphics, affectedBounds, 12, 12, LinearGradientMode.Horizontal, e.ToolStrip.RightToLeft == RightToLeft.Yes);
		}

		// Token: 0x06005C0A RID: 23562 RVA: 0x0014E588 File Offset: 0x0014D588
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderItemText(e);
				return;
			}
			if (e.Item is ToolStripMenuItem && (e.Item.Selected || e.Item.Pressed))
			{
				e.DefaultTextColor = e.Item.ForeColor;
			}
			base.OnRenderItemText(e);
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x0014E5E4 File Offset: 0x0014D5E4
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderItemCheck(e);
				return;
			}
			this.RenderCheckBackground(e);
			base.OnRenderItemCheck(e);
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x0014E604 File Offset: 0x0014D604
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderItemImage(e);
				return;
			}
			Rectangle imageRectangle = e.ImageRectangle;
			Image image = e.Image;
			if (e.Item is ToolStripMenuItem)
			{
				ToolStripMenuItem toolStripMenuItem = e.Item as ToolStripMenuItem;
				if (toolStripMenuItem.CheckState != CheckState.Unchecked)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = toolStripMenuItem.ParentInternal as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null && !toolStripDropDownMenu.ShowCheckMargin && toolStripDropDownMenu.ShowImageMargin)
					{
						this.RenderCheckBackground(e);
					}
				}
			}
			if (imageRectangle != Rectangle.Empty && image != null)
			{
				if (!e.Item.Enabled)
				{
					base.OnRenderItemImage(e);
					return;
				}
				if (e.Item.ImageScaling == ToolStripItemImageScaling.None)
				{
					e.Graphics.DrawImage(image, imageRectangle, new Rectangle(Point.Empty, imageRectangle.Size), GraphicsUnit.Pixel);
					return;
				}
				e.Graphics.DrawImage(image, imageRectangle);
			}
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x0014E6D8 File Offset: 0x0014D6D8
		protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripPanelBackground(e);
				return;
			}
			ToolStripPanel toolStripPanel = e.ToolStripPanel;
			if (!base.ShouldPaintBackground(toolStripPanel))
			{
				return;
			}
			e.Handled = true;
			this.RenderBackgroundGradient(e.Graphics, toolStripPanel, this.ColorTable.ToolStripPanelGradientBegin, this.ColorTable.ToolStripPanelGradientEnd);
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x0014E730 File Offset: 0x0014D730
		protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
		{
			if (this.RendererOverride != null)
			{
				base.OnRenderToolStripContentPanelBackground(e);
				return;
			}
			ToolStripContentPanel toolStripContentPanel = e.ToolStripContentPanel;
			if (!base.ShouldPaintBackground(toolStripContentPanel))
			{
				return;
			}
			if (SystemInformation.InLockedTerminalSession())
			{
				return;
			}
			e.Handled = true;
			e.Graphics.Clear(this.ColorTable.ToolStripContentPanelGradientEnd);
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x0014E784 File Offset: 0x0014D784
		internal override Region GetTransparentRegion(ToolStrip toolStrip)
		{
			if (toolStrip is ToolStripDropDown || toolStrip is MenuStrip || toolStrip is StatusStrip)
			{
				return null;
			}
			if (!this.RoundedEdges)
			{
				return null;
			}
			Rectangle rectangle = new Rectangle(Point.Empty, toolStrip.Size);
			if (toolStrip.ParentInternal != null)
			{
				Point empty = Point.Empty;
				Point point = new Point(rectangle.Width - 1, 0);
				Point location = new Point(0, rectangle.Height - 1);
				Point point2 = new Point(rectangle.Width - 1, rectangle.Height - 1);
				Rectangle rect = new Rectangle(empty, ToolStripProfessionalRenderer.onePix);
				Rectangle rect2 = new Rectangle(location, new Size(2, 1));
				Rectangle rect3 = new Rectangle(location.X, location.Y - 1, 1, 2);
				Rectangle rect4 = new Rectangle(point2.X - 1, point2.Y, 2, 1);
				Rectangle rect5 = new Rectangle(point2.X, point2.Y - 1, 1, 2);
				Rectangle rect6;
				Rectangle rect7;
				if (toolStrip.OverflowButton.Visible)
				{
					rect6 = new Rectangle(point.X - 1, point.Y, 1, 1);
					rect7 = new Rectangle(point.X, point.Y, 1, 2);
				}
				else
				{
					rect6 = new Rectangle(point.X - 2, point.Y, 2, 1);
					rect7 = new Rectangle(point.X, point.Y, 1, 3);
				}
				Region region = new Region(rect);
				region.Union(rect);
				region.Union(rect2);
				region.Union(rect3);
				region.Union(rect4);
				region.Union(rect5);
				region.Union(rect6);
				region.Union(rect7);
				return region;
			}
			return null;
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x0014E938 File Offset: 0x0014D938
		private void RenderOverflowButtonEffectsOverBorder(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			ToolStripItem overflowButton = toolStrip.OverflowButton;
			if (!overflowButton.Visible)
			{
				return;
			}
			Graphics graphics = e.Graphics;
			Color color;
			Color color2;
			if (overflowButton.Pressed)
			{
				color = this.ColorTable.ButtonPressedGradientBegin;
				color2 = color;
			}
			else if (overflowButton.Selected)
			{
				color = this.ColorTable.ButtonSelectedGradientMiddle;
				color2 = color;
			}
			else
			{
				color = this.ColorTable.ToolStripBorder;
				color2 = this.ColorTable.ToolStripGradientMiddle;
			}
			using (Brush brush = new SolidBrush(color))
			{
				graphics.FillRectangle(brush, toolStrip.Width - 1, toolStrip.Height - 2, 1, 1);
				graphics.FillRectangle(brush, toolStrip.Width - 2, toolStrip.Height - 1, 1, 1);
			}
			using (Brush brush2 = new SolidBrush(color2))
			{
				graphics.FillRectangle(brush2, toolStrip.Width - 2, 0, 1, 1);
				graphics.FillRectangle(brush2, toolStrip.Width - 1, 1, 1, 1);
			}
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x0014EA54 File Offset: 0x0014DA54
		private void FillWithDoubleGradient(Color beginColor, Color middleColor, Color endColor, Graphics g, Rectangle bounds, int firstGradientWidth, int secondGradientWidth, LinearGradientMode mode, bool flipHorizontal)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			Rectangle rect = bounds;
			Rectangle rect2 = bounds;
			bool flag;
			if (mode == LinearGradientMode.Horizontal)
			{
				if (flipHorizontal)
				{
					Color color = endColor;
					endColor = beginColor;
					beginColor = color;
				}
				rect2.Width = firstGradientWidth;
				rect.Width = secondGradientWidth + 1;
				rect.X = bounds.Right - rect.Width;
				flag = (bounds.Width > firstGradientWidth + secondGradientWidth);
			}
			else
			{
				rect2.Height = firstGradientWidth;
				rect.Height = secondGradientWidth + 1;
				rect.Y = bounds.Bottom - rect.Height;
				flag = (bounds.Height > firstGradientWidth + secondGradientWidth);
			}
			if (flag)
			{
				using (Brush brush = new SolidBrush(middleColor))
				{
					g.FillRectangle(brush, bounds);
				}
				using (Brush brush2 = new LinearGradientBrush(rect2, beginColor, middleColor, mode))
				{
					g.FillRectangle(brush2, rect2);
				}
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, middleColor, endColor, mode))
				{
					if (mode == LinearGradientMode.Horizontal)
					{
						rect.X++;
						rect.Width--;
					}
					else
					{
						rect.Y++;
						rect.Height--;
					}
					g.FillRectangle(linearGradientBrush, rect);
					return;
				}
			}
			using (Brush brush3 = new LinearGradientBrush(bounds, beginColor, endColor, mode))
			{
				g.FillRectangle(brush3, bounds);
			}
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x0014EC0C File Offset: 0x0014DC0C
		private void RenderStatusStripBorder(ToolStripRenderEventArgs e)
		{
			e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, 0, e.ToolStrip.Width, 0);
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x0014EC2C File Offset: 0x0014DC2C
		private void RenderStatusStripBackground(ToolStripRenderEventArgs e)
		{
			StatusStrip statusStrip = e.ToolStrip as StatusStrip;
			this.RenderBackgroundGradient(e.Graphics, statusStrip, this.ColorTable.StatusStripGradientBegin, this.ColorTable.StatusStripGradientEnd, statusStrip.Orientation);
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x0014EC70 File Offset: 0x0014DC70
		private void RenderCheckBackground(ToolStripItemImageRenderEventArgs e)
		{
			Rectangle rectangle = new Rectangle(e.ImageRectangle.Left - 2, 1, e.ImageRectangle.Width + 4, e.Item.Height - 2);
			Graphics graphics = e.Graphics;
			if (!this.UseSystemColors)
			{
				Color color = e.Item.Selected ? this.ColorTable.CheckSelectedBackground : this.ColorTable.CheckBackground;
				color = (e.Item.Pressed ? this.ColorTable.CheckPressedBackground : color);
				using (Brush brush = new SolidBrush(color))
				{
					graphics.FillRectangle(brush, rectangle);
				}
				using (Pen pen = new Pen(this.ColorTable.ButtonSelectedBorder))
				{
					graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					return;
				}
			}
			if (e.Item.Pressed)
			{
				this.RenderPressedButtonFill(graphics, rectangle);
			}
			else
			{
				this.RenderSelectedButtonFill(graphics, rectangle);
			}
			graphics.DrawRectangle(SystemPens.Highlight, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
		}

		// Token: 0x06005C15 RID: 23573 RVA: 0x0014EDD4 File Offset: 0x0014DDD4
		private void RenderPressedGradient(Graphics g, Rectangle bounds)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.MenuItemPressedGradientBegin, this.ColorTable.MenuItemPressedGradientEnd, LinearGradientMode.Vertical))
			{
				g.FillRectangle(brush, bounds);
			}
			using (Pen pen = new Pen(this.ColorTable.MenuBorder))
			{
				g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
			}
		}

		// Token: 0x06005C16 RID: 23574 RVA: 0x0014EE88 File Offset: 0x0014DE88
		private void RenderMenuStripBackground(ToolStripRenderEventArgs e)
		{
			this.RenderBackgroundGradient(e.Graphics, e.ToolStrip, this.ColorTable.MenuStripGradientBegin, this.ColorTable.MenuStripGradientEnd, e.ToolStrip.Orientation);
		}

		// Token: 0x06005C17 RID: 23575 RVA: 0x0014EEC0 File Offset: 0x0014DEC0
		private static void RenderLabelInternal(ToolStripItemRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			ToolStripItem item = e.Item;
			Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
			Rectangle clipRect = item.Selected ? item.ContentRectangle : rectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, rectangle, clipRect);
			}
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x0014EF21 File Offset: 0x0014DF21
		private void RenderBackgroundGradient(Graphics g, Control control, Color beginColor, Color endColor)
		{
			this.RenderBackgroundGradient(g, control, beginColor, endColor, Orientation.Horizontal);
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x0014EF30 File Offset: 0x0014DF30
		private void RenderBackgroundGradient(Graphics g, Control control, Color beginColor, Color endColor, Orientation orientation)
		{
			if (control.RightToLeft == RightToLeft.Yes)
			{
				Color color = beginColor;
				beginColor = endColor;
				endColor = color;
			}
			if (orientation == Orientation.Horizontal)
			{
				Control parentInternal = control.ParentInternal;
				if (parentInternal != null)
				{
					Rectangle rectangle = new Rectangle(Point.Empty, parentInternal.Size);
					if (LayoutUtils.IsZeroWidthOrHeight(rectangle))
					{
						return;
					}
					using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, beginColor, endColor, LinearGradientMode.Horizontal))
					{
						linearGradientBrush.TranslateTransform((float)(parentInternal.Width - control.Location.X), (float)(parentInternal.Height - control.Location.Y));
						g.FillRectangle(linearGradientBrush, new Rectangle(Point.Empty, control.Size));
						return;
					}
				}
				Rectangle rectangle2 = new Rectangle(Point.Empty, control.Size);
				if (LayoutUtils.IsZeroWidthOrHeight(rectangle2))
				{
					return;
				}
				using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rectangle2, beginColor, endColor, LinearGradientMode.Horizontal))
				{
					g.FillRectangle(linearGradientBrush2, rectangle2);
					return;
				}
			}
			using (Brush brush = new SolidBrush(beginColor))
			{
				g.FillRectangle(brush, new Rectangle(Point.Empty, control.Size));
			}
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x0014F078 File Offset: 0x0014E078
		private void RenderToolStripBackgroundInternal(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle bounds = new Rectangle(Point.Empty, e.ToolStrip.Size);
			LinearGradientMode mode = (toolStrip.Orientation == Orientation.Horizontal) ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
			this.FillWithDoubleGradient(this.ColorTable.ToolStripGradientBegin, this.ColorTable.ToolStripGradientMiddle, this.ColorTable.ToolStripGradientEnd, e.Graphics, bounds, 12, 12, mode, false);
		}

		// Token: 0x06005C1B RID: 23579 RVA: 0x0014F0EC File Offset: 0x0014E0EC
		private void RenderToolStripDropDownBackground(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Rectangle rect = new Rectangle(Point.Empty, e.ToolStrip.Size);
			using (Brush brush = new SolidBrush(this.ColorTable.ToolStripDropDownBackground))
			{
				e.Graphics.FillRectangle(brush, rect);
			}
		}

		// Token: 0x06005C1C RID: 23580 RVA: 0x0014F154 File Offset: 0x0014E154
		private void RenderToolStripDropDownBorder(ToolStripRenderEventArgs e)
		{
			ToolStripDropDown toolStripDropDown = e.ToolStrip as ToolStripDropDown;
			Graphics graphics = e.Graphics;
			if (toolStripDropDown != null)
			{
				Rectangle rectangle = new Rectangle(Point.Empty, toolStripDropDown.Size);
				using (Pen pen = new Pen(this.ColorTable.MenuBorder))
				{
					graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				}
				if (!(toolStripDropDown is ToolStripOverflow))
				{
					using (Brush brush = new SolidBrush(this.ColorTable.ToolStripDropDownBackground))
					{
						graphics.FillRectangle(brush, e.ConnectedArea);
					}
				}
			}
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x0014F224 File Offset: 0x0014E224
		private void RenderOverflowBackground(ToolStripItemRenderEventArgs e, bool rightToLeft)
		{
			Graphics graphics = e.Graphics;
			ToolStripOverflowButton toolStripOverflowButton = e.Item as ToolStripOverflowButton;
			Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
			Rectangle withinBounds = rectangle;
			bool flag = this.RoundedEdges && !(toolStripOverflowButton.GetCurrentParent() is MenuStrip);
			bool flag2 = e.ToolStrip.Orientation == Orientation.Horizontal;
			if (flag2)
			{
				rectangle.X += rectangle.Width - 12 + 1;
				rectangle.Width = 12;
				if (rightToLeft)
				{
					rectangle = LayoutUtils.RTLTranslate(rectangle, withinBounds);
				}
			}
			else
			{
				rectangle.Y = rectangle.Height - 12 + 1;
				rectangle.Height = 12;
			}
			Color color;
			Color middleColor;
			Color endColor;
			Color color2;
			Color color3;
			if (toolStripOverflowButton.Pressed)
			{
				color = this.ColorTable.ButtonPressedGradientBegin;
				middleColor = this.ColorTable.ButtonPressedGradientMiddle;
				endColor = this.ColorTable.ButtonPressedGradientEnd;
				color2 = this.ColorTable.ButtonPressedGradientBegin;
				color3 = color2;
			}
			else if (toolStripOverflowButton.Selected)
			{
				color = this.ColorTable.ButtonSelectedGradientBegin;
				middleColor = this.ColorTable.ButtonSelectedGradientMiddle;
				endColor = this.ColorTable.ButtonSelectedGradientEnd;
				color2 = this.ColorTable.ButtonSelectedGradientMiddle;
				color3 = color2;
			}
			else
			{
				color = this.ColorTable.OverflowButtonGradientBegin;
				middleColor = this.ColorTable.OverflowButtonGradientMiddle;
				endColor = this.ColorTable.OverflowButtonGradientEnd;
				color2 = this.ColorTable.ToolStripBorder;
				color3 = (flag2 ? this.ColorTable.ToolStripGradientMiddle : this.ColorTable.ToolStripGradientEnd);
			}
			if (flag)
			{
				using (Pen pen = new Pen(color2))
				{
					Point pt = new Point(rectangle.Left - 1, rectangle.Height - 2);
					Point pt2 = new Point(rectangle.Left, rectangle.Height - 2);
					if (rightToLeft)
					{
						pt.X = rectangle.Right + 1;
						pt2.X = rectangle.Right;
					}
					graphics.DrawLine(pen, pt, pt2);
				}
			}
			LinearGradientMode mode = flag2 ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
			this.FillWithDoubleGradient(color, middleColor, endColor, graphics, rectangle, 12, 12, mode, false);
			if (flag)
			{
				using (Brush brush = new SolidBrush(color3))
				{
					if (flag2)
					{
						Point point = new Point(rectangle.X - 2, 0);
						Point point2 = new Point(rectangle.X - 1, 1);
						if (rightToLeft)
						{
							point.X = rectangle.Right + 1;
							point2.X = rectangle.Right;
						}
						graphics.FillRectangle(brush, point.X, point.Y, 1, 1);
						graphics.FillRectangle(brush, point2.X, point2.Y, 1, 1);
					}
					else
					{
						graphics.FillRectangle(brush, rectangle.Width - 3, rectangle.Top - 1, 1, 1);
						graphics.FillRectangle(brush, rectangle.Width - 2, rectangle.Top - 2, 1, 1);
					}
				}
				using (Brush brush2 = new SolidBrush(color))
				{
					if (flag2)
					{
						Rectangle rect = new Rectangle(rectangle.X - 1, 0, 1, 1);
						if (rightToLeft)
						{
							rect.X = rectangle.Right;
						}
						graphics.FillRectangle(brush2, rect);
					}
					else
					{
						graphics.FillRectangle(brush2, rectangle.X, rectangle.Top - 1, 1, 1);
					}
				}
			}
		}

		// Token: 0x06005C1E RID: 23582 RVA: 0x0014F5A8 File Offset: 0x0014E5A8
		private void RenderToolStripCurve(ToolStripRenderEventArgs e)
		{
			Rectangle rectangle = new Rectangle(Point.Empty, e.ToolStrip.Size);
			ToolStrip toolStrip = e.ToolStrip;
			Rectangle displayRectangle = toolStrip.DisplayRectangle;
			Graphics graphics = e.Graphics;
			Point empty = Point.Empty;
			Point location = new Point(rectangle.Width - 1, 0);
			Point point = new Point(0, rectangle.Height - 1);
			using (Brush brush = new SolidBrush(this.ColorTable.ToolStripGradientMiddle))
			{
				Rectangle rectangle2 = new Rectangle(empty, ToolStripProfessionalRenderer.onePix);
				rectangle2.X++;
				Rectangle rectangle3 = new Rectangle(empty, ToolStripProfessionalRenderer.onePix);
				rectangle3.Y++;
				Rectangle rectangle4 = new Rectangle(location, ToolStripProfessionalRenderer.onePix);
				rectangle4.X -= 2;
				Rectangle rectangle5 = rectangle4;
				rectangle5.Y++;
				rectangle5.X++;
				Rectangle[] array = new Rectangle[]
				{
					rectangle2,
					rectangle3,
					rectangle4,
					rectangle5
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (displayRectangle.IntersectsWith(array[i]))
					{
						array[i] = Rectangle.Empty;
					}
				}
				graphics.FillRectangles(brush, array);
			}
			using (Brush brush2 = new SolidBrush(this.ColorTable.ToolStripGradientEnd))
			{
				Point point2 = point;
				point2.Offset(1, -1);
				if (!displayRectangle.Contains(point2))
				{
					graphics.FillRectangle(brush2, new Rectangle(point2, ToolStripProfessionalRenderer.onePix));
				}
				Rectangle rect = new Rectangle(point.X, point.Y - 2, 1, 1);
				if (!displayRectangle.IntersectsWith(rect))
				{
					graphics.FillRectangle(brush2, rect);
				}
			}
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x0014F7E0 File Offset: 0x0014E7E0
		private void RenderSelectedButtonFill(Graphics g, Rectangle bounds)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			if (!this.UseSystemColors)
			{
				using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.ButtonSelectedGradientBegin, this.ColorTable.ButtonSelectedGradientEnd, LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, bounds);
					return;
				}
			}
			Color buttonSelectedHighlight = this.ColorTable.ButtonSelectedHighlight;
			using (Brush brush2 = new SolidBrush(buttonSelectedHighlight))
			{
				g.FillRectangle(brush2, bounds);
			}
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x0014F880 File Offset: 0x0014E880
		private void RenderCheckedButtonFill(Graphics g, Rectangle bounds)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			if (!this.UseSystemColors)
			{
				using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.ButtonCheckedGradientBegin, this.ColorTable.ButtonCheckedGradientEnd, LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, bounds);
					return;
				}
			}
			Color buttonCheckedHighlight = this.ColorTable.ButtonCheckedHighlight;
			using (Brush brush2 = new SolidBrush(buttonCheckedHighlight))
			{
				g.FillRectangle(brush2, bounds);
			}
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x0014F920 File Offset: 0x0014E920
		private void RenderSeparatorInternal(Graphics g, ToolStripItem item, Rectangle bounds, bool vertical)
		{
			Color separatorDark = this.ColorTable.SeparatorDark;
			Color separatorLight = this.ColorTable.SeparatorLight;
			Pen pen = new Pen(separatorDark);
			Pen pen2 = new Pen(separatorLight);
			bool flag = true;
			bool flag2 = true;
			bool flag3 = item is ToolStripSeparator;
			bool flag4 = false;
			if (flag3)
			{
				if (vertical)
				{
					if (!item.IsOnDropDown)
					{
						bounds.Y += 3;
						bounds.Height = Math.Max(0, bounds.Height - 6);
					}
				}
				else
				{
					ToolStripDropDownMenu toolStripDropDownMenu = item.GetCurrentParent() as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						if (toolStripDropDownMenu.RightToLeft == RightToLeft.No)
						{
							bounds.X += toolStripDropDownMenu.Padding.Left - 2;
							bounds.Width = toolStripDropDownMenu.Width - bounds.X;
						}
						else
						{
							bounds.X += 2;
							bounds.Width = toolStripDropDownMenu.Width - bounds.X - toolStripDropDownMenu.Padding.Right;
						}
					}
					else
					{
						flag4 = true;
					}
				}
			}
			try
			{
				if (vertical)
				{
					if (bounds.Height >= 4)
					{
						bounds.Inflate(0, -2);
					}
					bool flag5 = item.RightToLeft == RightToLeft.Yes;
					Pen pen3 = flag5 ? pen2 : pen;
					Pen pen4 = flag5 ? pen : pen2;
					int num = bounds.Width / 2;
					g.DrawLine(pen3, num, bounds.Top, num, bounds.Bottom - 1);
					num++;
					g.DrawLine(pen4, num, bounds.Top + 1, num, bounds.Bottom);
				}
				else
				{
					if (flag4 && bounds.Width >= 4)
					{
						bounds.Inflate(-2, 0);
					}
					int num2 = bounds.Height / 2;
					g.DrawLine(pen, bounds.Left, num2, bounds.Right - 1, num2);
					if (!flag3 || flag4)
					{
						num2++;
						g.DrawLine(pen2, bounds.Left + 1, num2, bounds.Right - 1, num2);
					}
				}
			}
			finally
			{
				if (flag && pen != null)
				{
					pen.Dispose();
				}
				if (flag2 && pen2 != null)
				{
					pen2.Dispose();
				}
			}
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x0014FB4C File Offset: 0x0014EB4C
		private void RenderPressedButtonFill(Graphics g, Rectangle bounds)
		{
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}
			if (!this.UseSystemColors)
			{
				using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.ButtonPressedGradientBegin, this.ColorTable.ButtonPressedGradientEnd, LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, bounds);
					return;
				}
			}
			Color buttonPressedHighlight = this.ColorTable.ButtonPressedHighlight;
			using (Brush brush2 = new SolidBrush(buttonPressedHighlight))
			{
				g.FillRectangle(brush2, bounds);
			}
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x0014FBEC File Offset: 0x0014EBEC
		private void RenderItemInternal(ToolStripItemRenderEventArgs e, bool useHotBorder)
		{
			Graphics graphics = e.Graphics;
			ToolStripItem item = e.Item;
			Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
			bool flag = false;
			Rectangle clipRect = item.Selected ? item.ContentRectangle : rectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, rectangle, clipRect);
			}
			if (item.Pressed)
			{
				this.RenderPressedButtonFill(graphics, rectangle);
				flag = useHotBorder;
			}
			else if (item.Selected)
			{
				this.RenderSelectedButtonFill(graphics, rectangle);
				flag = useHotBorder;
			}
			else if (item.Owner != null && item.BackColor != item.Owner.BackColor)
			{
				using (Brush brush = new SolidBrush(item.BackColor))
				{
					graphics.FillRectangle(brush, rectangle);
				}
			}
			if (flag)
			{
				using (Pen pen = new Pen(this.ColorTable.ButtonSelectedBorder))
				{
					graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				}
			}
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x0014FD24 File Offset: 0x0014ED24
		internal void RenderArrowInternal(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
		{
			Point point = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
			point.X += dropDownRect.Width % 2;
			Point[] points;
			switch (direction)
			{
			case ArrowDirection.Left:
				points = new Point[]
				{
					new Point(point.X + 2, point.Y - 3),
					new Point(point.X + 2, point.Y + 3),
					new Point(point.X - 1, point.Y)
				};
				break;
			case ArrowDirection.Up:
				points = new Point[]
				{
					new Point(point.X - 2, point.Y + 1),
					new Point(point.X + 3, point.Y + 1),
					new Point(point.X, point.Y - 2)
				};
				break;
			default:
				switch (direction)
				{
				case ArrowDirection.Right:
					points = new Point[]
					{
						new Point(point.X - 2, point.Y - 3),
						new Point(point.X - 2, point.Y + 3),
						new Point(point.X + 1, point.Y)
					};
					goto IL_243;
				}
				points = new Point[]
				{
					new Point(point.X - 2, point.Y - 1),
					new Point(point.X + 3, point.Y - 1),
					new Point(point.X, point.Y + 2)
				};
				break;
			}
			IL_243:
			g.FillPolygon(brush, points);
		}

		// Token: 0x04003905 RID: 14597
		private const int GRIP_PADDING = 4;

		// Token: 0x04003906 RID: 14598
		private const int ICON_WELL_GRADIENT_WIDTH = 12;

		// Token: 0x04003907 RID: 14599
		private const int overflowButtonWidth = 12;

		// Token: 0x04003908 RID: 14600
		private static readonly Size onePix = new Size(1, 1);

		// Token: 0x04003909 RID: 14601
		private Padding dropDownMenuItemPaintPadding = new Padding(2, 0, 1, 0);

		// Token: 0x0400390A RID: 14602
		private ProfessionalColorTable professionalColorTable;

		// Token: 0x0400390B RID: 14603
		private bool roundedEdges = true;

		// Token: 0x0400390C RID: 14604
		private ToolStripRenderer toolStripHighContrastRenderer;

		// Token: 0x0400390D RID: 14605
		private ToolStripRenderer toolStripLowResolutionRenderer;
	}
}
