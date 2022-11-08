using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000746 RID: 1862
	internal sealed class CheckBoxStandardAdapter : CheckBoxBaseAdapter
	{
		// Token: 0x06006310 RID: 25360 RVA: 0x001692E0 File Offset: 0x001682E0
		internal CheckBoxStandardAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x06006311 RID: 25361 RVA: 0x001692EC File Offset: 0x001682EC
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintUp(e, base.Control.CheckState);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			int num = layoutData.focus.X & 1;
			if (!Application.RenderWithVisualStyles)
			{
				num = 1 - num;
			}
			if (!layoutData.options.everettButtonCompat)
			{
				layoutData.textBounds.Offset(-1, -1);
			}
			layoutData.imageBounds.Offset(-1, -1);
			layoutData.focus.Offset(-(num + 1), -2);
			layoutData.focus.Width = layoutData.textBounds.Width + layoutData.imageBounds.Width - 1;
			layoutData.focus.Intersect(layoutData.textBounds);
			if (layoutData.options.textAlign != (ContentAlignment)273 && layoutData.options.useCompatibleTextRendering && layoutData.options.font.Italic)
			{
				ButtonBaseAdapter.LayoutData layoutData2 = layoutData;
				layoutData2.focus.Width = layoutData2.focus.Width + 2;
			}
			base.PaintImage(e, layoutData);
			base.DrawCheckBox(e, layoutData);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x06006312 RID: 25362 RVA: 0x0016943B File Offset: 0x0016843B
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintDown(e, base.Control.CheckState);
				return;
			}
			this.PaintUp(e, state);
		}

		// Token: 0x06006313 RID: 25363 RVA: 0x0016946B File Offset: 0x0016846B
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintOver(e, base.Control.CheckState);
				return;
			}
			this.PaintUp(e, state);
		}

		// Token: 0x06006314 RID: 25364 RVA: 0x0016949C File Offset: 0x0016849C
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonStandardAdapter buttonStandardAdapter = new ButtonStandardAdapter(base.Control);
				return buttonStandardAdapter.GetPreferredSizeCore(proposedSize);
			}
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

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x06006315 RID: 25365 RVA: 0x00169528 File Offset: 0x00168528
		private new ButtonStandardAdapter ButtonAdapter
		{
			get
			{
				return (ButtonStandardAdapter)base.ButtonAdapter;
			}
		}

		// Token: 0x06006316 RID: 25366 RVA: 0x00169535 File Offset: 0x00168535
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonStandardAdapter(base.Control);
		}

		// Token: 0x06006317 RID: 25367 RVA: 0x00169544 File Offset: 0x00168544
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.checkPaddingSize = 1;
			layoutOptions.everettButtonCompat = !Application.RenderWithVisualStyles;
			if (Application.RenderWithVisualStyles)
			{
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					layoutOptions.checkSize = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxRenderer.ConvertFromButtonState(base.GetState(), true, base.Control.MouseIsOver)).Width;
				}
			}
			return layoutOptions;
		}
	}
}
