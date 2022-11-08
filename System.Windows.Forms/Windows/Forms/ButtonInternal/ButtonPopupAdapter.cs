using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000740 RID: 1856
	internal class ButtonPopupAdapter : ButtonBaseAdapter
	{
		// Token: 0x060062DD RID: 25309 RVA: 0x001679C6 File Offset: 0x001669C6
		internal ButtonPopupAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060062DE RID: 25310 RVA: 0x001679D0 File Offset: 0x001669D0
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layout = this.PaintPopupLayout(e, state == CheckState.Unchecked, 1).Layout();
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			Brush brush = null;
			if (state == CheckState.Indeterminate)
			{
				brush = ButtonBaseAdapter.CreateDitherBrush(colorData.highlight, colorData.buttonFace);
			}
			try
			{
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
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			base.PaintImage(e, layout);
			base.PaintField(e, layout, colorData, colorData.windowText, true);
			ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.options.highContrast ? colorData.windowText : colorData.buttonShadow, base.Control.IsDefault);
			if (state == CheckState.Unchecked)
			{
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.options.highContrast ? colorData.windowText : colorData.buttonShadow);
				return;
			}
			ButtonBaseAdapter.Draw3DLiteBorder(graphics, clientRectangle, colorData, false);
		}

		// Token: 0x060062DF RID: 25311 RVA: 0x00167AE4 File Offset: 0x00166AE4
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layout = this.PaintPopupLayout(e, state == CheckState.Unchecked, SystemInformation.HighContrast ? 2 : 1).Layout();
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			Brush brush = null;
			if (state == CheckState.Indeterminate)
			{
				brush = ButtonBaseAdapter.CreateDitherBrush(colorData.highlight, colorData.buttonFace);
			}
			try
			{
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
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			base.PaintImage(e, layout);
			base.PaintField(e, layout, colorData, colorData.windowText, true);
			ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.options.highContrast ? colorData.windowText : colorData.buttonShadow, base.Control.IsDefault);
			if (SystemInformation.HighContrast)
			{
				using (Pen pen = new Pen(colorData.windowFrame))
				{
					using (Pen pen2 = new Pen(colorData.highlight))
					{
						using (Pen pen3 = new Pen(colorData.buttonShadow))
						{
							graphics.DrawLine(pen, clientRectangle.Left + 1, clientRectangle.Top + 1, clientRectangle.Right - 2, clientRectangle.Top + 1);
							graphics.DrawLine(pen, clientRectangle.Left + 1, clientRectangle.Top + 1, clientRectangle.Left + 1, clientRectangle.Bottom - 2);
							graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Bottom - 1, clientRectangle.Right, clientRectangle.Bottom - 1);
							graphics.DrawLine(pen, clientRectangle.Right - 1, clientRectangle.Top, clientRectangle.Right - 1, clientRectangle.Bottom);
							graphics.DrawLine(pen2, clientRectangle.Left, clientRectangle.Top, clientRectangle.Right, clientRectangle.Top);
							graphics.DrawLine(pen2, clientRectangle.Left, clientRectangle.Top, clientRectangle.Left, clientRectangle.Bottom);
							graphics.DrawLine(pen3, clientRectangle.Left + 1, clientRectangle.Bottom - 2, clientRectangle.Right - 2, clientRectangle.Bottom - 2);
							graphics.DrawLine(pen3, clientRectangle.Right - 2, clientRectangle.Top + 1, clientRectangle.Right - 2, clientRectangle.Bottom - 2);
						}
					}
				}
				clientRectangle.Inflate(-2, -2);
				return;
			}
			ButtonBaseAdapter.Draw3DLiteBorder(graphics, clientRectangle, colorData, true);
		}

		// Token: 0x060062E0 RID: 25312 RVA: 0x00167DE0 File Offset: 0x00166DE0
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layout = this.PaintPopupLayout(e, false, SystemInformation.HighContrast ? 2 : 1).Layout();
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			base.PaintButtonBackground(e, clientRectangle, null);
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			clientRectangle.Inflate(-1, -1);
			base.PaintImage(e, layout);
			base.PaintField(e, layout, colorData, colorData.windowText, true);
			clientRectangle.Inflate(1, 1);
			ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.options.highContrast ? colorData.windowText : colorData.windowFrame, base.Control.IsDefault);
			ControlPaint.DrawBorder(graphics, clientRectangle, colorData.options.highContrast ? colorData.windowText : colorData.buttonShadow, ButtonBorderStyle.Solid);
		}

		// Token: 0x060062E1 RID: 25313 RVA: 0x00167EC4 File Offset: 0x00166EC4
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			return this.PaintPopupLayout(e, false, 0);
		}

		// Token: 0x060062E2 RID: 25314 RVA: 0x00167EDC File Offset: 0x00166EDC
		internal static ButtonBaseAdapter.LayoutOptions PaintPopupLayout(Graphics g, bool up, int paintedBorder, Rectangle clientRectangle, Padding padding, bool isDefault, Font font, string text, bool enabled, ContentAlignment textAlign, RightToLeft rtl)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = ButtonBaseAdapter.CommonLayout(clientRectangle, padding, isDefault, font, text, enabled, textAlign, rtl);
			layoutOptions.borderSize = paintedBorder;
			layoutOptions.paddingSize = 2 - paintedBorder;
			layoutOptions.hintTextUp = false;
			layoutOptions.textOffset = !up;
			layoutOptions.shadowedText = SystemInformation.HighContrast;
			return layoutOptions;
		}

		// Token: 0x060062E3 RID: 25315 RVA: 0x00167F2C File Offset: 0x00166F2C
		private ButtonBaseAdapter.LayoutOptions PaintPopupLayout(PaintEventArgs e, bool up, int paintedBorder)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.borderSize = paintedBorder;
			layoutOptions.paddingSize = 2 - paintedBorder;
			layoutOptions.hintTextUp = false;
			layoutOptions.textOffset = !up;
			layoutOptions.shadowedText = SystemInformation.HighContrast;
			return layoutOptions;
		}
	}
}
