using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000649 RID: 1609
	[TypeConverter(typeof(TableLayoutPanelCellPositionTypeConverter))]
	public struct TableLayoutPanelCellPosition
	{
		// Token: 0x060054B5 RID: 21685 RVA: 0x00134CD8 File Offset: 0x00133CD8
		public TableLayoutPanelCellPosition(int column, int row)
		{
			if (row < -1)
			{
				throw new ArgumentOutOfRangeException("row", SR.GetString("InvalidArgument", new object[]
				{
					"row",
					row.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (column < -1)
			{
				throw new ArgumentOutOfRangeException("column", SR.GetString("InvalidArgument", new object[]
				{
					"column",
					column.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.row = row;
			this.column = column;
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x060054B6 RID: 21686 RVA: 0x00134D63 File Offset: 0x00133D63
		// (set) Token: 0x060054B7 RID: 21687 RVA: 0x00134D6B File Offset: 0x00133D6B
		public int Row
		{
			get
			{
				return this.row;
			}
			set
			{
				this.row = value;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x060054B8 RID: 21688 RVA: 0x00134D74 File Offset: 0x00133D74
		// (set) Token: 0x060054B9 RID: 21689 RVA: 0x00134D7C File Offset: 0x00133D7C
		public int Column
		{
			get
			{
				return this.column;
			}
			set
			{
				this.column = value;
			}
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x00134D88 File Offset: 0x00133D88
		public override bool Equals(object other)
		{
			if (other is TableLayoutPanelCellPosition)
			{
				TableLayoutPanelCellPosition tableLayoutPanelCellPosition = (TableLayoutPanelCellPosition)other;
				return tableLayoutPanelCellPosition.row == this.row && tableLayoutPanelCellPosition.column == this.column;
			}
			return false;
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x00134DC6 File Offset: 0x00133DC6
		public static bool operator ==(TableLayoutPanelCellPosition p1, TableLayoutPanelCellPosition p2)
		{
			return p1.Row == p2.Row && p1.Column == p2.Column;
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x00134DEA File Offset: 0x00133DEA
		public static bool operator !=(TableLayoutPanelCellPosition p1, TableLayoutPanelCellPosition p2)
		{
			return !(p1 == p2);
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x00134DF8 File Offset: 0x00133DF8
		public override string ToString()
		{
			return this.Column.ToString(CultureInfo.CurrentCulture) + "," + this.Row.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x00134E38 File Offset: 0x00133E38
		public override int GetHashCode()
		{
			return WindowsFormsUtils.GetCombinedHashCodes(new int[]
			{
				this.row,
				this.column
			});
		}

		// Token: 0x040036E5 RID: 14053
		private int row;

		// Token: 0x040036E6 RID: 14054
		private int column;
	}
}
