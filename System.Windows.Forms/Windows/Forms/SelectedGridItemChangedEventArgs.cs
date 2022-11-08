using System;

namespace System.Windows.Forms
{
	// Token: 0x02000606 RID: 1542
	public class SelectedGridItemChangedEventArgs : EventArgs
	{
		// Token: 0x060050C9 RID: 20681 RVA: 0x00127284 File Offset: 0x00126284
		public SelectedGridItemChangedEventArgs(GridItem oldSel, GridItem newSel)
		{
			this.oldSelection = oldSel;
			this.newSelection = newSel;
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x060050CA RID: 20682 RVA: 0x0012729A File Offset: 0x0012629A
		public GridItem NewSelection
		{
			get
			{
				return this.newSelection;
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x001272A2 File Offset: 0x001262A2
		public GridItem OldSelection
		{
			get
			{
				return this.oldSelection;
			}
		}

		// Token: 0x04003502 RID: 13570
		private GridItem oldSelection;

		// Token: 0x04003503 RID: 13571
		private GridItem newSelection;
	}
}
