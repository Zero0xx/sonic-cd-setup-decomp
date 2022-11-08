using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000749 RID: 1865
	internal class RadioButtonPopupAdapter : RadioButtonFlatAdapter
	{
		// Token: 0x06006329 RID: 25385 RVA: 0x00169F1C File Offset: 0x00168F1C
		internal RadioButtonPopupAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x0600632A RID: 25386 RVA: 0x00169F28 File Offset: 0x00168F28
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			Graphics graphics = e.Graphics;
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintUp(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackgroundFlat(e, layoutData.checkBounds, colorData.buttonShadow, colorData.options.highContrast ? colorData.buttonFace : colorData.highlight, true);
			base.DrawCheckOnly(e, layoutData, colorData.windowText, colorData.highlight, true);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x0600632B RID: 25387 RVA: 0x0016A000 File Offset: 0x00169000
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			Graphics graphics = e.Graphics;
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintOver(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground3DLite(e, layoutData.checkBounds, colorData.windowText, colorData.options.highContrast ? colorData.buttonFace : colorData.highlight, colorData, true);
			base.DrawCheckOnly(e, layoutData, colorData.windowText, colorData.highlight, true);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x0600632C RID: 25388 RVA: 0x0016A0D8 File Offset: 0x001690D8
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			Graphics graphics = e.Graphics;
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonPopupAdapter buttonPopupAdapter = new ButtonPopupAdapter(base.Control);
				buttonPopupAdapter.PaintDown(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintPopupRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layoutData = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layoutData);
			base.DrawCheckBackground3DLite(e, layoutData.checkBounds, colorData.windowText, colorData.highlight, colorData, true);
			base.DrawCheckOnly(e, layoutData, colorData.buttonShadow, colorData.highlight, true);
			base.PaintField(e, layoutData, colorData, colorData.windowText, true);
		}

		// Token: 0x0600632D RID: 25389 RVA: 0x0016A199 File Offset: 0x00169199
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonPopupAdapter(base.Control);
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x0016A1A8 File Offset: 0x001691A8
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = base.Layout(e);
			if (!base.Control.MouseIsDown && !base.Control.MouseIsOver)
			{
				layoutOptions.shadowedText = true;
			}
			return layoutOptions;
		}
	}
}
