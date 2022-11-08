using System;
using System.Drawing;

namespace System.Windows.Forms.ButtonInternal
{
	// Token: 0x02000742 RID: 1858
	internal abstract class CheckableControlBaseAdapter : ButtonBaseAdapter
	{
		// Token: 0x060062ED RID: 25325 RVA: 0x00168355 File Offset: 0x00167355
		internal CheckableControlBaseAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x060062EE RID: 25326 RVA: 0x0016835E File Offset: 0x0016735E
		protected ButtonBaseAdapter ButtonAdapter
		{
			get
			{
				if (this.buttonAdapter == null)
				{
					this.buttonAdapter = this.CreateButtonAdapter();
				}
				return this.buttonAdapter;
			}
		}

		// Token: 0x060062EF RID: 25327 RVA: 0x0016837C File Offset: 0x0016737C
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (this.Appearance == Appearance.Button)
			{
				return this.ButtonAdapter.GetPreferredSizeCore(proposedSize);
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

		// Token: 0x060062F0 RID: 25328
		protected abstract ButtonBaseAdapter CreateButtonAdapter();

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x060062F1 RID: 25329 RVA: 0x001683FC File Offset: 0x001673FC
		private Appearance Appearance
		{
			get
			{
				CheckBox checkBox = base.Control as CheckBox;
				if (checkBox != null)
				{
					return checkBox.Appearance;
				}
				RadioButton radioButton = base.Control as RadioButton;
				if (radioButton != null)
				{
					return radioButton.Appearance;
				}
				return Appearance.Normal;
			}
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x00168438 File Offset: 0x00167438
		internal override ButtonBaseAdapter.LayoutOptions CommonLayout()
		{
			ButtonBaseAdapter.LayoutOptions layoutOptions = base.CommonLayout();
			layoutOptions.growBorderBy1PxWhenDefault = false;
			layoutOptions.borderSize = 0;
			layoutOptions.paddingSize = 0;
			layoutOptions.maxFocus = false;
			layoutOptions.focusOddEvenFixup = true;
			layoutOptions.checkSize = 13;
			return layoutOptions;
		}

		// Token: 0x04003B4E RID: 15182
		private const int standardCheckSize = 13;

		// Token: 0x04003B4F RID: 15183
		private ButtonBaseAdapter buttonAdapter;
	}
}
