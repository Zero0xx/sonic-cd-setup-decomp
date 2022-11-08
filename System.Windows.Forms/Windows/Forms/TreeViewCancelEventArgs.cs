using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000702 RID: 1794
	public class TreeViewCancelEventArgs : CancelEventArgs
	{
		// Token: 0x06006028 RID: 24616 RVA: 0x0015EC0A File Offset: 0x0015DC0A
		public TreeViewCancelEventArgs(TreeNode node, bool cancel, TreeViewAction action) : base(cancel)
		{
			this.node = node;
			this.action = action;
		}

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x06006029 RID: 24617 RVA: 0x0015EC21 File Offset: 0x0015DC21
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x0600602A RID: 24618 RVA: 0x0015EC29 File Offset: 0x0015DC29
		public TreeViewAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x04003A1A RID: 14874
		private TreeNode node;

		// Token: 0x04003A1B RID: 14875
		private TreeViewAction action;
	}
}
