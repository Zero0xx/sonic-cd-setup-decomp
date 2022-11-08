using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000745 RID: 1861
	internal class CheckBoxPopupAdapter : CheckBoxBaseAdapter
	{
		// Token: 0x06006308 RID: 25352 RVA: 0x00168F54 File Offset: 0x00167F54
		internal CheckBoxPopupAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x06006309 RID: 25353 RVA: 0x00168F60 File Offset: 0x00167F60
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintUp(e, base.Control.CheckState);
				return;
			}
			Graphics graphics = e.Graphics;
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintPopupLayout(e, false).Layout();
			Region clip = e.Graphics.Clip;
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground(e, layoutData.checkBounds, colorData.windowText, colorData.options.highContrast ? colorData.buttonFace : colorData.highlight, true, colorData);
			ButtonBaseAdapter.DrawFlatBorder(e.Graphics, layoutData.checkBounds, colorData.buttonShadow);
			base.DrawCheckOnly(e, layoutData, colorData, colorData.windowText, colorData.highlight, true);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x0600630A RID: 25354 RVA: 0x00169058 File Offset: 0x00168058
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			Graphics graphics = e.Graphics;
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintOver(e, base.Control.CheckState);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintPopupLayout(e, true).Layout();
			Region clip = e.Graphics.Clip;
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground(e, layoutData.checkBounds, colorData.windowText, colorData.options.highContrast ? colorData.buttonFace : colorData.highlight, true, colorData);
			CheckBoxBaseAdapter.DrawPopupBorder(graphics, layoutData.checkBounds, colorData);
			base.DrawCheckOnly(e, layoutData, colorData, colorData.windowText, colorData.highlight, true);
			e.Graphics.Clip = clip;
			e.Graphics.ExcludeClip(layoutData.checkArea);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x0600630B RID: 25355 RVA: 0x00169164 File Offset: 0x00168164
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintDown(e, base.Control.CheckState);
				return;
			}
			Graphics graphics = e.Graphics;
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.PaintPopupLayout(e, true).Layout();
			Region clip = e.Graphics.Clip;
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground(e, layoutData.checkBounds, colorData.windowText, colorData.buttonFace, true, colorData);
			CheckBoxBaseAdapter.DrawPopupBorder(graphics, layoutData.checkBounds, colorData);
			base.DrawCheckOnly(e, layoutData, colorData, colorData.windowText, colorData.buttonFace, true);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x0600630C RID: 25356 RVA: 0x0016923B File Offset: 0x0016823B
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonPopupAdapter(base.Control);
		}

		// Token: 0x0600630D RID: 25357 RVA: 0x00169248 File Offset: 0x00168248
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			return this.PaintPopupLayout(e, true);
		}

		// Token: 0x0600630E RID: 25358 RVA: 0x00169260 File Offset: 0x00168260
		internal static ButtonBaseAdapter.LayoutOptions PaintPopupLayout(Graphics g, bool show3D, int checkSize, Rectangle clientRectangle, Padding padding, bool isDefault, Font font, string text, bool enabled, ContentAlignment textAlign, RightToLeft rtl)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = ButtonBaseAdapter.CommonLayout(clientRectangle, padding, isDefault, font, text, enabled, textAlign, rtl);
			layoutOptions.shadowedText = false;
			if (show3D)
			{
				layoutOptions.checkSize = checkSize + 1;
			}
			else
			{
				layoutOptions.checkSize = checkSize;
				layoutOptions.checkPaddingSize = 1;
			}
			return layoutOptions;
		}

		// Token: 0x0600630F RID: 25359 RVA: 0x001692A8 File Offset: 0x001682A8
		private ButtonBaseAdapter.LayoutOptions PaintPopupLayout(PaintEventArgs e, bool show3D)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.shadowedText = false;
			if (show3D)
			{
				layoutOptions.checkSize = 12;
			}
			else
			{
				layoutOptions.checkSize = 11;
				layoutOptions.checkPaddingSize = 1;
			}
			return layoutOptions;
		}
	}
}
