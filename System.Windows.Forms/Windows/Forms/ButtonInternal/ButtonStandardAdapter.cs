using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000741 RID: 1857
	internal class ButtonStandardAdapter : ButtonBaseAdapter
	{
		// Token: 0x060062E4 RID: 25316 RVA: 0x00167F6D File Offset: 0x00166F6D
		internal ButtonStandardAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060062E5 RID: 25317 RVA: 0x00167F78 File Offset: 0x00166F78
		private PushButtonState DetermineState(bool up)
		{
			PushButtonState result = PushButtonState.Normal;
			if (!up)
			{
				result = PushButtonState.Pressed;
			}
			else if (base.Control.MouseIsOver)
			{
				result = PushButtonState.Hot;
			}
			else if (!base.Control.Enabled)
			{
				result = PushButtonState.Disabled;
			}
			else if (base.Control.Focused || base.Control.IsDefault)
			{
				result = PushButtonState.Default;
			}
			return result;
		}

		// Token: 0x060062E6 RID: 25318 RVA: 0x00167FCD File Offset: 0x00166FCD
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			this.PaintWorker(e, true, state);
		}

		// Token: 0x060062E7 RID: 25319 RVA: 0x00167FD8 File Offset: 0x00166FD8
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			this.PaintWorker(e, false, state);
		}

		// Token: 0x060062E8 RID: 25320 RVA: 0x00167FE3 File Offset: 0x00166FE3
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			this.PaintUp(e, state);
		}

		// Token: 0x060062E9 RID: 25321 RVA: 0x00167FF0 File Offset: 0x00166FF0
		private void PaintThemedButtonBackground(PaintEventArgs e, Rectangle bounds, bool up)
		{
			PushButtonState state = this.DetermineState(up);
			if (ButtonRenderer.IsBackgroundPartiallyTransparent(state))
			{
				ButtonRenderer.DrawParentBackground(e.Graphics, bounds, base.Control);
			}
			ButtonRenderer.DrawButton(e.Graphics, base.Control.ClientRectangle, false, state);
			bounds.Inflate(-ButtonBaseAdapter.buttonBorderSize, -ButtonBaseAdapter.buttonBorderSize);
			if (!base.Control.UseVisualStyleBackColor)
			{
				bool flag = false;
				Color color = base.Control.BackColor;
				if (color.A == 255 && e.HDC != IntPtr.Zero && DisplayInformation.BitsPerPixel > 8)
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
					SafeNativeMethods.FillRect(new HandleRef(e, e.HDC), ref rect, new HandleRef(this, base.Control.BackColorBrush));
					flag = true;
				}
				if (!flag && color.A > 0)
				{
					if (color.A == 255)
					{
						color = e.Graphics.GetNearestColor(color);
					}
					using (Brush brush = new SolidBrush(color))
					{
						e.Graphics.FillRectangle(brush, bounds);
					}
				}
			}
			if (base.Control.BackgroundImage != null && !DisplayInformation.HighContrast)
			{
				ControlPaint.DrawBackgroundImage(e.Graphics, base.Control.BackgroundImage, Color.Transparent, base.Control.BackgroundImageLayout, base.Control.ClientRectangle, bounds, base.Control.DisplayRectangle.Location, base.Control.RightToLeft);
			}
		}

		// Token: 0x060062EA RID: 25322 RVA: 0x001681A0 File Offset: 0x001671A0
		private void PaintWorker(PaintEventArgs e, bool up, CheckState state)
		{
			up = (up && state == CheckState.Unchecked);
			ButtonBaseAdapter.ColorData colorData = base.PaintRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData;
			if (Application.RenderWithVisualStyles)
			{
				layoutData = this.PaintLayout(e, true).Layout();
			}
			else
			{
				layoutData = this.PaintLayout(e, up).Layout();
			}
			Graphics graphics = e.Graphics;
			ButtonBase control = base.Control;
			if (Application.RenderWithVisualStyles)
			{
				this.PaintThemedButtonBackground(e, base.Control.ClientRectangle, up);
			}
			else
			{
				Brush brush = null;
				if (state == CheckState.Indeterminate)
				{
					brush = ButtonBaseAdapter.CreateDitherBrush(colorData.highlight, colorData.buttonFace);
				}
				try
				{
					Rectangle clientRectangle = base.Control.ClientRectangle;
					if (up)
					{
						clientRectangle.Inflate(-2, -2);
					}
					else
					{
						clientRectangle.Inflate(-1, -1);
					}
					base.PaintButtonBackground(e, clientRectangle, brush);
				}
				finally
				{
					if (brush != null)
					{
						brush.Dispose();
						brush = null;
					}
				}
			}
			base.PaintImage(e, layoutData);
			if (Application.RenderWithVisualStyles)
			{
				layoutData.focus.Inflate(1, 1);
			}
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
			if (!Application.RenderWithVisualStyles)
			{
				Rectangle clientRectangle2 = base.Control.ClientRectangle;
				if (base.Control.IsDefault)
				{
					clientRectangle2.Inflate(-1, -1);
				}
				ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle2, colorData.windowFrame, base.Control.IsDefault);
				if (up)
				{
					base.Draw3DBorder(graphics, clientRectangle2, colorData, up);
					return;
				}
				ControlPaint.DrawBorder(graphics, clientRectangle2, colorData.buttonShadow, ButtonBorderStyle.Solid);
			}
		}

		// Token: 0x060062EB RID: 25323 RVA: 0x00168310 File Offset: 0x00167310
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			return this.PaintLayout(e, false);
		}

		// Token: 0x060062EC RID: 25324 RVA: 0x00168328 File Offset: 0x00167328
		private ButtonBaseAdapter.LayoutOptions PaintLayout(PaintEventArgs e, bool up)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.textOffset = !up;
			layoutOptions.everettButtonCompat = !Application.RenderWithVisualStyles;
			return layoutOptions;
		}

		// Token: 0x04003B4D RID: 15181
		private const int borderWidth = 2;
	}
}
