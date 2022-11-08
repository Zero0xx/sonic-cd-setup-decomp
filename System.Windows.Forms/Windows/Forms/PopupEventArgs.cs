using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020005B3 RID: 1459
	public class PopupEventArgs : CancelEventArgs
	{
		// Token: 0x06004BDA RID: 19418 RVA: 0x00112496 File Offset: 0x00111496
		public PopupEventArgs(IWin32Window associatedWindow, Control associatedControl, bool isBalloon, Size size)
		{
			this.associatedWindow = associatedWindow;
			this.size = size;
			this.associatedControl = associatedControl;
			this.isBalloon = isBalloon;
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06004BDB RID: 19419 RVA: 0x001124BB File Offset: 0x001114BB
		public IWin32Window AssociatedWindow
		{
			get
			{
				return this.associatedWindow;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x001124C3 File Offset: 0x001114C3
		public Control AssociatedControl
		{
			get
			{
				return this.associatedControl;
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06004BDD RID: 19421 RVA: 0x001124CB File Offset: 0x001114CB
		public bool IsBalloon
		{
			get
			{
				return this.isBalloon;
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x001124D3 File Offset: 0x001114D3
		// (set) Token: 0x06004BDF RID: 19423 RVA: 0x001124DB File Offset: 0x001114DB
		public Size ToolTipSize
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x0400312D RID: 12589
		private IWin32Window associatedWindow;

		// Token: 0x0400312E RID: 12590
		private Size size;

		// Token: 0x0400312F RID: 12591
		private Control associatedControl;

		// Token: 0x04003130 RID: 12592
		private bool isBalloon;
	}
}
