using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x0200074A RID: 1866
	internal class RadioButtonStandardAdapter : RadioButtonBaseAdapter
	{
		// Token: 0x0600632F RID: 25391 RVA: 0x0016A1DF File Offset: 0x001691DF
		internal RadioButtonStandardAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x06006330 RID: 25392 RVA: 0x0016A1E8 File Offset: 0x001691E8
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintUp(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintRender(e.Graphics).Calculate();
			ButtonBaseAdapter.LayoutData layout = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layout);
			base.DrawCheckBox(e, layout);
			base.PaintField(e, layout, colorData, colorData.windowText, true);
		}

		// Token: 0x06006331 RID: 25393 RVA: 0x0016A273 File Offset: 0x00169273
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintDown(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			this.PaintUp(e, state);
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x0016A2A9 File Offset: 0x001692A9
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintOver(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			this.PaintUp(e, state);
		}

		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x06006333 RID: 25395 RVA: 0x0016A2DF File Offset: 0x001692DF
		private new ButtonStandardAdapter ButtonAdapter
		{
			get
			{
				return (ButtonStandardAdapter)base.ButtonAdapter;
			}
		}

		// Token: 0x06006334 RID: 25396 RVA: 0x0016A2EC File Offset: 0x001692EC
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonStandardAdapter(base.Control);
		}

		// Token: 0x06006335 RID: 25397 RVA: 0x0016A2FC File Offset: 0x001692FC
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.hintTextUp = false;
			layoutOptions.everettButtonCompat = !Application.RenderWithVisualStyles;
			if (Application.RenderWithVisualStyles)
			{
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					layoutOptions.checkSize = RadioButtonRenderer.GetGlyphSize(graphics, RadioButtonRenderer.ConvertFromButtonState(base.GetState(), base.Control.MouseIsOver)).Width;
				}
			}
			return layoutOptions;
		}
	}
}
