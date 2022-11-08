using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000695 RID: 1685
	public class ToolStripGripRenderEventArgs : ToolStripRenderEventArgs
	{
		// Token: 0x06005905 RID: 22789 RVA: 0x00143DAD File Offset: 0x00142DAD
		public ToolStripGripRenderEventArgs(Graphics g, ToolStrip toolStrip) : base(g, toolStrip)
		{
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06005906 RID: 22790 RVA: 0x00143DB7 File Offset: 0x00142DB7
		public Rectangle GripBounds
		{
			get
			{
				return base.ToolStrip.GripRectangle;
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06005907 RID: 22791 RVA: 0x00143DC4 File Offset: 0x00142DC4
		public ToolStripGripDisplayStyle GripDisplayStyle
		{
			get
			{
				return base.ToolStrip.GripDisplayStyle;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06005908 RID: 22792 RVA: 0x00143DD1 File Offset: 0x00142DD1
		public ToolStripGripStyle GripStyle
		{
			get
			{
				return base.ToolStrip.GripStyle;
			}
		}
	}
}
