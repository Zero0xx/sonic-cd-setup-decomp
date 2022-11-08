using System;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	// Token: 0x020005D4 RID: 1492
	[ComVisible(true)]
	public class PropertyTabChangedEventArgs : EventArgs
	{
		// Token: 0x06004E35 RID: 20021 RVA: 0x00120FDB File Offset: 0x0011FFDB
		public PropertyTabChangedEventArgs(PropertyTab oldTab, PropertyTab newTab)
		{
			this.oldTab = oldTab;
			this.newTab = newTab;
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06004E36 RID: 20022 RVA: 0x00120FF1 File Offset: 0x0011FFF1
		public PropertyTab OldTab
		{
			get
			{
				return this.oldTab;
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06004E37 RID: 20023 RVA: 0x00120FF9 File Offset: 0x0011FFF9
		public PropertyTab NewTab
		{
			get
			{
				return this.newTab;
			}
		}

		// Token: 0x040032B0 RID: 12976
		private PropertyTab oldTab;

		// Token: 0x040032B1 RID: 12977
		private PropertyTab newTab;
	}
}
