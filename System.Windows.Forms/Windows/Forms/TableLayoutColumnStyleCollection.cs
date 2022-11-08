using System;
using System.Collections;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000658 RID: 1624
	public class TableLayoutColumnStyleCollection : TableLayoutStyleCollection
	{
		// Token: 0x0600553C RID: 21820 RVA: 0x00136A10 File Offset: 0x00135A10
		internal TableLayoutColumnStyleCollection(IArrangedElement Owner) : base(Owner)
		{
		}

		// Token: 0x0600553D RID: 21821 RVA: 0x00136A19 File Offset: 0x00135A19
		internal TableLayoutColumnStyleCollection() : base(null)
		{
		}

		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x0600553E RID: 21822 RVA: 0x00136A22 File Offset: 0x00135A22
		internal override string PropertyName
		{
			get
			{
				return PropertyNames.ColumnStyles;
			}
		}

		// Token: 0x0600553F RID: 21823 RVA: 0x00136A29 File Offset: 0x00135A29
		public int Add(ColumnStyle columnStyle)
		{
			return ((IList)this).Add(columnStyle);
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x00136A32 File Offset: 0x00135A32
		public void Insert(int index, ColumnStyle columnStyle)
		{
			((IList)this).Insert(index, columnStyle);
		}

		// Token: 0x170011AB RID: 4523
		public ColumnStyle this[int index]
		{
			get
			{
				return (ColumnStyle)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		// Token: 0x06005543 RID: 21827 RVA: 0x00136A54 File Offset: 0x00135A54
		public void Remove(ColumnStyle columnStyle)
		{
			((IList)this).Remove(columnStyle);
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x00136A5D File Offset: 0x00135A5D
		public bool Contains(ColumnStyle columnStyle)
		{
			return ((IList)this).Contains(columnStyle);
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x00136A66 File Offset: 0x00135A66
		public int IndexOf(ColumnStyle columnStyle)
		{
			return ((IList)this).IndexOf(columnStyle);
		}
	}
}
