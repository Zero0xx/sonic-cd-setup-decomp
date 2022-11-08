using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000684 RID: 1668
	internal abstract class ButtonBaseAdapter
	{
		// Token: 0x060057F7 RID: 22519 RVA: 0x0013DD29 File Offset: 0x0013CD29
		internal ButtonBaseAdapter(ButtonBase control)
		{
			this.control = control;
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x060057F8 RID: 22520 RVA: 0x0013DD38 File Offset: 0x0013CD38
		protected ButtonBase Control
		{
			get
			{
				return this.control;
			}
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x0013DD40 File Offset: 0x0013CD40
		internal void Paint(PaintEventArgs pevent)
		{
			if (this.Control.MouseIsDown)
			{
				this.PaintDown(pevent, CheckState.Unchecked);
				return;
			}
			if (this.Control.MouseIsOver)
			{
				this.PaintOver(pevent, CheckState.Unchecked);
				return;
			}
			this.PaintUp(pevent, CheckState.Unchecked);
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x0013DD78 File Offset: 0x0013CD78
		internal virtual Size GetPreferredSizeCore(Size proposedSize)
		{
			Size preferredSizeCore;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				using (PaintEventArgs paintEventArgs = new PaintEventArgs(graphics, default(Rectangle)))
				{
					ButtonBaseAdapter.LayoutOptions layoutOptions = this.Layout(paintEventArgs);
					preferredSizeCore = layoutOptions.GetPreferredSizeCore(proposedSize);
				}
			}
			return preferredSizeCore;
		}

		// Token: 0x060057FB RID: 22523
		protected abstract ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e);

		// Token: 0x060057FC RID: 22524
		internal abstract void PaintUp(PaintEventArgs e, CheckState state);

		// Token: 0x060057FD RID: 22525
		internal abstract void PaintDown(PaintEventArgs e, CheckState state);

		// Token: 0x060057FE RID: 22526
		internal abstract void PaintOver(PaintEventArgs e, CheckState state);

		// Token: 0x060057FF RID: 22527 RVA: 0x0013DDE0 File Offset: 0x0013CDE0
		internal static Color MixedColor(Color color1, Color color2)
		{
			byte a = color1.A;
			byte r = color1.R;
			byte g = color1.G;
			byte b = color1.B;
			byte a2 = color2.A;
			byte r2 = color2.R;
			byte g2 = color2.G;
			byte b2 = color2.B;
			int alpha = (int)((a + a2) / 2);
			int red = (int)((r + r2) / 2);
			int green = (int)((g + g2) / 2);
			int blue = (int)((b + b2) / 2);
			return Color.FromArgb(alpha, red, green, blue);
		}

		// Token: 0x06005800 RID: 22528 RVA: 0x0013DE60 File Offset: 0x0013CE60
		internal static Brush CreateDitherBrush(Color color1, Color color2)
		{
			Brush result;
			using (Bitmap bitmap = new Bitmap(2, 2))
			{
				bitmap.SetPixel(0, 0, color1);
				bitmap.SetPixel(0, 1, color2);
				bitmap.SetPixel(1, 1, color1);
				bitmap.SetPixel(1, 0, color2);
				result = new TextureBrush(bitmap);
			}
			return result;
		}

		// Token: 0x06005801 RID: 22529 RVA: 0x0013DEC0 File Offset: 0x0013CEC0
		internal virtual StringFormat CreateStringFormat()
		{
			return ControlPaint.CreateStringFormat(this.Control, this.Control.TextAlign, this.Control.ShowToolTip, this.Control.UseMnemonic);
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x0013DEEE File Offset: 0x0013CEEE
		internal virtual TextFormatFlags CreateTextFormatFlags()
		{
			return ControlPaint.CreateTextFormatFlags(this.Control, this.Control.TextAlign, this.Control.ShowToolTip, this.Control.UseMnemonic);
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x0013DF1C File Offset: 0x0013CF1C
		internal static void DrawDitheredFill(Graphics g, Color color1, Color color2, Rectangle bounds)
		{
			using (Brush brush = ButtonBaseAdapter.CreateDitherBrush(color1, color2))
			{
				g.FillRectangle(brush, bounds);
			}
		}

		// Token: 0x06005804 RID: 22532 RVA: 0x0013DF58 File Offset: 0x0013CF58
		protected void Draw3DBorder(Graphics g, Rectangle bounds, ButtonBaseAdapter.ColorData colors, bool raised)
		{
			if (this.Control.BackColor != SystemColors.Control && SystemInformation.HighContrast)
			{
				if (raised)
				{
					this.Draw3DBorderHighContrastRaised(g, ref bounds, colors);
					return;
				}
				ControlPaint.DrawBorder(g, bounds, ControlPaint.Dark(this.Control.BackColor), ButtonBorderStyle.Solid);
				return;
			}
			else
			{
				if (raised)
				{
					this.Draw3DBorderRaised(g, ref bounds, colors);
					return;
				}
				this.Draw3DBorderNormal(g, ref bounds, colors);
				return;
			}
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x0013DFC4 File Offset: 0x0013CFC4
		private void Draw3DBorderHighContrastRaised(Graphics g, ref Rectangle bounds, ButtonBaseAdapter.ColorData colors)
		{
			bool flag = colors.buttonFace.ToKnownColor() == SystemColors.Control.ToKnownColor();
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				Point point = new Point(bounds.X + bounds.Width - 1, bounds.Y);
				Point point2 = new Point(bounds.X, bounds.Y);
				Point point3 = new Point(bounds.X, bounds.Y + bounds.Height - 1);
				Point point4 = new Point(bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1);
				WindowsPen windowsPen = null;
				WindowsPen windowsPen2 = null;
				WindowsPen windowsPen3 = null;
				WindowsPen windowsPen4 = null;
				try
				{
					windowsPen = (flag ? new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlLightLight) : new WindowsPen(windowsGraphics.DeviceContext, colors.highlight));
					windowsGraphics.DrawLine(windowsPen, point, point2);
					windowsGraphics.DrawLine(windowsPen, point2, point3);
					windowsPen2 = (flag ? new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlDarkDark) : new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadowDark));
					point.Offset(0, -1);
					windowsGraphics.DrawLine(windowsPen2, point3, point4);
					windowsGraphics.DrawLine(windowsPen2, point4, point);
					if (flag)
					{
						if (SystemInformation.HighContrast)
						{
							windowsPen3 = new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlLight);
						}
						else
						{
							windowsPen3 = new WindowsPen(windowsGraphics.DeviceContext, SystemColors.Control);
						}
					}
					else if (SystemInformation.HighContrast)
					{
						windowsPen3 = new WindowsPen(windowsGraphics.DeviceContext, colors.highlight);
					}
					else
					{
						windowsPen3 = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonFace);
					}
					point.Offset(-1, 2);
					point2.Offset(1, 1);
					point3.Offset(1, -1);
					point4.Offset(-1, -1);
					windowsGraphics.DrawLine(windowsPen3, point, point2);
					windowsGraphics.DrawLine(windowsPen3, point2, point3);
					windowsPen4 = (flag ? new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlDark) : new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadow));
					point.Offset(0, -1);
					windowsGraphics.DrawLine(windowsPen4, point3, point4);
					windowsGraphics.DrawLine(windowsPen4, point4, point);
				}
				finally
				{
					if (windowsPen != null)
					{
						windowsPen.Dispose();
					}
					if (windowsPen2 != null)
					{
						windowsPen2.Dispose();
					}
					if (windowsPen3 != null)
					{
						windowsPen3.Dispose();
					}
					if (windowsPen4 != null)
					{
						windowsPen4.Dispose();
					}
				}
			}
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x0013E248 File Offset: 0x0013D248
		private void Draw3DBorderNormal(Graphics g, ref Rectangle bounds, ButtonBaseAdapter.ColorData colors)
		{
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				Point point = new Point(bounds.X + bounds.Width - 1, bounds.Y);
				Point point2 = new Point(bounds.X, bounds.Y);
				Point point3 = new Point(bounds.X, bounds.Y + bounds.Height - 1);
				Point point4 = new Point(bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1);
				WindowsPen windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadowDark);
				try
				{
					windowsGraphics.DrawLine(windowsPen, point, point2);
					windowsGraphics.DrawLine(windowsPen, point2, point3);
				}
				finally
				{
					windowsPen.Dispose();
				}
				windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.highlight);
				try
				{
					point.Offset(0, -1);
					windowsGraphics.DrawLine(windowsPen, point3, point4);
					windowsGraphics.DrawLine(windowsPen, point4, point);
				}
				finally
				{
					windowsPen.Dispose();
				}
				windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonFace);
				point.Offset(-1, 2);
				point2.Offset(1, 1);
				point3.Offset(1, -1);
				point4.Offset(-1, -1);
				try
				{
					windowsGraphics.DrawLine(windowsPen, point, point2);
					windowsGraphics.DrawLine(windowsPen, point2, point3);
				}
				finally
				{
					windowsPen.Dispose();
				}
				if (colors.buttonFace.ToKnownColor() == SystemColors.Control.ToKnownColor())
				{
					windowsPen = new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlLight);
				}
				else
				{
					windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonFace);
				}
				try
				{
					point.Offset(0, -1);
					windowsGraphics.DrawLine(windowsPen, point3, point4);
					windowsGraphics.DrawLine(windowsPen, point4, point);
				}
				finally
				{
					windowsPen.Dispose();
				}
			}
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x0013E484 File Offset: 0x0013D484
		private void Draw3DBorderRaised(Graphics g, ref Rectangle bounds, ButtonBaseAdapter.ColorData colors)
		{
			bool flag = colors.buttonFace.ToKnownColor() == SystemColors.Control.ToKnownColor();
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				Point point = new Point(bounds.X + bounds.Width - 1, bounds.Y);
				Point point2 = new Point(bounds.X, bounds.Y);
				Point point3 = new Point(bounds.X, bounds.Y + bounds.Height - 1);
				Point point4 = new Point(bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1);
				WindowsPen windowsPen = flag ? new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlLightLight) : new WindowsPen(windowsGraphics.DeviceContext, colors.highlight);
				try
				{
					windowsGraphics.DrawLine(windowsPen, point, point2);
					windowsGraphics.DrawLine(windowsPen, point2, point3);
				}
				finally
				{
					windowsPen.Dispose();
				}
				if (flag)
				{
					windowsPen = new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlDarkDark);
				}
				else
				{
					windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadowDark);
				}
				try
				{
					point.Offset(0, -1);
					windowsGraphics.DrawLine(windowsPen, point3, point4);
					windowsGraphics.DrawLine(windowsPen, point4, point);
				}
				finally
				{
					windowsPen.Dispose();
				}
				point.Offset(-1, 2);
				point2.Offset(1, 1);
				point3.Offset(1, -1);
				point4.Offset(-1, -1);
				if (flag)
				{
					if (SystemInformation.HighContrast)
					{
						windowsPen = new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlLight);
					}
					else
					{
						windowsPen = new WindowsPen(windowsGraphics.DeviceContext, SystemColors.Control);
					}
				}
				else
				{
					windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonFace);
				}
				try
				{
					windowsGraphics.DrawLine(windowsPen, point, point2);
					windowsGraphics.DrawLine(windowsPen, point2, point3);
				}
				finally
				{
					windowsPen.Dispose();
				}
				if (flag)
				{
					windowsPen = new WindowsPen(windowsGraphics.DeviceContext, SystemColors.ControlDark);
				}
				else
				{
					windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadow);
				}
				try
				{
					point.Offset(0, -1);
					windowsGraphics.DrawLine(windowsPen, point3, point4);
					windowsGraphics.DrawLine(windowsPen, point4, point);
				}
				finally
				{
					windowsPen.Dispose();
				}
			}
		}

		// Token: 0x06005808 RID: 22536 RVA: 0x0013E724 File Offset: 0x0013D724
		protected internal static void Draw3DLiteBorder(Graphics g, Rectangle r, ButtonBaseAdapter.ColorData colors, bool up)
		{
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				Point point = new Point(r.Right - 1, r.Top);
				Point point2 = new Point(r.Left, r.Top);
				Point point3 = new Point(r.Left, r.Bottom - 1);
				Point point4 = new Point(r.Right - 1, r.Bottom - 1);
				WindowsPen windowsPen = up ? new WindowsPen(windowsGraphics.DeviceContext, colors.highlight) : new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadow);
				try
				{
					windowsGraphics.DrawLine(windowsPen, point, point2);
					windowsGraphics.DrawLine(windowsPen, point2, point3);
				}
				finally
				{
					windowsPen.Dispose();
				}
				windowsPen = (up ? new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadow) : new WindowsPen(windowsGraphics.DeviceContext, colors.highlight));
				try
				{
					point.Offset(0, -1);
					windowsGraphics.DrawLine(windowsPen, point3, point4);
					windowsGraphics.DrawLine(windowsPen, point4, point);
				}
				finally
				{
					windowsPen.Dispose();
				}
			}
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x0013E864 File Offset: 0x0013D864
		internal static void DrawFlatBorder(Graphics g, Rectangle r, Color c)
		{
			ControlPaint.DrawBorder(g, r, c, ButtonBorderStyle.Solid);
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x0013E870 File Offset: 0x0013D870
		internal static void DrawFlatBorderWithSize(Graphics g, Rectangle r, Color c, int size)
		{
			bool isSystemColor = c.IsSystemColor;
			SolidBrush solidBrush = null;
			if (size > 1)
			{
				solidBrush = new SolidBrush(c);
			}
			else if (isSystemColor)
			{
				solidBrush = (SolidBrush)SystemBrushes.FromSystemColor(c);
			}
			else
			{
				solidBrush = new SolidBrush(c);
			}
			try
			{
				size = Math.Min(size, Math.Min(r.Width, r.Height));
				g.FillRectangle(solidBrush, r.X, r.Y, size, r.Height);
				g.FillRectangle(solidBrush, r.X + r.Width - size, r.Y, size, r.Height);
				g.FillRectangle(solidBrush, r.X + size, r.Y, r.Width - size * 2, size);
				g.FillRectangle(solidBrush, r.X + size, r.Y + r.Height - size, r.Width - size * 2, size);
			}
			finally
			{
				if (!isSystemColor && solidBrush != null)
				{
					solidBrush.Dispose();
				}
			}
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x0013E97C File Offset: 0x0013D97C
		internal static void DrawFlatFocus(Graphics g, Rectangle r, Color c)
		{
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				using (WindowsPen windowsPen = new WindowsPen(windowsGraphics.DeviceContext, c))
				{
					windowsGraphics.DrawRectangle(windowsPen, r);
				}
			}
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x0013E9DC File Offset: 0x0013D9DC
		private void DrawFocus(Graphics g, Rectangle r)
		{
			if (this.Control.Focused && this.Control.ShowFocusCues)
			{
				ControlPaint.DrawFocusRectangle(g, r, this.Control.ForeColor, this.Control.BackColor);
			}
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x0013EA15 File Offset: 0x0013DA15
		private void DrawImage(Graphics graphics, ButtonBaseAdapter.LayoutData layout)
		{
			if (this.Control.Image != null)
			{
				this.DrawImageCore(graphics, this.Control.Image, layout.imageBounds, layout.imageStart, layout);
			}
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x0013EA44 File Offset: 0x0013DA44
		internal virtual void DrawImageCore(Graphics graphics, Image image, Rectangle imageBounds, Point imageStart, ButtonBaseAdapter.LayoutData layout)
		{
			Region clip = graphics.Clip;
			if (!layout.options.everettButtonCompat)
			{
				Rectangle rect = new Rectangle(ButtonBaseAdapter.buttonBorderSize, ButtonBaseAdapter.buttonBorderSize, this.Control.Width - 2 * ButtonBaseAdapter.buttonBorderSize, this.Control.Height - 2 * ButtonBaseAdapter.buttonBorderSize);
				Region region = clip.Clone();
				region.Intersect(rect);
				region.Intersect(imageBounds);
				graphics.Clip = region;
			}
			else
			{
				imageBounds.Width++;
				imageBounds.Height++;
				imageBounds.X = imageStart.X + 1;
				imageBounds.Y = imageStart.Y + 1;
			}
			try
			{
				if (!this.Control.Enabled)
				{
					ControlPaint.DrawImageDisabled(graphics, image, imageBounds, this.Control.BackColor, true);
				}
				else
				{
					graphics.DrawImage(image, imageBounds.X, imageBounds.Y, image.Width, image.Height);
				}
			}
			finally
			{
				if (!layout.options.everettButtonCompat)
				{
					graphics.Clip = clip;
				}
			}
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x0013EB68 File Offset: 0x0013DB68
		internal static void DrawDefaultBorder(Graphics g, Rectangle r, Color c, bool isDefault)
		{
			if (isDefault)
			{
				r.Inflate(1, 1);
				Pen pen;
				if (c.IsSystemColor)
				{
					pen = SystemPens.FromSystemColor(c);
				}
				else
				{
					pen = new Pen(c);
				}
				g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
				if (!c.IsSystemColor)
				{
					pen.Dispose();
				}
			}
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x0013EBD0 File Offset: 0x0013DBD0
		private void DrawText(Graphics g, ButtonBaseAdapter.LayoutData layout, Color c, ButtonBaseAdapter.ColorData colors)
		{
			Rectangle textBounds = layout.textBounds;
			bool shadowedText = layout.options.shadowedText;
			if (this.Control.UseCompatibleTextRendering)
			{
				using (StringFormat stringFormat = this.CreateStringFormat())
				{
					if ((this.Control.TextAlign & (ContentAlignment)546) == (ContentAlignment)0)
					{
						textBounds.X--;
					}
					textBounds.Width++;
					if (shadowedText && !this.Control.Enabled)
					{
						textBounds.Offset(1, 1);
						using (SolidBrush solidBrush = new SolidBrush(colors.highlight))
						{
							g.DrawString(this.Control.Text, this.Control.Font, solidBrush, textBounds, stringFormat);
							textBounds.Offset(-1, -1);
							solidBrush.Color = colors.buttonShadow;
							g.DrawString(this.Control.Text, this.Control.Font, solidBrush, textBounds, stringFormat);
							goto IL_141;
						}
					}
					Brush brush;
					if (c.IsSystemColor)
					{
						brush = SystemBrushes.FromSystemColor(c);
					}
					else
					{
						brush = new SolidBrush(c);
					}
					g.DrawString(this.Control.Text, this.Control.Font, brush, textBounds, stringFormat);
					if (!c.IsSystemColor)
					{
						brush.Dispose();
					}
					IL_141:
					return;
				}
			}
			TextFormatFlags flags = this.CreateTextFormatFlags();
			if (shadowedText && !this.Control.Enabled)
			{
				if (Application.RenderWithVisualStyles)
				{
					TextRenderer.DrawText(g, this.Control.Text, this.Control.Font, textBounds, colors.buttonShadow, flags);
					return;
				}
				textBounds.Offset(1, 1);
				TextRenderer.DrawText(g, this.Control.Text, this.Control.Font, textBounds, colors.highlight, flags);
				textBounds.Offset(-1, -1);
				TextRenderer.DrawText(g, this.Control.Text, this.Control.Font, textBounds, colors.buttonShadow, flags);
				return;
			}
			else
			{
				TextRenderer.DrawText(g, this.Control.Text, this.Control.Font, textBounds, c, flags);
			}
		}

		// Token: 0x06005811 RID: 22545 RVA: 0x0013EE2C File Offset: 0x0013DE2C
		internal static void PaintButtonBackground(WindowsGraphics wg, Rectangle bounds, WindowsBrush background)
		{
			wg.FillRectangle(background, bounds);
		}

		// Token: 0x06005812 RID: 22546 RVA: 0x0013EE36 File Offset: 0x0013DE36
		internal void PaintButtonBackground(PaintEventArgs e, Rectangle bounds, Brush background)
		{
			if (background == null)
			{
				this.Control.PaintBackground(e, bounds);
				return;
			}
			e.Graphics.FillRectangle(background, bounds);
		}

		// Token: 0x06005813 RID: 22547 RVA: 0x0013EE58 File Offset: 0x0013DE58
		internal void PaintField(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, ButtonBaseAdapter.ColorData colors, Color foreColor, bool drawFocus)
		{
			Graphics graphics = e.Graphics;
			Rectangle focus = layout.focus;
			this.DrawText(graphics, layout, foreColor, colors);
			if (drawFocus)
			{
				this.DrawFocus(graphics, focus);
			}
		}

		// Token: 0x06005814 RID: 22548 RVA: 0x0013EE8C File Offset: 0x0013DE8C
		internal void PaintImage(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout)
		{
			Graphics graphics = e.Graphics;
			this.DrawImage(graphics, layout);
		}

		// Token: 0x06005815 RID: 22549 RVA: 0x0013EEA8 File Offset: 0x0013DEA8
		internal static ButtonBaseAdapter.LayoutOptions CommonLayout(Rectangle clientRectangle, Padding padding, bool isDefault, Font font, string text, bool enabled, ContentAlignment textAlign, RightToLeft rtl)
		{
			return new ButtonBaseAdapter.LayoutOptions
			{
				client = LayoutUtils.DeflateRect(clientRectangle, padding),
				padding = padding,
				growBorderBy1PxWhenDefault = true,
				isDefault = isDefault,
				borderSize = 2,
				paddingSize = 0,
				maxFocus = true,
				focusOddEvenFixup = false,
				font = font,
				text = text,
				imageSize = Size.Empty,
				checkSize = 0,
				checkPaddingSize = 0,
				checkAlign = ContentAlignment.TopLeft,
				imageAlign = ContentAlignment.MiddleCenter,
				textAlign = textAlign,
				hintTextUp = false,
				shadowedText = !enabled,
				layoutRTL = (RightToLeft.Yes == rtl),
				textImageRelation = TextImageRelation.Overlay,
				useCompatibleTextRendering = false
			};
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x0013EF64 File Offset: 0x0013DF64
		internal virtual ButtonBaseAdapter.LayoutOptions CommonLayout()
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = new ButtonBaseAdapter.LayoutOptions();
			layoutOptions.client = LayoutUtils.DeflateRect(this.Control.ClientRectangle, this.Control.Padding);
			layoutOptions.padding = this.Control.Padding;
			layoutOptions.growBorderBy1PxWhenDefault = true;
			layoutOptions.isDefault = this.Control.IsDefault;
			layoutOptions.borderSize = 2;
			layoutOptions.paddingSize = 0;
			layoutOptions.maxFocus = true;
			layoutOptions.focusOddEvenFixup = false;
			layoutOptions.font = this.Control.Font;
			layoutOptions.text = this.Control.Text;
			layoutOptions.imageSize = ((this.Control.Image == null) ? Size.Empty : this.Control.Image.Size);
			layoutOptions.checkSize = 0;
			layoutOptions.checkPaddingSize = 0;
			layoutOptions.checkAlign = ContentAlignment.TopLeft;
			layoutOptions.imageAlign = this.Control.ImageAlign;
			layoutOptions.textAlign = this.Control.TextAlign;
			layoutOptions.hintTextUp = false;
			layoutOptions.shadowedText = !this.Control.Enabled;
			layoutOptions.layoutRTL = (RightToLeft.Yes == this.Control.RightToLeft);
			layoutOptions.textImageRelation = this.Control.TextImageRelation;
			layoutOptions.useCompatibleTextRendering = this.Control.UseCompatibleTextRendering;
			if (this.Control.FlatStyle != FlatStyle.System)
			{
				if (layoutOptions.useCompatibleTextRendering)
				{
					using (StringFormat stringFormat = this.Control.CreateStringFormat())
					{
						layoutOptions.StringFormat = stringFormat;
						return layoutOptions;
					}
				}
				layoutOptions.gdiTextFormatFlags = this.Control.CreateTextFormatFlags();
			}
			return layoutOptions;
		}

		// Token: 0x06005817 RID: 22551 RVA: 0x0013F108 File Offset: 0x0013E108
		private static ButtonBaseAdapter.ColorOptions CommonRender(Graphics g, Color foreColor, Color backColor, bool enabled)
		{
			return new ButtonBaseAdapter.ColorOptions(g, foreColor, backColor)
			{
				enabled = enabled
			};
		}

		// Token: 0x06005818 RID: 22552 RVA: 0x0013F128 File Offset: 0x0013E128
		private ButtonBaseAdapter.ColorOptions CommonRender(Graphics g)
		{
			return new ButtonBaseAdapter.ColorOptions(g, this.Control.ForeColor, this.Control.BackColor)
			{
				enabled = this.Control.Enabled
			};
		}

		// Token: 0x06005819 RID: 22553 RVA: 0x0013F164 File Offset: 0x0013E164
		protected ButtonBaseAdapter.ColorOptions PaintRender(Graphics g)
		{
			return this.CommonRender(g);
		}

		// Token: 0x0600581A RID: 22554 RVA: 0x0013F17C File Offset: 0x0013E17C
		internal static ButtonBaseAdapter.ColorOptions PaintFlatRender(Graphics g, Color foreColor, Color backColor, bool enabled)
		{
			ButtonBaseAdapter.ColorOptions colorOptions = ButtonBaseAdapter.CommonRender(g, foreColor, backColor, enabled);
			colorOptions.disabledTextDim = true;
			return colorOptions;
		}

		// Token: 0x0600581B RID: 22555 RVA: 0x0013F19C File Offset: 0x0013E19C
		protected ButtonBaseAdapter.ColorOptions PaintFlatRender(Graphics g)
		{
			ButtonBaseAdapter.ColorOptions colorOptions = this.CommonRender(g);
			colorOptions.disabledTextDim = true;
			return colorOptions;
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x0013F1BC File Offset: 0x0013E1BC
		internal static ButtonBaseAdapter.ColorOptions PaintPopupRender(Graphics g, Color foreColor, Color backColor, bool enabled)
		{
			ButtonBaseAdapter.ColorOptions colorOptions = ButtonBaseAdapter.CommonRender(g, foreColor, backColor, enabled);
			colorOptions.disabledTextDim = true;
			return colorOptions;
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x0013F1DC File Offset: 0x0013E1DC
		protected ButtonBaseAdapter.ColorOptions PaintPopupRender(Graphics g)
		{
			ButtonBaseAdapter.ColorOptions colorOptions = this.CommonRender(g);
			colorOptions.disabledTextDim = true;
			return colorOptions;
		}

		// Token: 0x040037B5 RID: 14261
		private ButtonBase control;

		// Token: 0x040037B6 RID: 14262
		protected static int buttonBorderSize = 4;

		// Token: 0x02000685 RID: 1669
		internal class ColorOptions
		{
			// Token: 0x0600581F RID: 22559 RVA: 0x0013F201 File Offset: 0x0013E201
			internal ColorOptions(Graphics graphics, Color foreColor, Color backColor)
			{
				this.graphics = graphics;
				this.backColor = backColor;
				this.foreColor = foreColor;
				this.highContrast = SystemInformation.HighContrast;
			}

			// Token: 0x06005820 RID: 22560 RVA: 0x0013F22C File Offset: 0x0013E22C
			internal static int Adjust255(float percentage, int value)
			{
				int num = (int)(percentage * (float)value);
				if (num > 255)
				{
					return 255;
				}
				return num;
			}

			// Token: 0x06005821 RID: 22561 RVA: 0x0013F250 File Offset: 0x0013E250
			internal ButtonBaseAdapter.ColorData Calculate()
			{
				ButtonBaseAdapter.ColorData colorData = new ButtonBaseAdapter.ColorData(this);
				colorData.buttonFace = this.backColor;
				if (this.backColor == SystemColors.Control)
				{
					colorData.buttonShadow = SystemColors.ControlDark;
					colorData.buttonShadowDark = SystemColors.ControlDarkDark;
					colorData.highlight = SystemColors.ControlLightLight;
				}
				else if (!this.highContrast)
				{
					colorData.buttonShadow = ControlPaint.Dark(this.backColor);
					colorData.buttonShadowDark = ControlPaint.DarkDark(this.backColor);
					colorData.highlight = ControlPaint.LightLight(this.backColor);
				}
				else
				{
					colorData.buttonShadow = ControlPaint.Dark(this.backColor);
					colorData.buttonShadowDark = ControlPaint.LightLight(this.backColor);
					colorData.highlight = ControlPaint.LightLight(this.backColor);
				}
				float percentage = 0.9f;
				if ((double)colorData.buttonFace.GetBrightness() < 0.5)
				{
					percentage = 1.2f;
				}
				colorData.lowButtonFace = Color.FromArgb(ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)colorData.buttonFace.R), ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)colorData.buttonFace.G), ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)colorData.buttonFace.B));
				percentage = 0.9f;
				if ((double)colorData.highlight.GetBrightness() < 0.5)
				{
					percentage = 1.2f;
				}
				colorData.lowHighlight = Color.FromArgb(ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)colorData.highlight.R), ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)colorData.highlight.G), ButtonBaseAdapter.ColorOptions.Adjust255(percentage, (int)colorData.highlight.B));
				if (this.highContrast && this.backColor != SystemColors.Control)
				{
					colorData.highlight = colorData.lowHighlight;
				}
				colorData.windowFrame = this.foreColor;
				if ((double)colorData.buttonFace.GetBrightness() < 0.5)
				{
					colorData.constrastButtonShadow = colorData.lowHighlight;
				}
				else
				{
					colorData.constrastButtonShadow = colorData.buttonShadow;
				}
				if (!this.enabled && this.disabledTextDim)
				{
					colorData.windowText = colorData.buttonShadow;
				}
				else
				{
					colorData.windowText = colorData.windowFrame;
				}
				IntPtr hdc = this.graphics.GetHdc();
				try
				{
					using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
					{
						colorData.buttonFace = windowsGraphics.GetNearestColor(colorData.buttonFace);
						colorData.buttonShadow = windowsGraphics.GetNearestColor(colorData.buttonShadow);
						colorData.buttonShadowDark = windowsGraphics.GetNearestColor(colorData.buttonShadowDark);
						colorData.constrastButtonShadow = windowsGraphics.GetNearestColor(colorData.constrastButtonShadow);
						colorData.windowText = windowsGraphics.GetNearestColor(colorData.windowText);
						colorData.highlight = windowsGraphics.GetNearestColor(colorData.highlight);
						colorData.lowHighlight = windowsGraphics.GetNearestColor(colorData.lowHighlight);
						colorData.lowButtonFace = windowsGraphics.GetNearestColor(colorData.lowButtonFace);
						colorData.windowFrame = windowsGraphics.GetNearestColor(colorData.windowFrame);
					}
				}
				finally
				{
					this.graphics.ReleaseHdc();
				}
				return colorData;
			}

			// Token: 0x040037B7 RID: 14263
			internal Color backColor;

			// Token: 0x040037B8 RID: 14264
			internal Color foreColor;

			// Token: 0x040037B9 RID: 14265
			internal bool enabled;

			// Token: 0x040037BA RID: 14266
			internal bool disabledTextDim;

			// Token: 0x040037BB RID: 14267
			internal bool highContrast;

			// Token: 0x040037BC RID: 14268
			internal Graphics graphics;
		}

		// Token: 0x02000686 RID: 1670
		internal class ColorData
		{
			// Token: 0x06005822 RID: 22562 RVA: 0x0013F550 File Offset: 0x0013E550
			internal ColorData(ButtonBaseAdapter.ColorOptions options)
			{
				this.options = options;
			}

			// Token: 0x040037BD RID: 14269
			internal Color buttonFace;

			// Token: 0x040037BE RID: 14270
			internal Color buttonShadow;

			// Token: 0x040037BF RID: 14271
			internal Color buttonShadowDark;

			// Token: 0x040037C0 RID: 14272
			internal Color constrastButtonShadow;

			// Token: 0x040037C1 RID: 14273
			internal Color windowText;

			// Token: 0x040037C2 RID: 14274
			internal Color highlight;

			// Token: 0x040037C3 RID: 14275
			internal Color lowHighlight;

			// Token: 0x040037C4 RID: 14276
			internal Color lowButtonFace;

			// Token: 0x040037C5 RID: 14277
			internal Color windowFrame;

			// Token: 0x040037C6 RID: 14278
			internal ButtonBaseAdapter.ColorOptions options;
		}

		// Token: 0x02000687 RID: 1671
		internal class LayoutOptions
		{
			// Token: 0x17001251 RID: 4689
			// (get) Token: 0x06005823 RID: 22563 RVA: 0x0013F560 File Offset: 0x0013E560
			// (set) Token: 0x06005824 RID: 22564 RVA: 0x0013F5CA File Offset: 0x0013E5CA
			public StringFormat StringFormat
			{
				get
				{
					StringFormat stringFormat = new StringFormat();
					stringFormat.FormatFlags = this.gdipFormatFlags;
					stringFormat.Trimming = this.gdipTrimming;
					stringFormat.HotkeyPrefix = this.gdipHotkeyPrefix;
					stringFormat.Alignment = this.gdipAlignment;
					stringFormat.LineAlignment = this.gdipLineAlignment;
					if (this.disableWordWrapping)
					{
						stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
					}
					return stringFormat;
				}
				set
				{
					this.gdipFormatFlags = value.FormatFlags;
					this.gdipTrimming = value.Trimming;
					this.gdipHotkeyPrefix = value.HotkeyPrefix;
					this.gdipAlignment = value.Alignment;
					this.gdipLineAlignment = value.LineAlignment;
				}
			}

			// Token: 0x17001252 RID: 4690
			// (get) Token: 0x06005825 RID: 22565 RVA: 0x0013F608 File Offset: 0x0013E608
			public TextFormatFlags TextFormatFlags
			{
				get
				{
					if (this.disableWordWrapping)
					{
						return this.gdiTextFormatFlags & ~TextFormatFlags.WordBreak;
					}
					return this.gdiTextFormatFlags;
				}
			}

			// Token: 0x06005826 RID: 22566 RVA: 0x0013F624 File Offset: 0x0013E624
			private Size Compose(Size checkSize, Size imageSize, Size textSize)
			{
				ButtonBaseAdapter.LayoutOptions.Composition horizontalComposition = this.GetHorizontalComposition();
				ButtonBaseAdapter.LayoutOptions.Composition verticalComposition = this.GetVerticalComposition();
				return new Size(this.xCompose(horizontalComposition, checkSize.Width, imageSize.Width, textSize.Width), this.xCompose(verticalComposition, checkSize.Height, imageSize.Height, textSize.Height));
			}

			// Token: 0x06005827 RID: 22567 RVA: 0x0013F67C File Offset: 0x0013E67C
			private int xCompose(ButtonBaseAdapter.LayoutOptions.Composition composition, int checkSize, int imageSize, int textSize)
			{
				switch (composition)
				{
				case ButtonBaseAdapter.LayoutOptions.Composition.NoneCombined:
					return checkSize + imageSize + textSize;
				case ButtonBaseAdapter.LayoutOptions.Composition.CheckCombined:
					return Math.Max(checkSize, imageSize + textSize);
				case ButtonBaseAdapter.LayoutOptions.Composition.TextImageCombined:
					return Math.Max(imageSize, textSize) + checkSize;
				case ButtonBaseAdapter.LayoutOptions.Composition.AllCombined:
					return Math.Max(Math.Max(checkSize, imageSize), textSize);
				default:
					return -7107;
				}
			}

			// Token: 0x06005828 RID: 22568 RVA: 0x0013F6D4 File Offset: 0x0013E6D4
			private Size Decompose(Size checkSize, Size imageSize, Size proposedSize)
			{
				ButtonBaseAdapter.LayoutOptions.Composition horizontalComposition = this.GetHorizontalComposition();
				ButtonBaseAdapter.LayoutOptions.Composition verticalComposition = this.GetVerticalComposition();
				return new Size(this.xDecompose(horizontalComposition, checkSize.Width, imageSize.Width, proposedSize.Width), this.xDecompose(verticalComposition, checkSize.Height, imageSize.Height, proposedSize.Height));
			}

			// Token: 0x06005829 RID: 22569 RVA: 0x0013F72C File Offset: 0x0013E72C
			private int xDecompose(ButtonBaseAdapter.LayoutOptions.Composition composition, int checkSize, int imageSize, int proposedSize)
			{
				switch (composition)
				{
				case ButtonBaseAdapter.LayoutOptions.Composition.NoneCombined:
					return proposedSize - (checkSize + imageSize);
				case ButtonBaseAdapter.LayoutOptions.Composition.CheckCombined:
					return proposedSize - imageSize;
				case ButtonBaseAdapter.LayoutOptions.Composition.TextImageCombined:
					return proposedSize - checkSize;
				case ButtonBaseAdapter.LayoutOptions.Composition.AllCombined:
					return proposedSize;
				default:
					return -7109;
				}
			}

			// Token: 0x0600582A RID: 22570 RVA: 0x0013F76C File Offset: 0x0013E76C
			private ButtonBaseAdapter.LayoutOptions.Composition GetHorizontalComposition()
			{
				BitVector32 bitVector = default(BitVector32);
				bitVector[ButtonBaseAdapter.LayoutOptions.combineCheck] = (this.checkAlign == ContentAlignment.MiddleCenter || !LayoutUtils.IsHorizontalAlignment(this.checkAlign));
				bitVector[ButtonBaseAdapter.LayoutOptions.combineImageText] = !LayoutUtils.IsHorizontalRelation(this.textImageRelation);
				return (ButtonBaseAdapter.LayoutOptions.Composition)bitVector.Data;
			}

			// Token: 0x0600582B RID: 22571 RVA: 0x0013F7CC File Offset: 0x0013E7CC
			internal Size GetPreferredSizeCore(Size proposedSize)
			{
				int num = this.borderSize * 2 + this.paddingSize * 2;
				if (this.growBorderBy1PxWhenDefault)
				{
					num += 2;
				}
				Size sz = new Size(num, num);
				proposedSize -= sz;
				int fullCheckSize = this.FullCheckSize;
				Size size = (fullCheckSize > 0) ? new Size(fullCheckSize + 1, fullCheckSize) : Size.Empty;
				Size sz2 = new Size(this.textImageInset * 2, this.textImageInset * 2);
				Size size2 = (this.imageSize != Size.Empty) ? (this.imageSize + sz2) : Size.Empty;
				proposedSize -= sz2;
				proposedSize = this.Decompose(size, size2, proposedSize);
				Size textSize = Size.Empty;
				if (!string.IsNullOrEmpty(this.text))
				{
					try
					{
						this.disableWordWrapping = true;
						textSize = this.GetTextSize(proposedSize) + sz2;
					}
					finally
					{
						this.disableWordWrapping = false;
					}
				}
				Size sz3 = this.Compose(size, this.imageSize, textSize);
				return sz3 + sz;
			}

			// Token: 0x0600582C RID: 22572 RVA: 0x0013F8DC File Offset: 0x0013E8DC
			private ButtonBaseAdapter.LayoutOptions.Composition GetVerticalComposition()
			{
				BitVector32 bitVector = default(BitVector32);
				bitVector[ButtonBaseAdapter.LayoutOptions.combineCheck] = (this.checkAlign == ContentAlignment.MiddleCenter || !LayoutUtils.IsVerticalAlignment(this.checkAlign));
				bitVector[ButtonBaseAdapter.LayoutOptions.combineImageText] = !LayoutUtils.IsVerticalRelation(this.textImageRelation);
				return (ButtonBaseAdapter.LayoutOptions.Composition)bitVector.Data;
			}

			// Token: 0x17001253 RID: 4691
			// (get) Token: 0x0600582D RID: 22573 RVA: 0x0013F939 File Offset: 0x0013E939
			private int FullBorderSize
			{
				get
				{
					if (this.OnePixExtraBorder)
					{
						this.borderSize++;
					}
					return this.borderSize;
				}
			}

			// Token: 0x17001254 RID: 4692
			// (get) Token: 0x0600582E RID: 22574 RVA: 0x0013F957 File Offset: 0x0013E957
			private bool OnePixExtraBorder
			{
				get
				{
					return this.growBorderBy1PxWhenDefault && this.isDefault;
				}
			}

			// Token: 0x0600582F RID: 22575 RVA: 0x0013F96C File Offset: 0x0013E96C
			internal ButtonBaseAdapter.LayoutData Layout()
			{
				ButtonBaseAdapter.LayoutData layoutData = new ButtonBaseAdapter.LayoutData(this);
				layoutData.client = this.client;
				int fullBorderSize = this.FullBorderSize;
				layoutData.face = Rectangle.Inflate(layoutData.client, -fullBorderSize, -fullBorderSize);
				this.CalcCheckmarkRectangle(layoutData);
				this.LayoutTextAndImage(layoutData);
				if (this.maxFocus)
				{
					layoutData.focus = layoutData.field;
					layoutData.focus.Inflate(-1, -1);
					layoutData.focus = LayoutUtils.InflateRect(layoutData.focus, this.padding);
				}
				else
				{
					Rectangle rectangle = new Rectangle(layoutData.textBounds.X - 1, layoutData.textBounds.Y - 1, layoutData.textBounds.Width + 2, layoutData.textBounds.Height + 3);
					if (this.imageSize != Size.Empty)
					{
						layoutData.focus = Rectangle.Union(rectangle, layoutData.imageBounds);
					}
					else
					{
						layoutData.focus = rectangle;
					}
				}
				if (this.focusOddEvenFixup)
				{
					if (layoutData.focus.Height % 2 == 0)
					{
						ButtonBaseAdapter.LayoutData layoutData2 = layoutData;
						layoutData2.focus.Y = layoutData2.focus.Y + 1;
						ButtonBaseAdapter.LayoutData layoutData3 = layoutData;
						layoutData3.focus.Height = layoutData3.focus.Height - 1;
					}
					if (layoutData.focus.Width % 2 == 0)
					{
						ButtonBaseAdapter.LayoutData layoutData4 = layoutData;
						layoutData4.focus.X = layoutData4.focus.X + 1;
						ButtonBaseAdapter.LayoutData layoutData5 = layoutData;
						layoutData5.focus.Width = layoutData5.focus.Width - 1;
					}
				}
				return layoutData;
			}

			// Token: 0x06005830 RID: 22576 RVA: 0x0013FACC File Offset: 0x0013EACC
			private TextImageRelation RtlTranslateRelation(TextImageRelation relation)
			{
				if (this.layoutRTL)
				{
					if (relation == TextImageRelation.ImageBeforeText)
					{
						return TextImageRelation.TextBeforeImage;
					}
					if (relation == TextImageRelation.TextBeforeImage)
					{
						return TextImageRelation.ImageBeforeText;
					}
				}
				return relation;
			}

			// Token: 0x06005831 RID: 22577 RVA: 0x0013FAF4 File Offset: 0x0013EAF4
			internal ContentAlignment RtlTranslateContent(ContentAlignment align)
			{
				if (this.layoutRTL)
				{
					ContentAlignment[][] array = new ContentAlignment[][]
					{
						new ContentAlignment[]
						{
							ContentAlignment.TopLeft,
							ContentAlignment.TopRight
						},
						new ContentAlignment[]
						{
							ContentAlignment.MiddleLeft,
							ContentAlignment.MiddleRight
						},
						new ContentAlignment[]
						{
							ContentAlignment.BottomLeft,
							ContentAlignment.BottomRight
						}
					};
					for (int i = 0; i < 3; i++)
					{
						if (array[i][0] == align)
						{
							return array[i][1];
						}
						if (array[i][1] == align)
						{
							return array[i][0];
						}
					}
				}
				return align;
			}

			// Token: 0x17001255 RID: 4693
			// (get) Token: 0x06005832 RID: 22578 RVA: 0x0013FB80 File Offset: 0x0013EB80
			private int FullCheckSize
			{
				get
				{
					return this.checkSize + this.checkPaddingSize;
				}
			}

			// Token: 0x06005833 RID: 22579 RVA: 0x0013FB90 File Offset: 0x0013EB90
			private void CalcCheckmarkRectangle(ButtonBaseAdapter.LayoutData layout)
			{
				int fullCheckSize = this.FullCheckSize;
				layout.checkBounds = new Rectangle(this.client.X, this.client.Y, fullCheckSize, fullCheckSize);
				ContentAlignment contentAlignment = this.RtlTranslateContent(this.checkAlign);
				Rectangle field = Rectangle.Inflate(layout.face, -this.paddingSize, -this.paddingSize);
				layout.field = field;
				if (fullCheckSize > 0)
				{
					if ((contentAlignment & (ContentAlignment)1092) != (ContentAlignment)0)
					{
						layout.checkBounds.X = field.X + field.Width - layout.checkBounds.Width;
					}
					else if ((contentAlignment & (ContentAlignment)546) != (ContentAlignment)0)
					{
						layout.checkBounds.X = field.X + (field.Width - layout.checkBounds.Width) / 2;
					}
					if ((contentAlignment & (ContentAlignment)1792) != (ContentAlignment)0)
					{
						layout.checkBounds.Y = field.Y + field.Height - layout.checkBounds.Height;
					}
					else if ((contentAlignment & (ContentAlignment)7) != (ContentAlignment)0)
					{
						layout.checkBounds.Y = field.Y + 2;
					}
					else
					{
						layout.checkBounds.Y = field.Y + (field.Height - layout.checkBounds.Height) / 2;
					}
					ContentAlignment contentAlignment2 = contentAlignment;
					if (contentAlignment2 <= ContentAlignment.MiddleCenter)
					{
						switch (contentAlignment2)
						{
						case ContentAlignment.TopLeft:
							break;
						case ContentAlignment.TopCenter:
							layout.checkArea.X = field.X;
							layout.checkArea.Width = field.Width;
							layout.checkArea.Y = field.Y;
							layout.checkArea.Height = fullCheckSize;
							layout.field.Y = layout.field.Y + fullCheckSize;
							layout.field.Height = layout.field.Height - fullCheckSize;
							goto IL_34D;
						case (ContentAlignment)3:
							goto IL_34D;
						case ContentAlignment.TopRight:
							goto IL_20E;
						default:
							if (contentAlignment2 != ContentAlignment.MiddleLeft)
							{
								if (contentAlignment2 != ContentAlignment.MiddleCenter)
								{
									goto IL_34D;
								}
								layout.checkArea = layout.checkBounds;
								goto IL_34D;
							}
							break;
						}
					}
					else if (contentAlignment2 <= ContentAlignment.BottomLeft)
					{
						if (contentAlignment2 == ContentAlignment.MiddleRight)
						{
							goto IL_20E;
						}
						if (contentAlignment2 != ContentAlignment.BottomLeft)
						{
							goto IL_34D;
						}
					}
					else
					{
						if (contentAlignment2 == ContentAlignment.BottomCenter)
						{
							layout.checkArea.X = field.X;
							layout.checkArea.Width = field.Width;
							layout.checkArea.Y = field.Y + field.Height - fullCheckSize;
							layout.checkArea.Height = fullCheckSize;
							layout.field.Height = layout.field.Height - fullCheckSize;
							goto IL_34D;
						}
						if (contentAlignment2 != ContentAlignment.BottomRight)
						{
							goto IL_34D;
						}
						goto IL_20E;
					}
					layout.checkArea.X = field.X;
					layout.checkArea.Width = fullCheckSize + 1;
					layout.checkArea.Y = field.Y;
					layout.checkArea.Height = field.Height;
					layout.field.X = layout.field.X + (fullCheckSize + 1);
					layout.field.Width = layout.field.Width - (fullCheckSize + 1);
					goto IL_34D;
					IL_20E:
					layout.checkArea.X = field.X + field.Width - fullCheckSize;
					layout.checkArea.Width = fullCheckSize + 1;
					layout.checkArea.Y = field.Y;
					layout.checkArea.Height = field.Height;
					layout.field.Width = layout.field.Width - (fullCheckSize + 1);
					IL_34D:
					layout.checkBounds.Width = layout.checkBounds.Width - this.checkPaddingSize;
					layout.checkBounds.Height = layout.checkBounds.Height - this.checkPaddingSize;
				}
			}

			// Token: 0x06005834 RID: 22580 RVA: 0x0013FF1A File Offset: 0x0013EF1A
			private static TextImageRelation ImageAlignToRelation(ContentAlignment alignment)
			{
				return ButtonBaseAdapter.LayoutOptions._imageAlignToRelation[LayoutUtils.ContentAlignmentToIndex(alignment)];
			}

			// Token: 0x06005835 RID: 22581 RVA: 0x0013FF28 File Offset: 0x0013EF28
			private static TextImageRelation TextAlignToRelation(ContentAlignment alignment)
			{
				return LayoutUtils.GetOppositeTextImageRelation(ButtonBaseAdapter.LayoutOptions.ImageAlignToRelation(alignment));
			}

			// Token: 0x06005836 RID: 22582 RVA: 0x0013FF38 File Offset: 0x0013EF38
			internal void LayoutTextAndImage(ButtonBaseAdapter.LayoutData layout)
			{
				ContentAlignment contentAlignment = this.RtlTranslateContent(this.imageAlign);
				ContentAlignment contentAlignment2 = this.RtlTranslateContent(this.textAlign);
				TextImageRelation textImageRelation = this.RtlTranslateRelation(this.textImageRelation);
				Rectangle rectangle = Rectangle.Inflate(layout.field, -this.textImageInset, -this.textImageInset);
				if (this.OnePixExtraBorder)
				{
					rectangle.Inflate(1, 1);
				}
				if (this.imageSize == Size.Empty || this.text == null || this.text.Length == 0 || textImageRelation == TextImageRelation.Overlay)
				{
					Size textSize = this.GetTextSize(rectangle.Size);
					Size alignThis = this.imageSize;
					if (layout.options.everettButtonCompat && this.imageSize != Size.Empty)
					{
						alignThis = new Size(alignThis.Width + 1, alignThis.Height + 1);
					}
					layout.imageBounds = LayoutUtils.Align(alignThis, rectangle, contentAlignment);
					layout.textBounds = LayoutUtils.Align(textSize, rectangle, contentAlignment2);
				}
				else
				{
					Size proposedSize = LayoutUtils.SubAlignedRegion(rectangle.Size, this.imageSize, textImageRelation);
					Size textSize2 = this.GetTextSize(proposedSize);
					Rectangle rectangle2 = rectangle;
					Size size = LayoutUtils.AddAlignedRegion(textSize2, this.imageSize, textImageRelation);
					rectangle2.Size = LayoutUtils.UnionSizes(rectangle2.Size, size);
					Rectangle bounds = LayoutUtils.Align(size, rectangle2, ContentAlignment.MiddleCenter);
					bool flag = (ButtonBaseAdapter.LayoutOptions.ImageAlignToRelation(contentAlignment) & textImageRelation) != TextImageRelation.Overlay;
					bool flag2 = (ButtonBaseAdapter.LayoutOptions.TextAlignToRelation(contentAlignment2) & textImageRelation) != TextImageRelation.Overlay;
					if (flag)
					{
						LayoutUtils.SplitRegion(rectangle2, this.imageSize, (AnchorStyles)textImageRelation, out layout.imageBounds, out layout.textBounds);
					}
					else if (flag2)
					{
						LayoutUtils.SplitRegion(rectangle2, textSize2, (AnchorStyles)LayoutUtils.GetOppositeTextImageRelation(textImageRelation), out layout.textBounds, out layout.imageBounds);
					}
					else
					{
						LayoutUtils.SplitRegion(bounds, this.imageSize, (AnchorStyles)textImageRelation, out layout.imageBounds, out layout.textBounds);
						LayoutUtils.ExpandRegionsToFillBounds(rectangle2, (AnchorStyles)textImageRelation, ref layout.imageBounds, ref layout.textBounds);
					}
					layout.imageBounds = LayoutUtils.Align(this.imageSize, layout.imageBounds, contentAlignment);
					layout.textBounds = LayoutUtils.Align(textSize2, layout.textBounds, contentAlignment2);
				}
				if (textImageRelation == TextImageRelation.TextBeforeImage || textImageRelation == TextImageRelation.ImageBeforeText)
				{
					int num = Math.Min(layout.textBounds.Bottom, layout.field.Bottom);
					layout.textBounds.Y = Math.Max(Math.Min(layout.textBounds.Y, layout.field.Y + (layout.field.Height - layout.textBounds.Height) / 2), layout.field.Y);
					layout.textBounds.Height = num - layout.textBounds.Y;
				}
				if (textImageRelation == TextImageRelation.TextAboveImage || textImageRelation == TextImageRelation.ImageAboveText)
				{
					int num2 = Math.Min(layout.textBounds.Right, layout.field.Right);
					layout.textBounds.X = Math.Max(Math.Min(layout.textBounds.X, layout.field.X + (layout.field.Width - layout.textBounds.Width) / 2), layout.field.X);
					layout.textBounds.Width = num2 - layout.textBounds.X;
				}
				if (textImageRelation == TextImageRelation.ImageBeforeText && layout.imageBounds.Size.Width != 0)
				{
					layout.imageBounds.Width = Math.Max(0, Math.Min(rectangle.Width - layout.textBounds.Width, layout.imageBounds.Width));
					layout.textBounds.X = layout.imageBounds.X + layout.imageBounds.Width;
				}
				if (textImageRelation == TextImageRelation.ImageAboveText && layout.imageBounds.Size.Height != 0)
				{
					layout.imageBounds.Height = Math.Max(0, Math.Min(rectangle.Height - layout.textBounds.Height, layout.imageBounds.Height));
					layout.textBounds.Y = layout.imageBounds.Y + layout.imageBounds.Height;
				}
				layout.textBounds = Rectangle.Intersect(layout.textBounds, layout.field);
				if (this.hintTextUp)
				{
					layout.textBounds.Y = layout.textBounds.Y - 1;
				}
				if (this.textOffset)
				{
					layout.textBounds.Offset(1, 1);
				}
				if (layout.options.everettButtonCompat)
				{
					layout.imageStart = layout.imageBounds.Location;
					layout.imageBounds = Rectangle.Intersect(layout.imageBounds, layout.field);
				}
				else if (!Application.RenderWithVisualStyles)
				{
					layout.textBounds.X = layout.textBounds.X + 1;
				}
				int num3;
				if (!this.useCompatibleTextRendering)
				{
					num3 = Math.Min(layout.textBounds.Bottom, rectangle.Bottom);
					layout.textBounds.Y = Math.Max(layout.textBounds.Y, rectangle.Y);
				}
				else
				{
					num3 = Math.Min(layout.textBounds.Bottom, layout.field.Bottom);
					layout.textBounds.Y = Math.Max(layout.textBounds.Y, layout.field.Y);
				}
				layout.textBounds.Height = num3 - layout.textBounds.Y;
			}

			// Token: 0x06005837 RID: 22583 RVA: 0x00140480 File Offset: 0x0013F480
			protected virtual Size GetTextSize(Size proposedSize)
			{
				proposedSize = LayoutUtils.FlipSizeIf(this.verticalText, proposedSize);
				Size size = Size.Empty;
				if (this.useCompatibleTextRendering)
				{
					using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
					{
						using (StringFormat stringFormat = this.StringFormat)
						{
							size = Size.Ceiling(graphics.MeasureString(this.text, this.font, new SizeF((float)proposedSize.Width, (float)proposedSize.Height), stringFormat));
						}
						goto IL_95;
					}
				}
				if (!string.IsNullOrEmpty(this.text))
				{
					size = TextRenderer.MeasureText(this.text, this.font, proposedSize, this.TextFormatFlags);
				}
				IL_95:
				return LayoutUtils.FlipSizeIf(this.verticalText, size);
			}

			// Token: 0x040037C7 RID: 14279
			internal Rectangle client;

			// Token: 0x040037C8 RID: 14280
			internal bool growBorderBy1PxWhenDefault;

			// Token: 0x040037C9 RID: 14281
			internal bool isDefault;

			// Token: 0x040037CA RID: 14282
			internal int borderSize;

			// Token: 0x040037CB RID: 14283
			internal int paddingSize;

			// Token: 0x040037CC RID: 14284
			internal bool maxFocus;

			// Token: 0x040037CD RID: 14285
			internal bool focusOddEvenFixup;

			// Token: 0x040037CE RID: 14286
			internal Font font;

			// Token: 0x040037CF RID: 14287
			internal string text;

			// Token: 0x040037D0 RID: 14288
			internal Size imageSize;

			// Token: 0x040037D1 RID: 14289
			internal int checkSize;

			// Token: 0x040037D2 RID: 14290
			internal int checkPaddingSize;

			// Token: 0x040037D3 RID: 14291
			internal ContentAlignment checkAlign;

			// Token: 0x040037D4 RID: 14292
			internal ContentAlignment imageAlign;

			// Token: 0x040037D5 RID: 14293
			internal ContentAlignment textAlign;

			// Token: 0x040037D6 RID: 14294
			internal TextImageRelation textImageRelation;

			// Token: 0x040037D7 RID: 14295
			internal bool hintTextUp;

			// Token: 0x040037D8 RID: 14296
			internal bool textOffset;

			// Token: 0x040037D9 RID: 14297
			internal bool shadowedText;

			// Token: 0x040037DA RID: 14298
			internal bool layoutRTL;

			// Token: 0x040037DB RID: 14299
			internal bool verticalText;

			// Token: 0x040037DC RID: 14300
			internal bool useCompatibleTextRendering;

			// Token: 0x040037DD RID: 14301
			internal bool everettButtonCompat = true;

			// Token: 0x040037DE RID: 14302
			internal TextFormatFlags gdiTextFormatFlags = TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak;

			// Token: 0x040037DF RID: 14303
			internal StringFormatFlags gdipFormatFlags;

			// Token: 0x040037E0 RID: 14304
			internal StringTrimming gdipTrimming;

			// Token: 0x040037E1 RID: 14305
			internal HotkeyPrefix gdipHotkeyPrefix;

			// Token: 0x040037E2 RID: 14306
			internal StringAlignment gdipAlignment;

			// Token: 0x040037E3 RID: 14307
			internal StringAlignment gdipLineAlignment;

			// Token: 0x040037E4 RID: 14308
			private bool disableWordWrapping;

			// Token: 0x040037E5 RID: 14309
			internal int textImageInset = 2;

			// Token: 0x040037E6 RID: 14310
			internal Padding padding;

			// Token: 0x040037E7 RID: 14311
			private static readonly int combineCheck = BitVector32.CreateMask();

			// Token: 0x040037E8 RID: 14312
			private static readonly int combineImageText = BitVector32.CreateMask(ButtonBaseAdapter.LayoutOptions.combineCheck);

			// Token: 0x040037E9 RID: 14313
			private static readonly TextImageRelation[] _imageAlignToRelation = new TextImageRelation[]
			{
				(TextImageRelation)5,
				TextImageRelation.ImageAboveText,
				(TextImageRelation)9,
				TextImageRelation.Overlay,
				TextImageRelation.ImageBeforeText,
				TextImageRelation.Overlay,
				TextImageRelation.TextBeforeImage,
				TextImageRelation.Overlay,
				(TextImageRelation)6,
				TextImageRelation.TextAboveImage,
				(TextImageRelation)10
			};

			// Token: 0x02000688 RID: 1672
			private enum Composition
			{
				// Token: 0x040037EB RID: 14315
				NoneCombined,
				// Token: 0x040037EC RID: 14316
				CheckCombined,
				// Token: 0x040037ED RID: 14317
				TextImageCombined,
				// Token: 0x040037EE RID: 14318
				AllCombined
			}
		}

		// Token: 0x02000689 RID: 1673
		internal class LayoutData
		{
			// Token: 0x0600583A RID: 22586 RVA: 0x001405C5 File Offset: 0x0013F5C5
			internal LayoutData(ButtonBaseAdapter.LayoutOptions options)
			{
				this.options = options;
			}

			// Token: 0x040037EF RID: 14319
			internal Rectangle client;

			// Token: 0x040037F0 RID: 14320
			internal Rectangle face;

			// Token: 0x040037F1 RID: 14321
			internal Rectangle checkArea;

			// Token: 0x040037F2 RID: 14322
			internal Rectangle checkBounds;

			// Token: 0x040037F3 RID: 14323
			internal Rectangle textBounds;

			// Token: 0x040037F4 RID: 14324
			internal Rectangle field;

			// Token: 0x040037F5 RID: 14325
			internal Rectangle focus;

			// Token: 0x040037F6 RID: 14326
			internal Rectangle imageBounds;

			// Token: 0x040037F7 RID: 14327
			internal Point imageStart;

			// Token: 0x040037F8 RID: 14328
			internal ButtonBaseAdapter.LayoutOptions options;
		}
	}
}
