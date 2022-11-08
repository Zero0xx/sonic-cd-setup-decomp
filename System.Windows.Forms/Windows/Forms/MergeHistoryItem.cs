using System;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020006B4 RID: 1716
	internal class MergeHistoryItem
	{
		// Token: 0x060059FC RID: 23036 RVA: 0x0014713F File Offset: 0x0014613F
		public MergeHistoryItem(MergeAction mergeAction)
		{
			this.mergeAction = mergeAction;
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x060059FD RID: 23037 RVA: 0x0014715C File Offset: 0x0014615C
		public MergeAction MergeAction
		{
			get
			{
				return this.mergeAction;
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x060059FE RID: 23038 RVA: 0x00147164 File Offset: 0x00146164
		// (set) Token: 0x060059FF RID: 23039 RVA: 0x0014716C File Offset: 0x0014616C
		public ToolStripItem TargetItem
		{
			get
			{
				return this.targetItem;
			}
			set
			{
				this.targetItem = value;
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06005A00 RID: 23040 RVA: 0x00147175 File Offset: 0x00146175
		// (set) Token: 0x06005A01 RID: 23041 RVA: 0x0014717D File Offset: 0x0014617D
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06005A02 RID: 23042 RVA: 0x00147186 File Offset: 0x00146186
		// (set) Token: 0x06005A03 RID: 23043 RVA: 0x0014718E File Offset: 0x0014618E
		public int PreviousIndex
		{
			get
			{
				return this.previousIndex;
			}
			set
			{
				this.previousIndex = value;
			}
		}

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06005A04 RID: 23044 RVA: 0x00147197 File Offset: 0x00146197
		// (set) Token: 0x06005A05 RID: 23045 RVA: 0x0014719F File Offset: 0x0014619F
		public ToolStripItemCollection PreviousIndexCollection
		{
			get
			{
				return this.previousIndexCollection;
			}
			set
			{
				this.previousIndexCollection = value;
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06005A06 RID: 23046 RVA: 0x001471A8 File Offset: 0x001461A8
		// (set) Token: 0x06005A07 RID: 23047 RVA: 0x001471B0 File Offset: 0x001461B0
		public ToolStripItemCollection IndexCollection
		{
			get
			{
				return this.indexCollection;
			}
			set
			{
				this.indexCollection = value;
			}
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x001471BC File Offset: 0x001461BC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"MergeAction: ",
				this.mergeAction.ToString(),
				" | TargetItem: ",
				(this.TargetItem == null) ? "null" : this.TargetItem.Text,
				" Index: ",
				this.index.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x040038A0 RID: 14496
		private MergeAction mergeAction;

		// Token: 0x040038A1 RID: 14497
		private ToolStripItem targetItem;

		// Token: 0x040038A2 RID: 14498
		private int index = -1;

		// Token: 0x040038A3 RID: 14499
		private int previousIndex = -1;

		// Token: 0x040038A4 RID: 14500
		private ToolStripItemCollection previousIndexCollection;

		// Token: 0x040038A5 RID: 14501
		private ToolStripItemCollection indexCollection;
	}
}
