using System;

namespace System.Windows.Forms
{
	// Token: 0x020005E6 RID: 1510
	public class RetrieveVirtualItemEventArgs : EventArgs
	{
		// Token: 0x06004EBC RID: 20156 RVA: 0x0012205A File Offset: 0x0012105A
		public RetrieveVirtualItemEventArgs(int itemIndex)
		{
			this.itemIndex = itemIndex;
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06004EBD RID: 20157 RVA: 0x00122069 File Offset: 0x00121069
		public int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x06004EBE RID: 20158 RVA: 0x00122071 File Offset: 0x00121071
		// (set) Token: 0x06004EBF RID: 20159 RVA: 0x00122079 File Offset: 0x00121079
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
			set
			{
				this.item = value;
			}
		}

		// Token: 0x040032CF RID: 13007
		private int itemIndex;

		// Token: 0x040032D0 RID: 13008
		private ListViewItem item;
	}
}
