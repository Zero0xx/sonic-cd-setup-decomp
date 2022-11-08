using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020004B3 RID: 1203
	public class MeasureItemEventArgs : EventArgs
	{
		// Token: 0x06004809 RID: 18441 RVA: 0x00105E1D File Offset: 0x00104E1D
		public MeasureItemEventArgs(Graphics graphics, int index, int itemHeight)
		{
			this.graphics = graphics;
			this.index = index;
			this.itemHeight = itemHeight;
			this.itemWidth = 0;
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x00105E41 File Offset: 0x00104E41
		public MeasureItemEventArgs(Graphics graphics, int index)
		{
			this.graphics = graphics;
			this.index = index;
			this.itemHeight = 0;
			this.itemWidth = 0;
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x0600480B RID: 18443 RVA: 0x00105E65 File Offset: 0x00104E65
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x0600480C RID: 18444 RVA: 0x00105E6D File Offset: 0x00104E6D
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x0600480D RID: 18445 RVA: 0x00105E75 File Offset: 0x00104E75
		// (set) Token: 0x0600480E RID: 18446 RVA: 0x00105E7D File Offset: 0x00104E7D
		public int ItemHeight
		{
			get
			{
				return this.itemHeight;
			}
			set
			{
				this.itemHeight = value;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x0600480F RID: 18447 RVA: 0x00105E86 File Offset: 0x00104E86
		// (set) Token: 0x06004810 RID: 18448 RVA: 0x00105E8E File Offset: 0x00104E8E
		public int ItemWidth
		{
			get
			{
				return this.itemWidth;
			}
			set
			{
				this.itemWidth = value;
			}
		}

		// Token: 0x04002209 RID: 8713
		private int itemHeight;

		// Token: 0x0400220A RID: 8714
		private int itemWidth;

		// Token: 0x0400220B RID: 8715
		private int index;

		// Token: 0x0400220C RID: 8716
		private readonly Graphics graphics;
	}
}
