using System;

namespace System.Windows.Forms
{
	// Token: 0x020006F9 RID: 1785
	public class TreeNodeMouseClickEventArgs : MouseEventArgs
	{
		// Token: 0x06005F25 RID: 24357 RVA: 0x0015A440 File Offset: 0x00159440
		public TreeNodeMouseClickEventArgs(TreeNode node, MouseButtons button, int clicks, int x, int y) : base(button, clicks, x, y, 0)
		{
			this.node = node;
		}

		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x06005F26 RID: 24358 RVA: 0x0015A456 File Offset: 0x00159456
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x040039C6 RID: 14790
		private TreeNode node;
	}
}
