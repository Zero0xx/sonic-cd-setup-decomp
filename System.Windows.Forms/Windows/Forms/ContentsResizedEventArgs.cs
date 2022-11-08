using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020005E4 RID: 1508
	public class ContentsResizedEventArgs : EventArgs
	{
		// Token: 0x06004EB6 RID: 20150 RVA: 0x00122043 File Offset: 0x00121043
		public ContentsResizedEventArgs(Rectangle newRectangle)
		{
			this.newRectangle = newRectangle;
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06004EB7 RID: 20151 RVA: 0x00122052 File Offset: 0x00121052
		public Rectangle NewRectangle
		{
			get
			{
				return this.newRectangle;
			}
		}

		// Token: 0x040032CE RID: 13006
		private readonly Rectangle newRectangle;
	}
}
