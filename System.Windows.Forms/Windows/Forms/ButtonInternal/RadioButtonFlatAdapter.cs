using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000748 RID: 1864
	internal class RadioButtonFlatAdapter : RadioButtonBaseAdapter
	{
		// Token: 0x06006322 RID: 25378 RVA: 0x00169CBA File Offset: 0x00168CBA
		internal RadioButtonFlatAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x06006323 RID: 25379 RVA: 0x00169CC4 File Offset: 0x00168CC4
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonFlatAdapter buttonFlatAdapter = new ButtonFlatAdapter(base.Control);
				buttonFlatAdapter.PaintDown(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.highlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x06006324 RID: 25380 RVA: 0x00169D58 File Offset: 0x00168D58
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonFlatAdapter buttonFlatAdapter = new ButtonFlatAdapter(base.Control);
				buttonFlatAdapter.PaintOver(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.lowHighlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x06006325 RID: 25381 RVA: 0x00169DEC File Offset: 0x00168DEC
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				ButtonFlatAdapter buttonFlatAdapter = new ButtonFlatAdapter(base.Control);
				buttonFlatAdapter.PaintUp(e, base.Control.Checked ? CheckState.Checked : CheckState.Unchecked);
				return;
			}
			ButtonBaseAdapter.ColorData colorData = base.PaintFlatRender(e.Graphics).Calculate();
			if (base.Control.Enabled)
			{
				this.PaintFlatWorker(e, colorData.windowText, colorData.highlight, colorData.windowFrame, colorData);
				return;
			}
			this.PaintFlatWorker(e, colorData.buttonShadow, colorData.buttonFace, colorData.buttonShadow, colorData);
		}

		// Token: 0x06006326 RID: 25382 RVA: 0x00169E80 File Offset: 0x00168E80
		private void PaintFlatWorker(PaintEventArgs e, Color checkColor, Color checkBackground, Color checkBorder, ButtonBaseAdapter.ColorData colors)
		{
			Graphics graphics = e.Graphics;
			ButtonBaseAdapter.LayoutData layout = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layout);
			base.DrawCheckFlat(e, layout, checkColor, colors.options.highContrast ? colors.buttonFace : checkBackground, checkBorder);
			base.PaintField(e, layout, colors, checkColor, true);
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x00169EEB File Offset: 0x00168EEB
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonFlatAdapter(base.Control);
		}

		// Token: 0x06006328 RID: 25384 RVA: 0x00169EF8 File Offset: 0x00168EF8
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.checkSize = 12;
			layoutOptions.shadowedText = false;
			return layoutOptions;
		}

		// Token: 0x04003B55 RID: 15189
		protected const int flatCheckSize = 12;
	}
}
