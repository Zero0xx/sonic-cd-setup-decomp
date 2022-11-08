using System;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007B2 RID: 1970
	internal class GridEntryRecreateChildrenEventArgs : EventArgs
	{
		// Token: 0x06006859 RID: 26713 RVA: 0x0017E0FC File Offset: 0x0017D0FC
		public GridEntryRecreateChildrenEventArgs(int oldCount, int newCount)
		{
			this.OldChildCount = oldCount;
			this.NewChildCount = newCount;
		}

		// Token: 0x04003D6D RID: 15725
		public readonly int OldChildCount;

		// Token: 0x04003D6E RID: 15726
		public readonly int NewChildCount;
	}
}
