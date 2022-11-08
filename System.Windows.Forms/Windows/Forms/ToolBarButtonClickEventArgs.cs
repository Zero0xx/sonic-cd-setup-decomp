using System;

namespace System.Windows.Forms
{
	// Token: 0x0200066B RID: 1643
	public class ToolBarButtonClickEventArgs : EventArgs
	{
		// Token: 0x06005685 RID: 22149 RVA: 0x0013AE91 File Offset: 0x00139E91
		public ToolBarButtonClickEventArgs(ToolBarButton button)
		{
			this.button = button;
		}

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06005686 RID: 22150 RVA: 0x0013AEA0 File Offset: 0x00139EA0
		// (set) Token: 0x06005687 RID: 22151 RVA: 0x0013AEA8 File Offset: 0x00139EA8
		public ToolBarButton Button
		{
			get
			{
				return this.button;
			}
			set
			{
				this.button = value;
			}
		}

		// Token: 0x04003763 RID: 14179
		private ToolBarButton button;
	}
}
