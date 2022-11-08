using System;
using System.Drawing;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000747 RID: 1863
	internal abstract class RadioButtonBaseAdapter : CheckableControlBaseAdapter
	{
		// Token: 0x06006318 RID: 25368 RVA: 0x001695C4 File Offset: 0x001685C4
		internal RadioButtonBaseAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x06006319 RID: 25369 RVA: 0x001695CD File Offset: 0x001685CD
		protected new RadioButton Control
		{
			get
			{
				return (RadioButton)base.Control;
			}
		}

		// Token: 0x0600631A RID: 25370 RVA: 0x001695DA File Offset: 0x001685DA
		protected void DrawCheckFlat(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, Color checkColor, Color checkBackground, Color checkBorder)
		{
			this.DrawCheckBackgroundFlat(e, layout.checkBounds, checkBorder, checkBackground, true);
			this.DrawCheckOnly(e, layout, checkColor, checkBackground, true);
		}

		// Token: 0x0600631B RID: 25371 RVA: 0x001695FC File Offset: 0x001685FC
		protected void DrawCheckBackground3DLite(PaintEventArgs e, Rectangle bounds, Color checkColor, Color checkBackground, ButtonBaseAdapter.ColorData colors, bool disabledColors)
		{
			Graphics graphics = e.Graphics;
			Color color = checkBackground;
			if (!this.Control.Enabled && disabledColors)
			{
				color = SystemColors.Control;
			}
			using (Brush brush = new SolidBrush(color))
			{
				using (Pen pen = new Pen(colors.buttonShadow))
				{
					using (Pen pen2 = new Pen(colors.buttonFace))
					{
						using (Pen pen3 = new Pen(colors.highlight))
						{
							bounds.Width--;
							bounds.Height--;
							graphics.DrawPie(pen, bounds, 136f, 88f);
							graphics.DrawPie(pen, bounds, 226f, 88f);
							graphics.DrawPie(pen3, bounds, 316f, 88f);
							graphics.DrawPie(pen3, bounds, 46f, 88f);
							bounds.Inflate(-1, -1);
							graphics.FillEllipse(brush, bounds);
							graphics.DrawEllipse(pen2, bounds);
						}
					}
				}
			}
		}

		// Token: 0x0600631C RID: 25372 RVA: 0x00169744 File Offset: 0x00168744
		protected void DrawCheckBackgroundFlat(PaintEventArgs e, Rectangle bounds, Color borderColor, Color checkBackground, bool disabledColors)
		{
			Color color = checkBackground;
			Color color2 = borderColor;
			if (!this.Control.Enabled && disabledColors)
			{
				color2 = ControlPaint.ContrastControlDark;
				color = SystemColors.Control;
			}
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(e.Graphics))
			{
				using (WindowsPen windowsPen = new WindowsPen(windowsGraphics.DeviceContext, color2))
				{
					using (WindowsBrush windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, color))
					{
						RadioButtonBaseAdapter.DrawAndFillEllipse(windowsGraphics, windowsPen, windowsBrush, bounds);
					}
				}
			}
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x001697F4 File Offset: 0x001687F4
		private static void DrawAndFillEllipse(WindowsGraphics wg, WindowsPen borderPen, WindowsBrush fieldBrush, Rectangle bounds)
		{
			if (wg == null)
			{
				return;
			}
			wg.FillRectangle(fieldBrush, new Rectangle(bounds.X + 2, bounds.Y + 2, 8, 8));
			wg.FillRectangle(fieldBrush, new Rectangle(bounds.X + 4, bounds.Y + 1, 4, 10));
			wg.FillRectangle(fieldBrush, new Rectangle(bounds.X + 1, bounds.Y + 4, 10, 4));
			wg.DrawLine(borderPen, new Point(bounds.X + 4, bounds.Y), new Point(bounds.X + 8, bounds.Y));
			wg.DrawLine(borderPen, new Point(bounds.X + 4, bounds.Y + 11), new Point(bounds.X + 8, bounds.Y + 11));
			wg.DrawLine(borderPen, new Point(bounds.X + 2, bounds.Y + 1), new Point(bounds.X + 4, bounds.Y + 1));
			wg.DrawLine(borderPen, new Point(bounds.X + 8, bounds.Y + 1), new Point(bounds.X + 10, bounds.Y + 1));
			wg.DrawLine(borderPen, new Point(bounds.X + 2, bounds.Y + 10), new Point(bounds.X + 4, bounds.Y + 10));
			wg.DrawLine(borderPen, new Point(bounds.X + 8, bounds.Y + 10), new Point(bounds.X + 10, bounds.Y + 10));
			wg.DrawLine(borderPen, new Point(bounds.X, bounds.Y + 4), new Point(bounds.X, bounds.Y + 8));
			wg.DrawLine(borderPen, new Point(bounds.X + 11, bounds.Y + 4), new Point(bounds.X + 11, bounds.Y + 8));
			wg.DrawLine(borderPen, new Point(bounds.X + 1, bounds.Y + 2), new Point(bounds.X + 1, bounds.Y + 4));
			wg.DrawLine(borderPen, new Point(bounds.X + 1, bounds.Y + 8), new Point(bounds.X + 1, bounds.Y + 10));
			wg.DrawLine(borderPen, new Point(bounds.X + 10, bounds.Y + 2), new Point(bounds.X + 10, bounds.Y + 4));
			wg.DrawLine(borderPen, new Point(bounds.X + 10, bounds.Y + 8), new Point(bounds.X + 10, bounds.Y + 10));
		}

		// Token: 0x0600631E RID: 25374 RVA: 0x00169AEC File Offset: 0x00168AEC
		protected void DrawCheckOnly(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout, Color checkColor, Color checkBackground, bool disabledColors)
		{
			if (this.Control.Checked)
			{
				if (!this.Control.Enabled && disabledColors)
				{
					checkColor = SystemColors.ControlDark;
				}
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(e.Graphics))
				{
					using (WindowsBrush windowsBrush = new WindowsSolidBrush(windowsGraphics.DeviceContext, checkColor))
					{
						int num = 5;
						Rectangle rect = new Rectangle(layout.checkBounds.X + num, layout.checkBounds.Y + num - 1, 2, 4);
						windowsGraphics.FillRectangle(windowsBrush, rect);
						Rectangle rect2 = new Rectangle(layout.checkBounds.X + num - 1, layout.checkBounds.Y + num, 4, 2);
						windowsGraphics.FillRectangle(windowsBrush, rect2);
					}
				}
			}
		}

		// Token: 0x0600631F RID: 25375 RVA: 0x00169BCC File Offset: 0x00168BCC
		protected ButtonState GetState()
		{
			ButtonState buttonState = ButtonState.Normal;
			if (this.Control.Checked)
			{
				buttonState |= ButtonState.Checked;
			}
			else
			{
				buttonState = buttonState;
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

		// Token: 0x06006320 RID: 25376 RVA: 0x00169C20 File Offset: 0x00168C20
		protected void DrawCheckBox(PaintEventArgs e, ButtonBaseAdapter.LayoutData layout)
		{
			Graphics graphics = e.Graphics;
			Rectangle checkBounds = layout.checkBounds;
			if (!Application.RenderWithVisualStyles)
			{
				checkBounds.X--;
			}
			ButtonState state = this.GetState();
			if (Application.RenderWithVisualStyles)
			{
				RadioButtonRenderer.DrawRadioButton(graphics, new Point(checkBounds.Left, checkBounds.Top), RadioButtonRenderer.ConvertFromButtonState(state, this.Control.MouseIsOver));
				return;
			}
			ControlPaint.DrawRadioButton(graphics, checkBounds, state);
		}

		// Token: 0x06006321 RID: 25377 RVA: 0x00169C94 File Offset: 0x00168C94
		internal override ButtonBaseAdapter.LayoutOptions CommonLayout()
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = base.CommonLayout();
			layoutOptions.checkAlign = this.Control.CheckAlign;
			return layoutOptions;
		}
	}
}
