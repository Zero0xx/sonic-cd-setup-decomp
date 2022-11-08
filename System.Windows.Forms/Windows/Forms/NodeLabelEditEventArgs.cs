using System;

namespace System.Windows.Forms
{
	// Token: 0x0200059F RID: 1439
	public class NodeLabelEditEventArgs : EventArgs
	{
		// Token: 0x06004A72 RID: 19058 RVA: 0x0010E501 File Offset: 0x0010D501
		public NodeLabelEditEventArgs(TreeNode node)
		{
			this.node = node;
			this.label = null;
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x0010E517 File Offset: 0x0010D517
		public NodeLabelEditEventArgs(TreeNode node, string label)
		{
			this.node = node;
			this.label = label;
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06004A74 RID: 19060 RVA: 0x0010E52D File Offset: 0x0010D52D
		// (set) Token: 0x06004A75 RID: 19061 RVA: 0x0010E535 File Offset: 0x0010D535
		public bool CancelEdit
		{
			get
			{
				return this.cancelEdit;
			}
			set
			{
				this.cancelEdit = value;
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06004A76 RID: 19062 RVA: 0x0010E53E File Offset: 0x0010D53E
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06004A77 RID: 19063 RVA: 0x0010E546 File Offset: 0x0010D546
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x040030B4 RID: 12468
		private readonly string label;

		// Token: 0x040030B5 RID: 12469
		private readonly TreeNode node;

		// Token: 0x040030B6 RID: 12470
		private bool cancelEdit;
	}
}
