using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000743 RID: 1859
	internal abstract class CheckBoxBaseAdapter : CheckableControlBaseAdapter
	{
		// Token: 0x060062F3 RID: 25331 RVA: 0x00168478 File Offset: 0x00167478
		internal CheckBoxBaseAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x060062F4 RID: 25332 RVA: 0x00168481 File Offset: 0x00167481
		protected new CheckBox Control
		{
			get
			{
				return (CheckBox)base.Control;
			}
		}

		// Token: 0x060062F5 RID: 25333 RVA: 0x00168490 File Offset: 0x00167490
		protected void DrawCheckFlat(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, Color checkColor, Color checkBackground, Color checkBorder, ButtonBaseAdapter.ColorData colors)
		{
			Rectangle checkBounds = layout.checkBounds;
			if (!layout.options.everettButtonCompat)
			{
				checkBounds.Width--;
				checkBounds.Height--;
			}
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(e.Graphics))
			{
				using (WindowsPen windowsPen = new WindowsPen(windowsGraphics.DeviceContext, checkBorder))
				{
					windowsGraphics.DrawRectangle(windowsPen, checkBounds);
				}
				if (layout.options.everettButtonCompat)
				{
					checkBounds.Width--;
					checkBounds.Height--;
				}
				checkBounds.Inflate(-1, -1);
			}
			if (this.Control.CheckState == CheckState.Indeterminate)
			{
				checkBounds.Width++;
				checkBounds.Height++;
				ButtonBaseAdapter.DrawDitheredFill(e.Graphics, colors.buttonFace, checkBackground, checkBounds);
			}
			else
			{
				using (WindowsGraphics windowsGraphics2 = WindowsGraphics.FromGraphics(e.Graphics))
				{
					using (WindowsBrush windowsBrush = new WindowsSolidBrush(windowsGraphics2.DeviceContext, checkBackground))
					{
						checkBounds.Width++;
						checkBounds.Height++;
						windowsGraphics2.FillRectangle(windowsBrush, checkBounds);
					}
				}
			}
			this.DrawCheckOnly(e, layout, colors, checkColor, checkBackground, true);
		}

		// Token: 0x060062F6 RID: 25334 RVA: 0x00168620 File Offset: 0x00167620
		internal static void DrawCheckBackground(bool controlEnabled, CheckState controlCheckState, Graphics g, Rectangle bounds, Color checkColor, Color checkBackground, bool disabledColors, ButtonBaseAdapter.ColorData colors)
		{
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				WindowsBrush windowsBrush;
				if (!controlEnabled && disabledColors)
				{
					windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, SystemColors.Control);
				}
				else if (controlCheckState == CheckState.Indeterminate && checkBackground == SystemColors.Window && disabledColors)
				{
					Color color = SystemInformation.HighContrast ? SystemColors.ControlDark : SystemColors.Control;
					byte red = (color.R + SystemColors.Window.R) / 2;
					byte green = (color.G + SystemColors.Window.G) / 2;
					byte blue = (color.B + SystemColors.Window.B) / 2;
					windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, Color.FromArgb((int)red, (int)green, (int)blue));
				}
				else
				{
					windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, checkBackground);
				}
				try
				{
					windowsGraphics.FillRectangle(windowsBrush, bounds);
				}
				finally
				{
					if (windowsBrush != null)
					{
						windowsBrush.Dispose();
					}
				}
			}
		}

		// Token: 0x060062F7 RID: 25335 RVA: 0x00168734 File Offset: 0x00167734
		protected void DrawCheckBackground(PaintEventArgs e, Rectangle bounds, Color checkColor, Color checkBackground, bool disabledColors, ButtonBaseAdapter.ColorData colors)
		{
			if (this.Control.CheckState == CheckState.Indeterminate)
			{
				ButtonBaseAdapter.DrawDitheredFill(e.Graphics, colors.buttonFace, checkBackground, bounds);
				return;
			}
			CheckBoxBaseAdapter.DrawCheckBackground(this.Control.Enabled, this.Control.CheckState, e.Graphics, bounds, checkColor, checkBackground, disabledColors, colors);
		}

		// Token: 0x060062F8 RID: 25336 RVA: 0x00168790 File Offset: 0x00167790
		protected void DrawCheckOnly(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, ButtonBaseAdapter.ColorData colors, Color checkColor, Color checkBackground, bool disabledColors)
		{
			CheckBoxBaseAdapter.DrawCheckOnly(11, this.Control.Checked, this.Control.Enabled, this.Control.CheckState, e.Graphics, layout, colors, checkColor, checkBackground, disabledColors);
		}

		// Token: 0x060062F9 RID: 25337 RVA: 0x001687D4 File Offset: 0x001677D4
		internal static void DrawCheckOnly(int checkSize, bool controlChecked, bool controlEnabled, CheckState controlCheckState, Graphics g, ButtonBaseAdapter.LayoutData layout, ButtonBaseAdapter.ColorData colors, Color checkColor, Color checkBackground, bool disabledColors)
		{
			if (controlChecked)
			{
				if (!controlEnabled && disabledColors)
				{
					checkColor = colors.buttonShadow;
				}
				else if (controlCheckState == CheckState.Indeterminate && disabledColors)
				{
					checkColor = (SystemInformation.HighContrast ? colors.highlight : colors.buttonShadow);
				}
				Rectangle checkBounds = layout.checkBounds;
				if (checkBounds.Width == checkSize)
				{
					checkBounds.Width++;
					checkBounds.Height++;
				}
				checkBounds.Width++;
				checkBounds.Height++;
				Bitmap checkBoxImage;
				if (controlCheckState == CheckState.Checked)
				{
					checkBoxImage = CheckBoxBaseAdapter.GetCheckBoxImage(checkColor, checkBounds, ref CheckBoxBaseAdapter.checkImageCheckedBackColor, ref CheckBoxBaseAdapter.checkImageChecked);
				}
				else
				{
					checkBoxImage = CheckBoxBaseAdapter.GetCheckBoxImage(checkColor, checkBounds, ref CheckBoxBaseAdapter.checkImageIndeterminateBackColor, ref CheckBoxBaseAdapter.checkImageIndeterminate);
				}
				if (layout.options.everettButtonCompat)
				{
					checkBounds.Y--;
				}
				else
				{
					checkBounds.Y -= 2;
				}
				ControlPaint.DrawImageColorized(g, checkBoxImage, checkBounds, checkColor);
			}
		}

		// Token: 0x060062FA RID: 25338 RVA: 0x001688D0 File Offset: 0x001678D0
		internal static Rectangle DrawPopupBorder(Graphics g, Rectangle r, ButtonBaseAdapter.ColorData colors)
		{
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
			{
				using (WindowsPen windowsPen = new WindowsPen(windowsGraphics.DeviceContext, colors.highlight))
				{
					using (WindowsPen windowsPen2 = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonShadow))
					{
						using (WindowsPen windowsPen3 = new WindowsPen(windowsGraphics.DeviceContext, colors.buttonFace))
						{
							windowsGraphics.DrawLine(windowsPen, r.Right - 1, r.Top, r.Right - 1, r.Bottom);
							windowsGraphics.DrawLine(windowsPen, r.Left, r.Bottom - 1, r.Right, r.Bottom - 1);
							windowsGraphics.DrawLine(windowsPen2, r.Left, r.Top, r.Left, r.Bottom);
							windowsGraphics.DrawLine(windowsPen2, r.Left, r.Top, r.Right - 1, r.Top);
							windowsGraphics.DrawLine(windowsPen3, r.Right - 2, r.Top + 1, r.Right - 2, r.Bottom - 1);
							windowsGraphics.DrawLine(windowsPen3, r.Left + 1, r.Bottom - 2, r.Right - 1, r.Bottom - 2);
						}
					}
				}
			}
			r.Inflate(-1, -1);
			return r;
		}

		// Token: 0x060062FB RID: 25339 RVA: 0x00168AA4 File Offset: 0x00167AA4
		protected ButtonState GetState()
		{
			ButtonState buttonState = ButtonState.Normal;
			if (this.Control.CheckState == CheckState.Unchecked)
			{
				buttonState = buttonState;
			}
			else
			{
				buttonState |= ButtonState.Checked;
			}
			if (!this.Control.Enabled)
			{
				buttonState |= ButtonState.Inactive;
			}
			if (this.Control.MouseIsDown)
			{
				buttonState |= ButtonState.Pushed;
			}
			return buttonState;
		}

		// Token: 0x060062FC RID: 25340 RVA: 0x00168AF8 File Offset: 0x00167AF8
		protected void DrawCheckBox(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout)
		{
			Graphics graphics = e.Graphics;
			ButtonState state = this.GetState();
			if (this.Control.CheckState == CheckState.Indeterminate)
			{
				if (Application.RenderWithVisualStyles)
				{
					CheckBoxRenderer.DrawCheckBox(graphics, new Point(layout.checkBounds.Left, layout.checkBounds.Top), CheckBoxRenderer.ConvertFromButtonState(state, true, this.Control.MouseIsOver));
					return;
				}
				ControlPaint.DrawMixedCheckBox(graphics, layout.checkBounds, state);
				return;
			}
			else
			{
				if (Application.RenderWithVisualStyles)
				{
					CheckBoxRenderer.DrawCheckBox(graphics, new Point(layout.checkBounds.Left, layout.checkBounds.Top), CheckBoxRenderer.ConvertFromButtonState(state, false, this.Control.MouseIsOver));
					return;
				}
				ControlPaint.DrawCheckBox(graphics, layout.checkBounds, state);
				return;
			}
		}

		// Token: 0x060062FD RID: 25341 RVA: 0x00168BB4 File Offset: 0x00167BB4
		private static Bitmap GetCheckBoxImage(Color checkColor, Rectangle fullSize, ref Color cacheCheckColor, ref Bitmap cacheCheckImage)
		{
			if (cacheCheckImage != null && cacheCheckColor.Equals(checkColor) && cacheCheckImage.Width == fullSize.Width && cacheCheckImage.Height == fullSize.Height)
			{
				return cacheCheckImage;
			}
			if (cacheCheckImage != null)
			{
				cacheCheckImage.Dispose();
				cacheCheckImage = null;
			}
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(0, 0, fullSize.Width, fullSize.Height);
			Bitmap bitmap = new Bitmap(fullSize.Width, fullSize.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.Transparent);
			IntPtr hdc = graphics.GetHdc();
			try
			{
				SafeNativeMethods.DrawFrameControl(new HandleRef(graphics, hdc), ref rect, 2, 1);
			}
			finally
			{
				graphics.ReleaseHdcInternal(hdc);
				graphics.Dispose();
			}
			bitmap.MakeTransparent();
			cacheCheckImage = bitmap;
			cacheCheckColor = checkColor;
			return cacheCheckImage;
		}

		// Token: 0x060062FE RID: 25342 RVA: 0x00168C90 File Offset: 0x00167C90
		internal override ButtonBaseAdapter.LayoutOptions CommonLayout()
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = base.CommonLayout();
			layoutOptions.checkAlign = this.Control.CheckAlign;
			layoutOptions.textOffset = false;
			layoutOptions.shadowedText = !this.Control.Enabled;
			layoutOptions.layoutRTL = (RightToLeft.Yes == this.Control.RightToLeft);
			return layoutOptions;
		}

		// Token: 0x04003B50 RID: 15184
		protected const int flatCheckSize = 11;

		// Token: 0x04003B51 RID: 15185
		[ThreadStatic]
		private static Bitmap checkImageChecked = null;

		// Token: 0x04003B52 RID: 15186
		[ThreadStatic]
		private static Color checkImageCheckedBackColor = Color.Empty;

		// Token: 0x04003B53 RID: 15187
		[ThreadStatic]
		private static Bitmap checkImageIndeterminate = null;

		// Token: 0x04003B54 RID: 15188
		[ThreadStatic]
		private static Color checkImageIndeterminateBackColor = Color.Empty;
	}
}
