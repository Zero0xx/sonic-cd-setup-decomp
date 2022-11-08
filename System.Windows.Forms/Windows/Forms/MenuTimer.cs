using System;

namespace System.Windows.Forms
{
	// Token: 0x020006B6 RID: 1718
	internal class MenuTimer
	{
		// Token: 0x06005A09 RID: 23049 RVA: 0x00147234 File Offset: 0x00146234
		public MenuTimer()
		{
			this.autoMenuExpandTimer.Tick += this.OnTick;
			this.slowShow = Math.Max(this.quickShow, SystemInformation.MenuShowDelay);
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06005A0A RID: 23050 RVA: 0x00147286 File Offset: 0x00146286
		// (set) Token: 0x06005A0B RID: 23051 RVA: 0x0014728E File Offset: 0x0014628E
		private ToolStripMenuItem CurrentItem
		{
			get
			{
				return this.currentItem;
			}
			set
			{
				this.currentItem = value;
			}
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06005A0C RID: 23052 RVA: 0x00147297 File Offset: 0x00146297
		// (set) Token: 0x06005A0D RID: 23053 RVA: 0x0014729F File Offset: 0x0014629F
		public bool InTransition
		{
			get
			{
				return this.inTransition;
			}
			set
			{
				this.inTransition = value;
			}
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x001472A8 File Offset: 0x001462A8
		public void Start(ToolStripMenuItem item)
		{
			if (this.InTransition)
			{
				return;
			}
			this.StartCore(item);
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x001472BC File Offset: 0x001462BC
		private void StartCore(ToolStripMenuItem item)
		{
			if (item != this.CurrentItem)
			{
				this.Cancel(this.CurrentItem);
			}
			this.CurrentItem = item;
			if (item != null)
			{
				this.CurrentItem = item;
				this.autoMenuExpandTimer.Interval = (item.IsOnDropDown ? this.slowShow : this.quickShow);
				this.autoMenuExpandTimer.Enabled = true;
			}
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x0014731C File Offset: 0x0014631C
		public void Transition(ToolStripMenuItem fromItem, ToolStripMenuItem toItem)
		{
			if (toItem == null && this.InTransition)
			{
				this.Cancel();
				this.EndTransition(true);
				return;
			}
			if (this.fromItem != fromItem)
			{
				this.fromItem = fromItem;
				this.CancelCore();
				this.StartCore(toItem);
			}
			this.CurrentItem = toItem;
			this.InTransition = true;
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x0014736D File Offset: 0x0014636D
		public void Cancel()
		{
			if (this.InTransition)
			{
				return;
			}
			this.CancelCore();
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x0014737E File Offset: 0x0014637E
		public void Cancel(ToolStripMenuItem item)
		{
			if (this.InTransition)
			{
				return;
			}
			if (item == this.CurrentItem)
			{
				this.CancelCore();
			}
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x00147398 File Offset: 0x00146398
		private void CancelCore()
		{
			this.autoMenuExpandTimer.Enabled = false;
			this.CurrentItem = null;
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x001473B0 File Offset: 0x001463B0
		private void EndTransition(bool forceClose)
		{
			ToolStripMenuItem toolStripMenuItem = this.fromItem;
			this.fromItem = null;
			if (this.InTransition)
			{
				this.InTransition = false;
				bool flag = forceClose || (this.CurrentItem != null && this.CurrentItem != toolStripMenuItem && this.CurrentItem.Selected);
				if (flag && toolStripMenuItem != null && toolStripMenuItem.HasDropDownItems)
				{
					toolStripMenuItem.HideDropDown();
				}
			}
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x00147414 File Offset: 0x00146414
		internal void HandleToolStripMouseLeave(ToolStrip toolStrip)
		{
			if (this.InTransition && toolStrip == this.fromItem.ParentInternal)
			{
				if (this.CurrentItem != null)
				{
					this.CurrentItem.Select();
					return;
				}
			}
			else if (toolStrip.IsDropDown && toolStrip.ActiveDropDowns.Count > 0)
			{
				ToolStripDropDown toolStripDropDown = toolStrip.ActiveDropDowns[0] as ToolStripDropDown;
				ToolStripMenuItem toolStripMenuItem = (toolStripDropDown == null) ? null : (toolStripDropDown.OwnerItem as ToolStripMenuItem);
				if (toolStripMenuItem != null && toolStripMenuItem.Pressed)
				{
					toolStripMenuItem.Select();
				}
			}
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x00147498 File Offset: 0x00146498
		private void OnTick(object sender, EventArgs e)
		{
			this.autoMenuExpandTimer.Enabled = false;
			if (this.CurrentItem == null)
			{
				return;
			}
			this.EndTransition(false);
			if (this.CurrentItem != null && !this.CurrentItem.IsDisposed && this.CurrentItem.Selected && ToolStripManager.ModalMenuFilter.InMenuMode)
			{
				this.CurrentItem.OnMenuAutoExpand();
			}
		}

		// Token: 0x040038AA RID: 14506
		private Timer autoMenuExpandTimer = new Timer();

		// Token: 0x040038AB RID: 14507
		private ToolStripMenuItem currentItem;

		// Token: 0x040038AC RID: 14508
		private ToolStripMenuItem fromItem;

		// Token: 0x040038AD RID: 14509
		private bool inTransition;

		// Token: 0x040038AE RID: 14510
		private int quickShow = 1;

		// Token: 0x040038AF RID: 14511
		private int slowShow;
	}
}
