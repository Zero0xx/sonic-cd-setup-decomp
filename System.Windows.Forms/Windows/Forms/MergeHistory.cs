using System;
using System.Collections.Generic;

namespace System.Windows.Forms
{
	// Token: 0x020006B3 RID: 1715
	internal class MergeHistory
	{
		// Token: 0x060059F9 RID: 23033 RVA: 0x0014710D File Offset: 0x0014610D
		public MergeHistory(ToolStrip mergedToolStrip)
		{
			this.mergedToolStrip = mergedToolStrip;
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x060059FA RID: 23034 RVA: 0x0014711C File Offset: 0x0014611C
		public Stack<MergeHistoryItem> MergeHistoryItemsStack
		{
			get
			{
				if (this.mergeHistoryItemsStack == null)
				{
					this.mergeHistoryItemsStack = new Stack<MergeHistoryItem>();
				}
				return this.mergeHistoryItemsStack;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x060059FB RID: 23035 RVA: 0x00147137 File Offset: 0x00146137
		public ToolStrip MergedToolStrip
		{
			get
			{
				return this.mergedToolStrip;
			}
		}

		// Token: 0x0400389E RID: 14494
		private Stack<MergeHistoryItem> mergeHistoryItemsStack;

		// Token: 0x0400389F RID: 14495
		private ToolStrip mergedToolStrip;
	}
}
