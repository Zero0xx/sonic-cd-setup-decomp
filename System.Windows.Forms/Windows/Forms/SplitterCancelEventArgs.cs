using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000621 RID: 1569
	public class SplitterCancelEventArgs : CancelEventArgs
	{
		// Token: 0x06005206 RID: 20998 RVA: 0x0012D453 File Offset: 0x0012C453
		public SplitterCancelEventArgs(int mouseCursorX, int mouseCursorY, int splitX, int splitY) : base(false)
		{
			this.mouseCursorX = mouseCursorX;
			this.mouseCursorY = mouseCursorY;
			this.splitX = splitX;
			this.splitY = splitY;
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06005207 RID: 20999 RVA: 0x0012D479 File Offset: 0x0012C479
		public int MouseCursorX
		{
			get
			{
				return this.mouseCursorX;
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06005208 RID: 21000 RVA: 0x0012D481 File Offset: 0x0012C481
		public int MouseCursorY
		{
			get
			{
				return this.mouseCursorY;
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06005209 RID: 21001 RVA: 0x0012D489 File Offset: 0x0012C489
		// (set) Token: 0x0600520A RID: 21002 RVA: 0x0012D491 File Offset: 0x0012C491
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

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x0600520B RID: 21003 RVA: 0x0012D49A File Offset: 0x0012C49A
		// (set) Token: 0x0600520C RID: 21004 RVA: 0x0012D4A2 File Offset: 0x0012C4A2
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

		// Token: 0x0400361F RID: 13855
		private readonly int mouseCursorX;

		// Token: 0x04003620 RID: 13856
		private readonly int mouseCursorY;

		// Token: 0x04003621 RID: 13857
		private int splitX;

		// Token: 0x04003622 RID: 13858
		private int splitY;
	}
}
