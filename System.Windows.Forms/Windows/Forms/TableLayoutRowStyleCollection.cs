using System;
using System.Collections;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000659 RID: 1625
	public class TableLayoutRowStyleCollection : TableLayoutStyleCollection
	{
		// Token: 0x06005546 RID: 21830 RVA: 0x00136A6F File Offset: 0x00135A6F
		internal TableLayoutRowStyleCollection(IArrangedElement Owner) : base(Owner)
		{
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x00136A78 File Offset: 0x00135A78
		internal TableLayoutRowStyleCollection() : base(null)
		{
		}

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x06005548 RID: 21832 RVA: 0x00136A81 File Offset: 0x00135A81
		internal override string PropertyName
		{
			get
			{
				return PropertyNames.RowStyles;
			}
		}

		// Token: 0x06005549 RID: 21833 RVA: 0x00136A88 File Offset: 0x00135A88
		public int Add(RowStyle rowStyle)
		{
			return ((IList)this).Add(rowStyle);
		}

		// Token: 0x0600554A RID: 21834 RVA: 0x00136A91 File Offset: 0x00135A91
		public void Insert(int index, RowStyle rowStyle)
		{
			((IList)this).Insert(index, rowStyle);
		}

		// Token: 0x170011AD RID: 4525
		public RowStyle this[int index]
		{
			get
			{
				return (RowStyle)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x00136AB3 File Offset: 0x00135AB3
		public void Remove(RowStyle rowStyle)
		{
			((IList)this).Remove(rowStyle);
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x00136ABC File Offset: 0x00135ABC
		public bool Contains(RowStyle rowStyle)
		{
			return ((IList)this).Contains(rowStyle);
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x00136AC5 File Offset: 0x00135AC5
		public int IndexOf(RowStyle rowStyle)
		{
			return ((IList)this).IndexOf(rowStyle);
		}
	}
}
