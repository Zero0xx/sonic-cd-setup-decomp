using System;
using System.Drawing;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020006D8 RID: 1752
	internal class ToolStripScrollButton : ToolStripControlHost
	{
		// Token: 0x06005C71 RID: 23665 RVA: 0x00150813 File Offset: 0x0014F813
		public ToolStripScrollButton(bool up) : base(ToolStripScrollButton.CreateControlInstance(up))
		{
			this.up = up;
		}

		// Token: 0x06005C72 RID: 23666 RVA: 0x00150830 File Offset: 0x0014F830
		private static Control CreateControlInstance(bool up)
		{
			return new ToolStripScrollButton.StickyLabel
			{
				ImageAlign = ContentAlignment.MiddleCenter,
				Image = (up ? ToolStripScrollButton.UpImage : ToolStripScrollButton.DownImage)
			};
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06005C73 RID: 23667 RVA: 0x00150861 File Offset: 0x0014F861
		protected internal override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x06005C74 RID: 23668 RVA: 0x00150868 File Offset: 0x0014F868
		protected override Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x06005C75 RID: 23669 RVA: 0x0015086F File Offset: 0x0014F86F
		private static Image DownImage
		{
			get
			{
				if (ToolStripScrollButton.downScrollImage == null)
				{
					ToolStripScrollButton.downScrollImage = new Bitmap(typeof(ToolStripScrollButton), "ScrollButtonDown.bmp");
					ToolStripScrollButton.downScrollImage.MakeTransparent(Color.White);
				}
				return ToolStripScrollButton.downScrollImage;
			}
		}

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x06005C76 RID: 23670 RVA: 0x001508A5 File Offset: 0x0014F8A5
		internal ToolStripScrollButton.StickyLabel Label
		{
			get
			{
				return base.Control as ToolStripScrollButton.StickyLabel;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06005C77 RID: 23671 RVA: 0x001508B2 File Offset: 0x0014F8B2
		private static Image UpImage
		{
			get
			{
				if (ToolStripScrollButton.upScrollImage == null)
				{
					ToolStripScrollButton.upScrollImage = new Bitmap(typeof(ToolStripScrollButton), "ScrollButtonUp.bmp");
					ToolStripScrollButton.upScrollImage.MakeTransparent(Color.White);
				}
				return ToolStripScrollButton.upScrollImage;
			}
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06005C78 RID: 23672 RVA: 0x001508E8 File Offset: 0x0014F8E8
		private Timer MouseDownTimer
		{
			get
			{
				if (this.mouseDownTimer == null)
				{
					this.mouseDownTimer = new Timer();
				}
				return this.mouseDownTimer;
			}
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x00150903 File Offset: 0x0014F903
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.mouseDownTimer != null)
			{
				this.mouseDownTimer.Enabled = false;
				this.mouseDownTimer.Dispose();
				this.mouseDownTimer = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x00150938 File Offset: 0x0014F938
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.UnsubscribeAll();
			base.OnMouseDown(e);
			this.Scroll();
			this.MouseDownTimer.Interval = ToolStripScrollButton.AUTOSCROLL_PAUSE;
			this.MouseDownTimer.Tick += this.OnInitialAutoScrollMouseDown;
			this.MouseDownTimer.Enabled = true;
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x0015098B File Offset: 0x0014F98B
		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.UnsubscribeAll();
			base.OnMouseUp(e);
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x0015099A File Offset: 0x0014F99A
		protected override void OnMouseLeave(EventArgs e)
		{
			this.UnsubscribeAll();
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x001509A2 File Offset: 0x0014F9A2
		private void UnsubscribeAll()
		{
			this.MouseDownTimer.Enabled = false;
			this.MouseDownTimer.Tick -= this.OnInitialAutoScrollMouseDown;
			this.MouseDownTimer.Tick -= this.OnAutoScrollAccellerate;
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x001509DE File Offset: 0x0014F9DE
		private void OnAutoScrollAccellerate(object sender, EventArgs e)
		{
			this.Scroll();
		}

		// Token: 0x06005C7F RID: 23679 RVA: 0x001509E8 File Offset: 0x0014F9E8
		private void OnInitialAutoScrollMouseDown(object sender, EventArgs e)
		{
			this.MouseDownTimer.Tick -= this.OnInitialAutoScrollMouseDown;
			this.Scroll();
			this.MouseDownTimer.Interval = 50;
			this.MouseDownTimer.Tick += this.OnAutoScrollAccellerate;
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x00150A38 File Offset: 0x0014FA38
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size empty = Size.Empty;
			empty.Height = ((this.Label.Image != null) ? (this.Label.Image.Height + 4) : 0);
			empty.Width = ((base.ParentInternal != null) ? (base.ParentInternal.Width - 2) : empty.Width);
			return empty;
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x00150A9C File Offset: 0x0014FA9C
		private void Scroll()
		{
			ToolStripDropDownMenu toolStripDropDownMenu = base.ParentInternal as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null && this.Label.Enabled)
			{
				toolStripDropDownMenu.ScrollInternal(this.up);
			}
		}

		// Token: 0x0400391B RID: 14619
		private const int AUTOSCROLL_UPDATE = 50;

		// Token: 0x0400391C RID: 14620
		private bool up = true;

		// Token: 0x0400391D RID: 14621
		[ThreadStatic]
		private static Bitmap upScrollImage;

		// Token: 0x0400391E RID: 14622
		[ThreadStatic]
		private static Bitmap downScrollImage;

		// Token: 0x0400391F RID: 14623
		private static readonly int AUTOSCROLL_PAUSE = SystemInformation.DoubleClickTime;

		// Token: 0x04003920 RID: 14624
		private Timer mouseDownTimer;

		// Token: 0x020006D9 RID: 1753
		internal class StickyLabel : Label
		{
			// Token: 0x1700135C RID: 4956
			// (get) Token: 0x06005C84 RID: 23684 RVA: 0x00150AE5 File Offset: 0x0014FAE5
			public bool FreezeLocationChange
			{
				get
				{
					return this.freezeLocationChange;
				}
			}

			// Token: 0x06005C85 RID: 23685 RVA: 0x00150AED File Offset: 0x0014FAED
			protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
			{
				if ((specified & BoundsSpecified.Location) != BoundsSpecified.None && this.FreezeLocationChange)
				{
					return;
				}
				base.SetBoundsCore(x, y, width, height, specified);
			}

			// Token: 0x06005C86 RID: 23686 RVA: 0x00150B0B File Offset: 0x0014FB0B
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override void WndProc(ref Message m)
			{
				if (m.Msg >= 256 && m.Msg <= 264)
				{
					this.DefWndProc(ref m);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x04003921 RID: 14625
			private bool freezeLocationChange;
		}
	}
}
