using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000623 RID: 1571
	[ComVisible(true)]
	public class SplitterEventArgs : EventArgs
	{
		// Token: 0x06005211 RID: 21009 RVA: 0x0012D4AB File Offset: 0x0012C4AB
		public SplitterEventArgs(int x, int y, int splitX, int splitY)
		{
			this.x = x;
			this.y = y;
			this.splitX = splitX;
			this.splitY = splitY;
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06005212 RID: 21010 RVA: 0x0012D4D0 File Offset: 0x0012C4D0
		public int X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06005213 RID: 21011 RVA: 0x0012D4D8 File Offset: 0x0012C4D8
		public int Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x06005214 RID: 21012 RVA: 0x0012D4E0 File Offset: 0x0012C4E0
		// (set) Token: 0x06005215 RID: 21013 RVA: 0x0012D4E8 File Offset: 0x0012C4E8
		public int SplitX
		{
			get
			{
				return this.splitX;
			}
			set
			{
				this.splitX = value;
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x06005216 RID: 21014 RVA: 0x0012D4F1 File Offset: 0x0012C4F1
		// (set) Token: 0x06005217 RID: 21015 RVA: 0x0012D4F9 File Offset: 0x0012C4F9
		public int SplitY
		{
			get
			{
				return this.splitY;
			}
			set
			{
				this.splitY = value;
			}
		}

		// Token: 0x04003623 RID: 13859
		private readonly int x;

		// Token: 0x04003624 RID: 13860
		private readonly int y;

		// Token: 0x04003625 RID: 13861
		private int splitX;

		// Token: 0x04003626 RID: 13862
		private int splitY;
	}
}
