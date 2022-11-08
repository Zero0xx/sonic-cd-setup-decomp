using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000641 RID: 1601
	public class TabControlCancelEventArgs : CancelEventArgs
	{
		// Token: 0x06005476 RID: 21622 RVA: 0x0013436F File Offset: 0x0013336F
		public TabControlCancelEventArgs(TabPage tabPage, int tabPageIndex, bool cancel, TabControlAction action) : base(cancel)
		{
			this.tabPage = tabPage;
			this.tabPageIndex = tabPageIndex;
			this.action = action;
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x06005477 RID: 21623 RVA: 0x0013438E File Offset: 0x0013338E
		public TabPage TabPage
		{
			get
			{
				return this.tabPage;
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x06005478 RID: 21624 RVA: 0x00134396 File Offset: 0x00133396
		public int TabPageIndex
		{
			get
			{
				return this.tabPageIndex;
			}
		}

		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x06005479 RID: 21625 RVA: 0x0013439E File Offset: 0x0013339E
		public TabControlAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x040036D1 RID: 14033
		private TabPage tabPage;

		// Token: 0x040036D2 RID: 14034
		private int tabPageIndex;

		// Token: 0x040036D3 RID: 14035
		private TabControlAction action;
	}
}
