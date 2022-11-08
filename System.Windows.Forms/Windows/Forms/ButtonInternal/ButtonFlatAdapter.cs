using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x0200073F RID: 1855
	internal class ButtonFlatAdapter : ButtonBaseAdapter
	{
		// Token: 0x060062D5 RID: 25301 RVA: 0x0016703D File Offset: 0x0016603D
		internal ButtonFlatAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x060062D6 RID: 25302 RVA: 0x00167048 File Offset: 0x00166048
		private void PaintBackground(PaintEventArgs e, Rectangle r, Color backColor)
		{
			Rectangle rectangle = r;
			rectangle.Inflate(-base.Control.FlatAppearance.BorderSize, -base.Control.FlatAppearance.BorderSize);
			base.Control.PaintBackground(e, rectangle, backColor, rectangle.Location);
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x00167098 File Offset: 0x00166098
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			bool flag = base.Control.FlatAppearance.BorderSize != 1 || !base.Control.FlatAppearance.BorderColor.IsEmpty;
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintFlatLayout(e, !base.Control.FlatAppearance.CheckedBackColor.IsEmpty || (SystemInformation.HighContrast ? (state != CheckState.Indeterminate) : (state == CheckState.Unchecked)), !flag && SystemInformation.HighContrast && state == CheckState.Checked, base.Control.FlatAppearance.BorderSize).Layout();
			if (!base.Control.FlatAppearance.BorderColor.IsEmpty)
			{
				colorData.windowFrame = base.Control.FlatAppearance.BorderColor;
			}
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			Color backColor = base.Control.BackColor;
			if (!base.Control.FlatAppearance.CheckedBackColor.IsEmpty)
			{
				switch (state)
				{
				case CheckState.Checked:
					backColor = base.Control.FlatAppearance.CheckedBackColor;
					break;
				case CheckState.Indeterminate:
					backColor = ButtonBaseAdapter.MixedColor(base.Control.FlatAppearance.CheckedBackColor, colorData.buttonFace);
					break;
				}
			}
			else
			{
				switch (state)
				{
				case CheckState.Checked:
					backColor = colorData.highlight;
					break;
				case CheckState.Indeterminate:
					backColor = ButtonBaseAdapter.MixedColor(colorData.highlight, colorData.buttonFace);
					break;
				}
			}
			this.PaintBackground(e, clientRectangle, backColor);
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			base.PaintImage(e, layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, false);
			if (base.Control.Focused && base.Control.ShowFocusCues)
			{
				ButtonBaseAdapter.DrawFlatFocus(graphics, layoutData.focus, colorData.options.highContrast ? colorData.windowText : colorData.constrastButtonShadow);
			}
			if (!base.Control.IsDefault || !base.Control.Focused || base.Control.FlatAppearance.BorderSize != 0)
			{
				ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.windowFrame, base.Control.IsDefault);
			}
			if (flag)
			{
				if (base.Control.FlatAppearance.BorderSize != 1)
				{
					ButtonBaseAdapter.DrawFlatBorderWithSize(graphics, clientRectangle, colorData.windowFrame, base.Control.FlatAppearance.BorderSize);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
			else
			{
				if (state == CheckState.Checked && SystemInformation.HighContrast)
				{
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.buttonShadow);
					return;
				}
				if (state == CheckState.Indeterminate)
				{
					ButtonBaseAdapter.Draw3DLiteBorder(graphics, clientRectangle, colorData, false);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
		}

		// Token: 0x060062D8 RID: 25304 RVA: 0x0016737C File Offset: 0x0016637C
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			bool flag = base.Control.FlatAppearance.BorderSize != 1 || !base.Control.FlatAppearance.BorderColor.IsEmpty;
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintFlatLayout(e, !base.Control.FlatAppearance.CheckedBackColor.IsEmpty || (SystemInformation.HighContrast ? (state != CheckState.Indeterminate) : (state == CheckState.Unchecked)), !flag && SystemInformation.HighContrast && state == CheckState.Checked, base.Control.FlatAppearance.BorderSize).Layout();
			if (!base.Control.FlatAppearance.BorderColor.IsEmpty)
			{
				colorData.windowFrame = base.Control.FlatAppearance.BorderColor;
			}
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			Color backColor = base.Control.BackColor;
			if (!base.Control.FlatAppearance.MouseDownBackColor.IsEmpty)
			{
				backColor = base.Control.FlatAppearance.MouseDownBackColor;
			}
			else
			{
				switch (state)
				{
				case CheckState.Unchecked:
				case CheckState.Checked:
					backColor = (colorData.options.highContrast ? colorData.buttonShadow : colorData.lowHighlight);
					break;
				case CheckState.Indeterminate:
					backColor = ButtonBaseAdapter.MixedColor(colorData.options.highContrast ? colorData.buttonShadow : colorData.lowHighlight, colorData.buttonFace);
					break;
				}
			}
			this.PaintBackground(e, clientRectangle, backColor);
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			base.PaintImage(e, layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, false);
			if (base.Control.Focused && base.Control.ShowFocusCues)
			{
				ButtonBaseAdapter.DrawFlatFocus(graphics, layoutData.focus, colorData.options.highContrast ? colorData.windowText : colorData.constrastButtonShadow);
			}
			if (!base.Control.IsDefault || !base.Control.Focused || base.Control.FlatAppearance.BorderSize != 0)
			{
				ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.windowFrame, base.Control.IsDefault);
			}
			if (flag)
			{
				if (base.Control.FlatAppearance.BorderSize != 1)
				{
					ButtonBaseAdapter.DrawFlatBorderWithSize(graphics, clientRectangle, colorData.windowFrame, base.Control.FlatAppearance.BorderSize);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
			else
			{
				if (state == CheckState.Checked && SystemInformation.HighContrast)
				{
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.buttonShadow);
					return;
				}
				if (state == CheckState.Indeterminate)
				{
					ButtonBaseAdapter.Draw3DLiteBorder(graphics, clientRectangle, colorData, false);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
		}

		// Token: 0x060062D9 RID: 25305 RVA: 0x00167658 File Offset: 0x00166658
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (SystemInformation.HighContrast)
			{
				this.PaintUp(e, state);
				return;
			}
			bool flag = base.Control.FlatAppearance.BorderSize != 1 || !base.Control.FlatAppearance.BorderColor.IsEmpty;
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintFlatLayout(e, !base.Control.FlatAppearance.CheckedBackColor.IsEmpty || state == CheckState.Unchecked, false, base.Control.FlatAppearance.BorderSize).Layout();
			if (!base.Control.FlatAppearance.BorderColor.IsEmpty)
			{
				colorData.windowFrame = base.Control.FlatAppearance.BorderColor;
			}
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			Color backColor = base.Control.BackColor;
			if (!base.Control.FlatAppearance.MouseOverBackColor.IsEmpty)
			{
				backColor = base.Control.FlatAppearance.MouseOverBackColor;
			}
			else if (!base.Control.FlatAppearance.CheckedBackColor.IsEmpty)
			{
				if (state == CheckState.Checked || state == CheckState.Indeterminate)
				{
					backColor = ButtonBaseAdapter.MixedColor(base.Control.FlatAppearance.CheckedBackColor, colorData.lowButtonFace);
				}
				else
				{
					backColor = colorData.lowButtonFace;
				}
			}
			else if (state == CheckState.Indeterminate)
			{
				backColor = ButtonBaseAdapter.MixedColor(colorData.buttonFace, colorData.lowButtonFace);
			}
			else
			{
				backColor = colorData.lowButtonFace;
			}
			this.PaintBackground(e, clientRectangle, backColor);
			if (base.Control.IsDefault)
			{
				clientRectangle.Inflate(-1, -1);
			}
			base.PaintImage(e, layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, false);
			if (base.Control.Focused && base.Control.ShowFocusCues)
			{
				ButtonBaseAdapter.DrawFlatFocus(graphics, layoutData.focus, colorData.constrastButtonShadow);
			}
			if (!base.Control.IsDefault || !base.Control.Focused || base.Control.FlatAppearance.BorderSize != 0)
			{
				ButtonBaseAdapter.DrawDefaultBorder(graphics, clientRectangle, colorData.windowFrame, base.Control.IsDefault);
			}
			if (flag)
			{
				if (base.Control.FlatAppearance.BorderSize != 1)
				{
					ButtonBaseAdapter.DrawFlatBorderWithSize(graphics, clientRectangle, colorData.windowFrame, base.Control.FlatAppearance.BorderSize);
					return;
				}
				ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
				return;
			}
			else
			{
				if (state == CheckState.Unchecked)
				{
					ButtonBaseAdapter.DrawFlatBorder(graphics, clientRectangle, colorData.windowFrame);
					return;
				}
				ButtonBaseAdapter.Draw3DLiteBorder(graphics, clientRectangle, colorData, false);
				return;
			}
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x001678F4 File Offset: 0x001668F4
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			return this.PaintFlatLayout(e, false, true, base.Control.FlatAppearance.BorderSize);
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x0016791C File Offset: 0x0016691C
		internal static ButtonBaseAdapter.LayoutOptions PaintFlatLayout(Graphics g, bool up, bool check, int borderSize, Rectangle clientRectangle, Padding padding, bool isDefault, Font font, string text, bool enabled, ContentAlignment textAlign, RightToLeft rtl)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = ButtonBaseAdapter.CommonLayout(clientRectangle, padding, isDefault, font, text, enabled, textAlign, rtl);
			layoutOptions.borderSize = borderSize + (check ? 1 : 0);
			layoutOptions.paddingSize = (check ? 1 : 2);
			layoutOptions.focusOddEvenFixup = false;
			layoutOptions.textOffset = !up;
			layoutOptions.shadowedText = SystemInformation.HighContrast;
			return layoutOptions;
		}

		// Token: 0x060062DC RID: 25308 RVA: 0x00167978 File Offset: 0x00166978
		private ButtonBaseAdapter.LayoutOptions PaintFlatLayout(PaintEventArgs e, bool up, bool check, int borderSize)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.borderSize = borderSize + (check ? 1 : 0);
			layoutOptions.paddingSize = (check ? 1 : 2);
			layoutOptions.focusOddEvenFixup = false;
			layoutOptions.textOffset = !up;
			layoutOptions.shadowedText = SystemInformation.HighContrast;
			return layoutOptions;
		}

		// Token: 0x04003B4C RID: 15180
		private const int BORDERSIZE = 1;
	}
}
