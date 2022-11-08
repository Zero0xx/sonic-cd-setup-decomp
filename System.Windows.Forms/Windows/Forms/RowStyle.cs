using System;

namespace System.Windows.Forms
{
	// Token: 0x02000653 RID: 1619
	public class RowStyle : TableLayoutStyle
	{
		// Token: 0x0600550D RID: 21773 RVA: 0x0013604F File Offset: 0x0013504F
		public RowStyle()
		{
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x00136057 File Offset: 0x00135057
		public RowStyle(SizeType sizeType)
		{
			base.SizeType = sizeType;
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x00136066 File Offset: 0x00135066
		public RowStyle(SizeType sizeType, float height)
		{
			base.SizeType = sizeType;
			this.Height = height;
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x06005510 RID: 21776 RVA: 0x0013607C File Offset: 0x0013507C
		// (set) Token: 0x06005511 RID: 21777 RVA: 0x00136084 File Offset: 0x00135084
		public float Height
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
