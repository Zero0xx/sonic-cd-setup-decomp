using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020006FD RID: 1789
	[ComVisible(true)]
	public class TreeNodeMouseHoverEventArgs : EventArgs
	{
		// Token: 0x06005F5D RID: 24413 RVA: 0x0015AF89 File Offset: 0x00159F89
		public TreeNodeMouseHoverEventArgs(TreeNode node)
		{
			this.node = node;
		}

		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x06005F5E RID: 24414 RVA: 0x0015AF98 File Offset: 0x00159F98
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x040039CA RID: 14794
		private readonly TreeNode node;
	}
}
