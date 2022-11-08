using System;
using System.Drawing;
using System.Windows.Forms.ButtonInternal;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007AF RID: 1967
	internal class DropDownButtonAdapter : ButtonStandardAdapter
	{
		// Token: 0x0600684E RID: 26702 RVA: 0x0017DEB5 File Offset: 0x0017CEB5
		internal DropDownButtonAdapter(ButtonBase control) : base(control)
		{
		}

		// Token: 0x0600684F RID: 26703 RVA: 0x0017DEC0 File Offset: 0x0017CEC0
		private void DDB_Draw3DBorder(Graphics g, Rectangle r, bool raised)
		{
			if (base.Control.BackColor != SystemColors.Control && SystemInformation.HighContrast)
			{
				if (raised)
				{
					Color color = ControlPaint.LightLight(base.Control.BackColor);
					ControlPaint.DrawBorder(g, r, color, 1, ButtonBorderStyle.Outset, color, 1, ButtonBorderStyle.Outset, color, 2, ButtonBorderStyle.Inset, color, 2, ButtonBorderStyle.Inset);
					return;
				}
				ControlPaint.DrawBorder(g, r, ControlPaint.Dark(base.Control.BackColor), ButtonBorderStyle.Solid);
				return;
			}
			else
			{
				if (raised)
				{
					Color color2 = ControlPaint.Light(base.Control.BackColor);
					ControlPaint.DrawBorder(g, r, color2, 1, ButtonBorderStyle.Solid, color2, 1, ButtonBorderStyle.Solid, base.Control.BackColor, 2, ButtonBorderStyle.Outset, base.Control.BackColor, 2, ButtonBorderStyle.Outset);
					Rectangle bounds = r;
					bounds.Offset(1, 1);
					bounds.Width -= 3;
					bounds.Height -= 3;
					color2 = ControlPaint.LightLight(base.Control.BackColor);
					ControlPaint.DrawBorder(g, bounds, color2, 1, ButtonBorderStyle.Solid, color2, 1, ButtonBorderStyle.Solid, color2, 1, ButtonBorderStyle.None, color2, 1, ButtonBorderStyle.None);
					return;
				}
				ControlPaint.DrawBorder(g, r, ControlPaint.Dark(base.Control.BackColor), ButtonBorderStyle.Solid);
				return;
			}
		}

		// Token: 0x06006850 RID: 26704 RVA: 0x0017DFD0 File Offset: 0x0017CFD0
		internal override void PaintUp(PaintEventArgs pevent, CheckState state)
		{
			base.PaintUp(pevent, state);
			if (!Application.RenderWithVisualStyles)
			{
				this.DDB_Draw3DBorder(pevent.Graphics, base.Control.ClientRectangle, true);
				return;
			}
			Color window = SystemColors.Window;
			Rectangle clientRectangle = base.Control.ClientRectangle;
			clientRectangle.Inflate(0, -1);
			ControlPaint.DrawBorder(pevent.Graphics, clientRectangle, window, 1, ButtonBorderStyle.None, window, 1, ButtonBorderStyle.None, window, 1, ButtonBorderStyle.Solid, window, 1, ButtonBorderStyle.None);
		}

		// Token: 0x06006851 RID: 26705 RVA: 0x0017E038 File Offset: 0x0017D038
		internal override void DrawImageCore(Graphics graphics, Image image, Rectangle imageBounds, Point imageStart, ButtonBaseAdapter.LayoutData layout)
		{
			ControlPaint.DrawImageReplaceColor(graphics, image, imageBounds, Color.Black, base.Control.ForeColor);
		}
	}
}
