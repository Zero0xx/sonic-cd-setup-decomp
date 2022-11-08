using System;

namespace System.Windows.Forms
{
	// Token: 0x02000707 RID: 1799
	public class TreeViewHitTestInfo
	{
		// Token: 0x06006037 RID: 24631 RVA: 0x0015EC66 File Offset: 0x0015DC66
		public TreeViewHitTestInfo(TreeNode hitNode, TreeViewHitTestLocations hitLocation)
		{
			this.node = hitNode;
			this.loc = hitLocation;
		}

		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x06006038 RID: 24632 RVA: 0x0015EC7C File Offset: 0x0015DC7C
		public TreeViewHitTestLocations Location
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x06006039 RID: 24633 RVA: 0x0015EC84 File Offset: 0x0015DC84
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x04003A22 RID: 14882
		private TreeViewHitTestLocations loc;

		// Token: 0x04003A23 RID: 14883
		private TreeNode node;
	}
}
