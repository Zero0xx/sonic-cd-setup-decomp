using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000609 RID: 1545
	[TypeConverter(typeof(SelectionRangeConverter))]
	public sealed class SelectionRange
	{
		// Token: 0x060050D0 RID: 20688 RVA: 0x001272AC File Offset: 0x001262AC
		public SelectionRange()
		{
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x001272E8 File Offset: 0x001262E8
		public SelectionRange(DateTime lower, DateTime upper)
		{
			if (lower < upper)
			{
				this.start = lower.Date;
				this.end = upper.Date;
				return;
			}
			this.start = upper.Date;
			this.end = lower.Date;
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x00127360 File Offset: 0x00126360
		public SelectionRange(SelectionRange range)
		{
			this.start = range.start;
			this.end = range.end;
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x060050D3 RID: 20691 RVA: 0x001273B1 File Offset: 0x001263B1
		// (set) Token: 0x060050D4 RID: 20692 RVA: 0x001273B9 File Offset: 0x001263B9
		public DateTime End
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value.Date;
			}
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x060050D5 RID: 20693 RVA: 0x001273C8 File Offset: 0x001263C8
		// (set) Token: 0x060050D6 RID: 20694 RVA: 0x001273D0 File Offset: 0x001263D0
		public DateTime Start
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value.Date;
			}
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x001273DF File Offset: 0x001263DF
		public override string ToString()
		{
			return "SelectionRange: Start: " + this.start.ToString() + ", End: " + this.end.ToString();
		}

		// Token: 0x04003509 RID: 13577
		private DateTime start = DateTime.MinValue.Date;

		// Token: 0x0400350A RID: 13578
		private DateTime end = DateTime.MaxValue.Date;
	}
}
