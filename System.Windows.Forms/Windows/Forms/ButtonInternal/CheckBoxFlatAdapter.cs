using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000744 RID: 1860
	internal class CheckBoxFlatAdapter : CheckBoxBaseAdapter
	{
		// Token: 0x06006300 RID: 25344 RVA: 0x00168D07 File Offset: 0x00167D07
		internal CheckBoxFlatAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x06006301 RID: 25345 RVA: 0x00168D10 File Offset: 0x00167D10
		internal override void PaintDown(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintDown(e, base.Control.CheckState);
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

		// Token: 0x06006302 RID: 25346 RVA: 0x00168D98 File Offset: 0x00167D98
		internal override void PaintOver(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintOver(e, base.Control.CheckState);
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

		// Token: 0x06006303 RID: 25347 RVA: 0x00168E20 File Offset: 0x00167E20
		internal override void PaintUp(PaintEventArgs e, CheckState state)
		{
			if (base.Control.Appearance == Appearance.Button)
			{
				this.ButtonAdapter.PaintUp(e, base.Control.CheckState);
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

		// Token: 0x06006304 RID: 25348 RVA: 0x00168EA8 File Offset: 0x00167EA8
		private void PaintFlatWorker(PaintEventArgs e, Color checkColor, Color checkBackground, Color checkBorder, ButtonBaseAdapter.ColorData colors)
		{
			Graphics graphics = e.Graphics;
			ButtonBaseAdapter.LayoutData layout = this.Layout(e).Layout();
			base.PaintButtonBackground(e, base.Control.ClientRectangle, null);
			base.PaintImage(e, layout);
			base.DrawCheckFlat(e, layout, checkColor, colors.options.highContrast ? colors.buttonFace : checkBackground, checkBorder, colors);
			base.PaintField(e, layout, colors, checkColor, true);
		}

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x06006305 RID: 25349 RVA: 0x00168F15 File Offset: 0x00167F15
		private new ButtonFlatAdapter ButtonAdapter
		{
			get
			{
				return (ButtonFlatAdapter)base.ButtonAdapter;
			}
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x00168F22 File Offset: 0x00167F22
		protected override ButtonBaseAdapter CreateButtonAdapter()
		{
			return new ButtonFlatAdapter(base.Control);
		}

		// Token: 0x06006307 RID: 25351 RVA: 0x00168F30 File Offset: 0x00167F30
		protected override ButtonBaseAdapter.LayoutOptions Layout(PaintEventArgs e)
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = this.CommonLayout();
			layoutOptions.checkSize = 11;
			layoutOptions.shadowedText = false;
			return layoutOptions;
		}
	}
}
