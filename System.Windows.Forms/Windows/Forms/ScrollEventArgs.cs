using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020005FE RID: 1534
	[ComVisible(true)]
	public class ScrollEventArgs : EventArgs
	{
		// Token: 0x060050AE RID: 20654 RVA: 0x00127131 File Offset: 0x00126131
		public ScrollEventArgs(ScrollEventType type, int newValue)
		{
			this.type = type;
			this.newValue = newValue;
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x0012714E File Offset: 0x0012614E
		public ScrollEventArgs(ScrollEventType type, int newValue, ScrollOrientation scroll)
		{
			this.type = type;
			this.newValue = newValue;
			this.scrollOrientation = scroll;
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x00127172 File Offset: 0x00126172
		public ScrollEventArgs(ScrollEventType type, int oldValue, int newValue)
		{
			this.type = type;
			this.newValue = newValue;
			this.oldValue = oldValue;
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x00127196 File Offset: 0x00126196
		public ScrollEventArgs(ScrollEventType type, int oldValue, int newValue, ScrollOrientation scroll)
		{
			this.type = type;
			this.newValue = newValue;
			this.scrollOrientation = scroll;
			this.oldValue = oldValue;
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x060050B2 RID: 20658 RVA: 0x001271C2 File Offset: 0x001261C2
		public ScrollOrientation ScrollOrientation
		{
			get
			{
				return this.scrollOrientation;
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x060050B3 RID: 20659 RVA: 0x001271CA File Offset: 0x001261CA
		public ScrollEventType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x060050B4 RID: 20660 RVA: 0x001271D2 File Offset: 0x001261D2
		// (set) Token: 0x060050B5 RID: 20661 RVA: 0x001271DA File Offset: 0x001261DA
		public int NewValue
		{
			get
			{
				return this.newValue;
			}
			set
			{
				this.newValue = value;
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x060050B6 RID: 20662 RVA: 0x001271E3 File Offset: 0x001261E3
		public int OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		// Token: 0x040034DA RID: 13530
		private readonly ScrollEventType type;

		// Token: 0x040034DB RID: 13531
		private int newValue;

		// Token: 0x040034DC RID: 13532
		private ScrollOrientation scrollOrientation;

		// Token: 0x040034DD RID: 13533
		private int oldValue = -1;
	}
}
