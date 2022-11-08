using System;
using System.Drawing;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007AE RID: 1966
	internal sealed class DropDownButton : Button
	{
		// Token: 0x06006845 RID: 26693 RVA: 0x0017DDD4 File Offset: 0x0017CDD4
		public DropDownButton()
		{
			base.SetStyle(ControlStyles.Selectable, true);
			base.AccessibleName = SR.GetString("PropertyGridDropDownButtonAccessibleName");
		}

		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x06006846 RID: 26694 RVA: 0x0017DDF8 File Offset: 0x0017CDF8
		// (set) Token: 0x06006847 RID: 26695 RVA: 0x0017DE00 File Offset: 0x0017CE00
		public bool IgnoreMouse
		{
			get
			{
				return this.ignoreMouse;
			}
			set
			{
				this.ignoreMouse = value;
			}
		}

		// Token: 0x17001627 RID: 5671
		// (set) Token: 0x06006848 RID: 26696 RVA: 0x0017DE09 File Offset: 0x0017CE09
		public bool UseComboBoxTheme
		{
			set
			{
				if (this.useComboBoxTheme != value)
				{
					this.useComboBoxTheme = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x06006849 RID: 26697 RVA: 0x0017DE21 File Offset: 0x0017CE21
		protected override void OnClick(EventArgs e)
		{
			if (!this.IgnoreMouse)
			{
				base.OnClick(e);
			}
		}

		// Token: 0x0600684A RID: 26698 RVA: 0x0017DE32 File Offset: 0x0017CE32
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!this.IgnoreMouse)
			{
				base.OnMouseUp(e);
			}
		}

		// Token: 0x0600684B RID: 26699 RVA: 0x0017DE43 File Offset: 0x0017CE43
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!this.IgnoreMouse)
			{
				base.OnMouseDown(e);
			}
		}

		// Token: 0x0600684C RID: 26700 RVA: 0x0017DE54 File Offset: 0x0017CE54
		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			if (Application.RenderWithVisualStyles & this.useComboBoxTheme)
			{
				ComboBoxState state = ComboBoxState.Normal;
				if (base.MouseIsDown)
				{
					state = ComboBoxState.Pressed;
				}
				else if (base.MouseIsOver)
				{
					state = ComboBoxState.Hot;
				}
				ComboBoxRenderer.DrawDropDownButton(pevent.Graphics, new Rectangle(0, 0, base.Width, base.Height), state);
			}
		}

		// Token: 0x0600684D RID: 26701 RVA: 0x0017DEAD File Offset: 0x0017CEAD
		internal override ButtonBaseAdapter CreateStandardAdapter()
		{
			return new DropDownButtonAdapter(this);
		}

		// Token: 0x04003D6A RID: 15722
		private bool useComboBoxTheme;

		// Token: 0x04003D6B RID: 15723
		private bool ignoreMouse;
	}
}
