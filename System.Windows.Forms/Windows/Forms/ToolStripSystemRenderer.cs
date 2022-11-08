using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200068F RID: 1679
	public class ToolStripSystemRenderer : ToolStripRenderer
	{
		// Token: 0x060058AE RID: 22702 RVA: 0x00141E62 File Offset: 0x00140E62
		public ToolStripSystemRenderer()
		{
		}

		// Token: 0x060058AF RID: 22703 RVA: 0x00141E6A File Offset: 0x00140E6A
		internal ToolStripSystemRenderer(bool isDefault) : base(isDefault)
		{
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x060058B0 RID: 22704 RVA: 0x00141E73 File Offset: 0x00140E73
		internal override ToolStripRenderer RendererOverride
		{
			get
			{
				if (DisplayInformation.HighContrast)
				{
					return this.HighContrastRenderer;
				}
				return null;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x060058B1 RID: 22705 RVA: 0x00141E84 File Offset: 0x00140E84
		internal ToolStripRenderer HighContrastRenderer
		{
			get
			{
				if (this.toolStripHighContrastRenderer == null)
				{
					this.toolStripHighContrastRenderer = new ToolStripHighContrastRenderer(true);
				}
				return this.toolStripHighContrastRenderer;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x060058B2 RID: 22706 RVA: 0x00141EA0 File Offset: 0x00140EA0
		private static VisualStyleRenderer VisualStyleRenderer
		{
			get
			{
				if (Application.RenderWithVisualStyles)
				{
					if (ToolStripSystemRenderer.renderer == null && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ToolBar.Button.Normal))
					{
						ToolStripSystemRenderer.renderer = new VisualStyleRenderer(VisualStyleElement.ToolBar.Button.Normal);
					}
				}
				else
				{
					ToolStripSystemRenderer.renderer = null;
				}
				return ToolStripSystemRenderer.renderer;
			}
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x00141ED8 File Offset: 0x00140ED8
		private static void FillBackground(Graphics g, Rectangle bounds, Color backColor)
		{
			if (backColor.IsSystemColor)
			{
				g.FillRectangle(SystemBrushes.FromSystemColor(backColor), bounds);
				return;
			}
			using (Brush brush = new SolidBrush(backColor))
			{
				g.FillRectangle(brush, bounds);
			}
		}

		// Token: 0x060058B4 RID: 22708 RVA: 0x00141F28 File Offset: 0x00140F28
		private static bool GetPen(Color color, ref Pen pen)
		{
			if (color.IsSystemColor)
			{
				pen = SystemPens.FromSystemColor(color);
				return false;
			}
			pen = new Pen(color);
			return true;
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x00141F46 File Offset: 0x00140F46
		private static int GetItemState(ToolStripItem item)
		{
			return (int)ToolStripSystemRenderer.GetToolBarState(item);
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x00141F4E File Offset: 0x00140F4E
		private static int GetSplitButtonDropDownItemState(ToolStripSplitButton item)
		{
			return (int)ToolStripSystemRenderer.GetSplitButtonToolBarState(item, true);
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x00141F57 File Offset: 0x00140F57
		private static int GetSplitButtonItemState(ToolStripSplitButton item)
		{
			return (int)ToolStripSystemRenderer.GetSplitButtonToolBarState(item, false);
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x00141F60 File Offset: 0x00140F60
		private static ToolBarState GetSplitButtonToolBarState(ToolStripSplitButton button, bool dropDownButton)
		{
			ToolBarState result = ToolBarState.Normal;
			if (button != null)
			{
				if (!button.Enabled)
				{
					result = ToolBarState.Disabled;
				}
				else if (dropDownButton)
				{
					if (button.DropDownButtonPressed || button.ButtonPressed)
					{
						result = ToolBarState.Pressed;
					}
					else if (button.DropDownButtonSelected || button.ButtonSelected)
					{
						result = ToolBarState.Hot;
					}
				}
				else if (button.ButtonPressed)
				{
					result = ToolBarState.Pressed;
				}
				else if (button.ButtonSelected)
				{
					result = ToolBarState.Hot;
				}
			}
			return result;
		}

		// Token: 0x060058B9 RID: 22713 RVA: 0x00141FC0 File Offset: 0x00140FC0
		private static ToolBarState GetToolBarState(ToolStripItem item)
		{
			ToolBarState result = ToolBarState.Normal;
			if (item != null)
			{
				if (!item.Enabled)
				{
					result = ToolBarState.Disabled;
				}
				if (item is ToolStripButton && ((ToolStripButton)item).Checked)
				{
					result = ToolBarState.Checked;
				}
				else if (item.Pressed)
				{
					result = ToolBarState.Pressed;
				}
				else if (item.Selected)
				{
					result = ToolBarState.Hot;
				}
			}
			return result;
		}

		// Token: 0x060058BA RID: 22714 RVA: 0x0014200C File Offset: 0x0014100C
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle affectedBounds = e.AffectedBounds;
			if (!base.ShouldPaintBackground(toolStrip))
			{
				return;
			}
			if (toolStrip is StatusStrip)
			{
				ToolStripSystemRenderer.RenderStatusStripBackground(e);
				return;
			}
			if (DisplayInformation.HighContrast)
			{
				ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, SystemColors.ButtonFace);
				return;
			}
			if (DisplayInformation.LowResolution)
			{
				ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, (toolStrip is ToolStripDropDown) ? SystemColors.ControlLight : e.BackColor);
				return;
			}
			if (toolStrip.IsDropDown)
			{
				ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, (!ToolStripManager.VisualStylesEnabled) ? e.BackColor : SystemColors.Menu);
				return;
			}
			if (toolStrip is MenuStrip)
			{
				ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, (!ToolStripManager.VisualStylesEnabled) ? e.BackColor : SystemColors.MenuBar);
				return;
			}
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Rebar.Band.Normal))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(VisualStyleElement.ToolBar.Bar.Normal);
				visualStyleRenderer.DrawBackground(graphics, affectedBounds);
				return;
			}
			ToolStripSystemRenderer.FillBackground(graphics, affectedBounds, (!ToolStripManager.VisualStylesEnabled) ? e.BackColor : SystemColors.MenuBar);
		}

		// Token: 0x060058BB RID: 22715 RVA: 0x00142110 File Offset: 0x00141110
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = e.ToolStrip.ClientRectangle;
			if (toolStrip is StatusStrip)
			{
				this.RenderStatusStripBorder(e);
				return;
			}
			if (toolStrip is ToolStripDropDown)
			{
				ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
				if (toolStripDropDown.DropShadowEnabled && ToolStripManager.VisualStylesEnabled)
				{
					clientRectangle.Width--;
					clientRectangle.Height--;
					e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), clientRectangle);
					return;
				}
				ControlPaint.DrawBorder3D(e.Graphics, clientRectangle, Border3DStyle.Raised);
				return;
			}
			else
			{
				if (ToolStripManager.VisualStylesEnabled)
				{
					e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, clientRectangle.Bottom - 1, clientRectangle.Width, clientRectangle.Bottom - 1);
					e.Graphics.DrawLine(SystemPens.InactiveBorder, 0, clientRectangle.Bottom - 2, clientRectangle.Width, clientRectangle.Bottom - 2);
					return;
				}
				e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, clientRectangle.Bottom - 1, clientRectangle.Width, clientRectangle.Bottom - 1);
				e.Graphics.DrawLine(SystemPens.ButtonShadow, 0, clientRectangle.Bottom - 2, clientRectangle.Width, clientRectangle.Bottom - 2);
				return;
			}
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x00142258 File Offset: 0x00141258
		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle bounds = new Rectangle(Point.Empty, e.GripBounds.Size);
			bool flag = e.GripDisplayStyle == ToolStripGripDisplayStyle.Vertical;
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Rebar.Gripper.Normal))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				if (flag)
				{
					visualStyleRenderer.SetParameters(VisualStyleElement.Rebar.Gripper.Normal);
					bounds.Height = (bounds.Height - 2) / 4 * 4;
					bounds.Y = Math.Max(0, (e.GripBounds.Height - bounds.Height - 2) / 2);
				}
				else
				{
					visualStyleRenderer.SetParameters(VisualStyleElement.Rebar.GripperVertical.Normal);
				}
				visualStyleRenderer.DrawBackground(graphics, bounds);
				return;
			}
			Color backColor = e.ToolStrip.BackColor;
			ToolStripSystemRenderer.FillBackground(graphics, bounds, backColor);
			if (flag)
			{
				if (bounds.Height >= 4)
				{
					bounds.Inflate(0, -2);
				}
				bounds.Width = 3;
			}
			else
			{
				if (bounds.Width >= 4)
				{
					bounds.Inflate(-2, 0);
				}
				bounds.Height = 3;
			}
			this.RenderSmall3DBorderInternal(graphics, bounds, ToolBarState.Hot, e.ToolStrip.RightToLeft == RightToLeft.Yes);
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x00142374 File Offset: 0x00141374
		protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
		{
		}

		// Token: 0x060058BE RID: 22718 RVA: 0x00142376 File Offset: 0x00141376
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
		}

		// Token: 0x060058BF RID: 22719 RVA: 0x00142378 File Offset: 0x00141378
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.RenderItemInternal(e);
		}

		// Token: 0x060058C0 RID: 22720 RVA: 0x00142381 File Offset: 0x00141381
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.RenderItemInternal(e);
		}

		// Token: 0x060058C1 RID: 22721 RVA: 0x0014238C File Offset: 0x0014138C
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(VisualStyleElement.Rebar.Chevron.Normal))
			{
				VisualStyleElement normal = VisualStyleElement.Rebar.Chevron.Normal;
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(normal.ClassName, normal.Part, ToolStripSystemRenderer.GetItemState(item));
				visualStyleRenderer.DrawBackground(graphics, new Rectangle(Point.Empty, item.Size));
				return;
			}
			this.RenderItemInternal(e);
			Color arrowColor = item.Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
			base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, item, new Rectangle(Point.Empty, item.Size), arrowColor, ArrowDirection.Down));
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x00142434 File Offset: 0x00141434
		protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripSystemRenderer.RenderLabelInternal(e);
		}

		// Token: 0x060058C3 RID: 22723 RVA: 0x0014243C File Offset: 0x0014143C
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = e.Item as ToolStripMenuItem;
			Graphics graphics = e.Graphics;
			if (toolStripMenuItem is MdiControlStrip.SystemMenuItem)
			{
				return;
			}
			if (toolStripMenuItem != null)
			{
				Rectangle bounds = new Rectangle(Point.Empty, toolStripMenuItem.Size);
				if (toolStripMenuItem.IsTopLevel && !ToolStripManager.VisualStylesEnabled)
				{
					if (toolStripMenuItem.BackgroundImage != null)
					{
						ControlPaint.DrawBackgroundImage(graphics, toolStripMenuItem.BackgroundImage, toolStripMenuItem.BackColor, toolStripMenuItem.BackgroundImageLayout, toolStripMenuItem.ContentRectangle, toolStripMenuItem.ContentRectangle);
					}
					else if (toolStripMenuItem.RawBackColor != Color.Empty)
					{
						ToolStripSystemRenderer.FillBackground(graphics, toolStripMenuItem.ContentRectangle, toolStripMenuItem.BackColor);
					}
					ToolBarState toolBarState = ToolStripSystemRenderer.GetToolBarState(toolStripMenuItem);
					this.RenderSmall3DBorderInternal(graphics, bounds, toolBarState, toolStripMenuItem.RightToLeft == RightToLeft.Yes);
					return;
				}
				Rectangle rectangle = new Rectangle(Point.Empty, toolStripMenuItem.Size);
				if (toolStripMenuItem.IsOnDropDown)
				{
					rectangle.X += 2;
					rectangle.Width -= 3;
				}
				if (toolStripMenuItem.Selected || toolStripMenuItem.Pressed)
				{
					graphics.FillRectangle(SystemBrushes.Highlight, rectangle);
					return;
				}
				if (toolStripMenuItem.BackgroundImage != null)
				{
					ControlPaint.DrawBackgroundImage(graphics, toolStripMenuItem.BackgroundImage, toolStripMenuItem.BackColor, toolStripMenuItem.BackgroundImageLayout, toolStripMenuItem.ContentRectangle, rectangle);
					return;
				}
				if (!ToolStripManager.VisualStylesEnabled && toolStripMenuItem.RawBackColor != Color.Empty)
				{
					ToolStripSystemRenderer.FillBackground(graphics, rectangle, toolStripMenuItem.BackColor);
				}
			}
		}

		// Token: 0x060058C4 RID: 22724 RVA: 0x0014259E File Offset: 0x0014159E
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			this.RenderSeparatorInternal(e.Graphics, e.Item, new Rectangle(Point.Empty, e.Item.Size), e.Vertical);
		}

		// Token: 0x060058C5 RID: 22725 RVA: 0x001425D0 File Offset: 0x001415D0
		protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripSystemRenderer.RenderLabelInternal(e);
			ToolStripStatusLabel toolStripStatusLabel = e.Item as ToolStripStatusLabel;
			ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0, 0, toolStripStatusLabel.Width - 1, toolStripStatusLabel.Height - 1), toolStripStatusLabel.BorderStyle, (Border3DSide)toolStripStatusLabel.BorderSides);
		}

		// Token: 0x060058C6 RID: 22726 RVA: 0x00142620 File Offset: 0x00141620
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripSplitButton toolStripSplitButton = e.Item as ToolStripSplitButton;
			Graphics graphics = e.Graphics;
			bool flag = toolStripSplitButton.RightToLeft == RightToLeft.Yes;
			Color arrowColor = toolStripSplitButton.Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
			VisualStyleElement visualStyleElement = flag ? VisualStyleElement.ToolBar.SplitButton.Normal : VisualStyleElement.ToolBar.SplitButtonDropDown.Normal;
			VisualStyleElement visualStyleElement2 = flag ? VisualStyleElement.ToolBar.DropDownButton.Normal : VisualStyleElement.ToolBar.SplitButton.Normal;
			Rectangle rectangle = new Rectangle(Point.Empty, toolStripSplitButton.Size);
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(visualStyleElement) && VisualStyleRenderer.IsElementDefined(visualStyleElement2))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(visualStyleElement2.ClassName, visualStyleElement2.Part, ToolStripSystemRenderer.GetSplitButtonItemState(toolStripSplitButton));
				Rectangle buttonBounds = toolStripSplitButton.ButtonBounds;
				if (flag)
				{
					buttonBounds.Inflate(2, 0);
				}
				visualStyleRenderer.DrawBackground(graphics, buttonBounds);
				visualStyleRenderer.SetParameters(visualStyleElement.ClassName, visualStyleElement.Part, ToolStripSystemRenderer.GetSplitButtonDropDownItemState(toolStripSplitButton));
				visualStyleRenderer.DrawBackground(graphics, toolStripSplitButton.DropDownButtonBounds);
				Rectangle contentRectangle = toolStripSplitButton.ContentRectangle;
				if (toolStripSplitButton.BackgroundImage != null)
				{
					ControlPaint.DrawBackgroundImage(graphics, toolStripSplitButton.BackgroundImage, toolStripSplitButton.BackColor, toolStripSplitButton.BackgroundImageLayout, contentRectangle, contentRectangle);
				}
				this.RenderSeparatorInternal(graphics, toolStripSplitButton, toolStripSplitButton.SplitterBounds, true);
				if (flag || toolStripSplitButton.BackgroundImage != null)
				{
					base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, toolStripSplitButton.DropDownButtonBounds, arrowColor, ArrowDirection.Down));
					return;
				}
			}
			else
			{
				Rectangle buttonBounds2 = toolStripSplitButton.ButtonBounds;
				if (toolStripSplitButton.BackgroundImage != null)
				{
					Rectangle clipRect = toolStripSplitButton.Selected ? toolStripSplitButton.ContentRectangle : rectangle;
					if (toolStripSplitButton.BackgroundImage != null)
					{
						ControlPaint.DrawBackgroundImage(graphics, toolStripSplitButton.BackgroundImage, toolStripSplitButton.BackColor, toolStripSplitButton.BackgroundImageLayout, rectangle, clipRect);
					}
				}
				else
				{
					ToolStripSystemRenderer.FillBackground(graphics, buttonBounds2, toolStripSplitButton.BackColor);
				}
				ToolBarState splitButtonToolBarState = ToolStripSystemRenderer.GetSplitButtonToolBarState(toolStripSplitButton, false);
				this.RenderSmall3DBorderInternal(graphics, buttonBounds2, splitButtonToolBarState, flag);
				Rectangle dropDownButtonBounds = toolStripSplitButton.DropDownButtonBounds;
				if (toolStripSplitButton.BackgroundImage == null)
				{
					ToolStripSystemRenderer.FillBackground(graphics, dropDownButtonBounds, toolStripSplitButton.BackColor);
				}
				splitButtonToolBarState = ToolStripSystemRenderer.GetSplitButtonToolBarState(toolStripSplitButton, true);
				if (splitButtonToolBarState == ToolBarState.Pressed || splitButtonToolBarState == ToolBarState.Hot)
				{
					this.RenderSmall3DBorderInternal(graphics, dropDownButtonBounds, splitButtonToolBarState, flag);
				}
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, dropDownButtonBounds, arrowColor, ArrowDirection.Down));
			}
		}

		// Token: 0x060058C7 RID: 22727 RVA: 0x0014283C File Offset: 0x0014183C
		private void RenderItemInternal(ToolStripItemRenderEventArgs e)
		{
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			ToolBarState toolBarState = ToolStripSystemRenderer.GetToolBarState(item);
			VisualStyleElement normal = VisualStyleElement.ToolBar.Button.Normal;
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(normal))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(normal.ClassName, normal.Part, (int)toolBarState);
				visualStyleRenderer.DrawBackground(graphics, new Rectangle(Point.Empty, item.Size));
			}
			else
			{
				this.RenderSmall3DBorderInternal(graphics, new Rectangle(Point.Empty, item.Size), toolBarState, item.RightToLeft == RightToLeft.Yes);
			}
			Rectangle contentRectangle = item.ContentRectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, contentRectangle, contentRectangle);
				return;
			}
			ToolStrip currentParent = item.GetCurrentParent();
			if (currentParent != null && toolBarState != ToolBarState.Checked && item.BackColor != currentParent.BackColor)
			{
				ToolStripSystemRenderer.FillBackground(graphics, contentRectangle, item.BackColor);
			}
		}

		// Token: 0x060058C8 RID: 22728 RVA: 0x00142928 File Offset: 0x00141928
		private void RenderSeparatorInternal(Graphics g, ToolStripItem item, Rectangle bounds, bool vertical)
		{
			VisualStyleElement visualStyleElement = vertical ? VisualStyleElement.ToolBar.SeparatorHorizontal.Normal : VisualStyleElement.ToolBar.SeparatorVertical.Normal;
			if (ToolStripManager.VisualStylesEnabled && VisualStyleRenderer.IsElementDefined(visualStyleElement))
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(visualStyleElement.ClassName, visualStyleElement.Part, ToolStripSystemRenderer.GetItemState(item));
				visualStyleRenderer.DrawBackground(g, bounds);
				return;
			}
			Color foreColor = item.ForeColor;
			Color backColor = item.BackColor;
			Pen controlDark = SystemPens.ControlDark;
			bool pen = ToolStripSystemRenderer.GetPen(foreColor, ref controlDark);
			try
			{
				if (vertical)
				{
					if (bounds.Height >= 4)
					{
						bounds.Inflate(0, -2);
					}
					bool flag = item.RightToLeft == RightToLeft.Yes;
					Pen pen2 = flag ? SystemPens.ButtonHighlight : controlDark;
					Pen pen3 = flag ? controlDark : SystemPens.ButtonHighlight;
					int num = bounds.Width / 2;
					g.DrawLine(pen2, num, bounds.Top, num, bounds.Bottom);
					num++;
					g.DrawLine(pen3, num, bounds.Top, num, bounds.Bottom);
				}
				else
				{
					if (bounds.Width >= 4)
					{
						bounds.Inflate(-2, 0);
					}
					int num2 = bounds.Height / 2;
					g.DrawLine(controlDark, bounds.Left, num2, bounds.Right, num2);
					num2++;
					g.DrawLine(SystemPens.ButtonHighlight, bounds.Left, num2, bounds.Right, num2);
				}
			}
			finally
			{
				if (pen && controlDark != null)
				{
					controlDark.Dispose();
				}
			}
		}

		// Token: 0x060058C9 RID: 22729 RVA: 0x00142AA0 File Offset: 0x00141AA0
		private void RenderSmall3DBorderInternal(Graphics g, Rectangle bounds, ToolBarState state, bool rightToLeft)
		{
			if (state == ToolBarState.Hot || state == ToolBarState.Pressed || state == ToolBarState.Checked)
			{
				Pen pen = (state == ToolBarState.Hot) ? SystemPens.ButtonHighlight : SystemPens.ButtonShadow;
				Pen pen2 = (state == ToolBarState.Hot) ? SystemPens.ButtonShadow : SystemPens.ButtonHighlight;
				Pen pen3 = rightToLeft ? pen2 : pen;
				Pen pen4 = rightToLeft ? pen : pen2;
				g.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top);
				g.DrawLine(pen3, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1);
				g.DrawLine(pen4, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1);
				g.DrawLine(pen2, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
			}
		}

		// Token: 0x060058CA RID: 22730 RVA: 0x00142B8C File Offset: 0x00141B8C
		private void RenderStatusStripBorder(ToolStripRenderEventArgs e)
		{
			if (!Application.RenderWithVisualStyles)
			{
				e.Graphics.DrawLine(SystemPens.ButtonHighlight, 0, 0, e.ToolStrip.Width, 0);
			}
		}

		// Token: 0x060058CB RID: 22731 RVA: 0x00142BB4 File Offset: 0x00141BB4
		private static void RenderStatusStripBackground(ToolStripRenderEventArgs e)
		{
			if (Application.RenderWithVisualStyles)
			{
				VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
				visualStyleRenderer.SetParameters(VisualStyleElement.Status.Bar.Normal);
				visualStyleRenderer.DrawBackground(e.Graphics, new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1));
				return;
			}
			if (!SystemInformation.InLockedTerminalSession())
			{
				e.Graphics.Clear(e.BackColor);
			}
		}

		// Token: 0x060058CC RID: 22732 RVA: 0x00142C20 File Offset: 0x00141C20
		private static void RenderLabelInternal(ToolStripItemRenderEventArgs e)
		{
			ToolStripItem item = e.Item;
			Graphics graphics = e.Graphics;
			Rectangle contentRectangle = item.ContentRectangle;
			if (item.BackgroundImage != null)
			{
				ControlPaint.DrawBackgroundImage(graphics, item.BackgroundImage, item.BackColor, item.BackgroundImageLayout, contentRectangle, contentRectangle);
				return;
			}
			VisualStyleRenderer visualStyleRenderer = ToolStripSystemRenderer.VisualStyleRenderer;
			if (visualStyleRenderer == null || item.BackColor != SystemColors.Control)
			{
				ToolStripSystemRenderer.FillBackground(graphics, contentRectangle, item.BackColor);
			}
		}

		// Token: 0x0400381E RID: 14366
		[ThreadStatic]
		private static VisualStyleRenderer renderer;

		// Token: 0x0400381F RID: 14367
		private ToolStripRenderer toolStripHighContrastRenderer;
	}
}
