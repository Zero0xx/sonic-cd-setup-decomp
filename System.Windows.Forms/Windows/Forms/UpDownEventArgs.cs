using System;

namespace System.Windows.Forms
{
	// Token: 0x0200071D RID: 1821
	public class UpDownEventArgs : EventArgs
	{
		// Token: 0x060060A4 RID: 24740 RVA: 0x00162389 File Offset: 0x00161389
		public UpDownEventArgs(int buttonPushed)
		{
			this.buttonID = buttonPushed;
		}

		// Token: 0x1700146F RID: 5231
		// (get) Token: 0x060060A5 RID: 24741 RVA: 0x00162398 File Offset: 0x00161398
		public int ButtonID
		{
			get
			{
				return this.buttonID;
			}
		}

		// Token: 0x04003A9A RID: 15002
		private int buttonID;
	}
}
