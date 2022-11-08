using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000603 RID: 1539
	public class SearchForVirtualItemEventArgs : EventArgs
	{
		// Token: 0x060050BB RID: 20667 RVA: 0x001271EC File Offset: 0x001261EC
		public SearchForVirtualItemEventArgs(bool isTextSearch, bool isPrefixSearch, bool includeSubItemsInSearch, string text, Point startingPoint, SearchDirectionHint direction, int startIndex)
		{
			this.isTextSearch = isTextSearch;
			this.isPrefixSearch = isPrefixSearch;
			this.includeSubItemsInSearch = includeSubItemsInSearch;
			this.text = text;
			this.startingPoint = startingPoint;
			this.direction = direction;
			this.startIndex = startIndex;
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x060050BC RID: 20668 RVA: 0x0012723B File Offset: 0x0012623B
		public bool IsTextSearch
		{
			get
			{
				return this.isTextSearch;
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x00127243 File Offset: 0x00126243
		public bool IncludeSubItemsInSearch
		{
			get
			{
				return this.includeSubItemsInSearch;
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x060050BE RID: 20670 RVA: 0x0012724B File Offset: 0x0012624B
		// (set) Token: 0x060050BF RID: 20671 RVA: 0x00127253 File Offset: 0x00126253
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x060050C0 RID: 20672 RVA: 0x0012725C File Offset: 0x0012625C
		public bool IsPrefixSearch
		{
			get
			{
				return this.isPrefixSearch;
			}
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x060050C1 RID: 20673 RVA: 0x00127264 File Offset: 0x00126264
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x060050C2 RID: 20674 RVA: 0x0012726C File Offset: 0x0012626C
		public Point StartingPoint
		{
			get
			{
				return this.startingPoint;
			}
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x060050C3 RID: 20675 RVA: 0x00127274 File Offset: 0x00126274
		public SearchDirectionHint Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x060050C4 RID: 20676 RVA: 0x0012727C File Offset: 0x0012627C
		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
		}

		// Token: 0x040034F0 RID: 13552
		private bool isTextSearch;

		// Token: 0x040034F1 RID: 13553
		private bool isPrefixSearch;

		// Token: 0x040034F2 RID: 13554
		private bool includeSubItemsInSearch;

		// Token: 0x040034F3 RID: 13555
		private string text;

		// Token: 0x040034F4 RID: 13556
		private Point startingPoint;

		// Token: 0x040034F5 RID: 13557
		private SearchDirectionHint direction;

		// Token: 0x040034F6 RID: 13558
		private int startIndex;

		// Token: 0x040034F7 RID: 13559
		private int index = -1;
	}
}
