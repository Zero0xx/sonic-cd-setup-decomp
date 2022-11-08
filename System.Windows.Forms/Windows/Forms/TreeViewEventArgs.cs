using System;

namespace System.Windows.Forms
{
	// Token: 0x02000705 RID: 1797
	public class TreeViewEventArgs : EventArgs
	{
		// Token: 0x0600602F RID: 24623 RVA: 0x0015EC31 File Offset: 0x0015DC31
		public TreeViewEventArgs(TreeNode node)
		{
			this.node = node;
		}

		// Token: 0x06006030 RID: 24624 RVA: 0x0015EC40 File Offset: 0x0015DC40
		public TreeViewEventArgs(TreeNode node, TreeViewAction action)
		{
			this.node = node;
			this.action = action;
		}

		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x06006031 RID: 24625 RVA: 0x0015EC56 File Offset: 0x0015DC56
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x06006032 RID: 24626 RVA: 0x0015EC5E File Offset: 0x0015DC5E
		public TreeViewAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x04003A20 RID: 14880
		private TreeNode node;

		// Token: 0x04003A21 RID: 14881
		private TreeViewAction action;
	}
}
