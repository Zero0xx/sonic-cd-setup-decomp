using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006DC RID: 1756
	public class ToolStripSeparatorRenderEventArgs : ToolStripItemRenderEventArgs
	{
		// Token: 0x06005CC0 RID: 23744 RVA: 0x00150E99 File Offset: 0x0014FE99
		public ToolStripSeparatorRenderEventArgs(Graphics g, ToolStripSeparator separator, bool vertical) : base(g, separator)
		{
			this.vertical = vertical;
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x06005CC1 RID: 23745 RVA: 0x00150EAA File Offset: 0x0014FEAA
		public bool Vertical
		{
			get
			{
				return this.vertical;
			}
		}

		// Token: 0x04003925 RID: 14629
		private bool vertical;
	}
}
