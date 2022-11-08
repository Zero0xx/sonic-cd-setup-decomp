using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
	// Token: 0x020006D3 RID: 1747
	internal class ToolStripProfessionalLowResolutionRenderer : ToolStripProfessionalRenderer
	{
		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x06005C27 RID: 23591 RVA: 0x0014FF93 File Offset: 0x0014EF93
		internal override ToolStripRenderer RendererOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x0014FF96 File Offset: 0x0014EF96
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip is ToolStripDropDown)
			{
				base.OnRenderToolStripBackground(e);
			}
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x0014FFAC File Offset: 0x0014EFAC
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip is MenuStrip)
			{
				return;
			}
			if (e.ToolStrip is StatusStrip)
			{
				return;
			}
			if (e.ToolStrip is ToolStripDropDown)
			{
				base.OnRenderToolStripBorder(e);
				return;
			}
			this.RenderToolStripBorderInternal(e);
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x0014FFE8 File Offset: 0x0014EFE8
		private void RenderToolStripBorderInternal(ToolStripRenderEventArgs e)
		{
			Rectangle rectangle = new Rectangle(Point.Empty, e.ToolStrip.Size);
			Graphics graphics = e.Graphics;
			using (Pen pen = new Pen(SystemColors.ButtonShadow))
			{
				pen.DashStyle = DashStyle.Dot;
				bool flag = (rectangle.Width & 1) == 1;
				bool flag2 = (rectangle.Height & 1) == 1;
				int num = 2;
				graphics.DrawLine(pen, rectangle.X + num, rectangle.Y, rectangle.Width - 1, rectangle.Y);
				graphics.DrawLine(pen, rectangle.X + num, rectangle.Height - 1, rectangle.Width - 1, rectangle.Height - 1);
				graphics.DrawLine(pen, rectangle.X, rectangle.Y + num, rectangle.X, rectangle.Height - 1);
				graphics.DrawLine(pen, rectangle.Width - 1, rectangle.Y + num, rectangle.Width - 1, rectangle.Height - 1);
				graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(1, 1, 1, 1));
				if (flag)
				{
					graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(rectangle.Width - 2, 1, 1, 1));
				}
				if (flag2)
				{
					graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(1, rectangle.Height - 2, 1, 1));
				}
				if (flag2 && flag)
				{
					graphics.FillRectangle(SystemBrushes.ButtonShadow, new Rectangle(rectangle.Width - 2, rectangle.Height - 2, 1, 1));
				}
			}
		}
	}
}
