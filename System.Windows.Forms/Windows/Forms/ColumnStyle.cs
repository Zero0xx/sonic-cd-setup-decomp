using System;

namespace System.Windows.Forms
{
	// Token: 0x02000652 RID: 1618
	public class ColumnStyle : TableLayoutStyle
	{
		// Token: 0x06005508 RID: 21768 RVA: 0x00136011 File Offset: 0x00135011
		public ColumnStyle()
		{
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x00136019 File Offset: 0x00135019
		public ColumnStyle(SizeType sizeType)
		{
			base.SizeType = sizeType;
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x00136028 File Offset: 0x00135028
		public ColumnStyle(SizeType sizeType, float width)
		{
			base.SizeType = sizeType;
			this.Width = width;
		}

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x0600550B RID: 21771 RVA: 0x0013603E File Offset: 0x0013503E
		// (set) Token: 0x0600550C RID: 21772 RVA: 0x00136046 File Offset: 0x00135046
		public float Width
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}
	}
}
