using System;

namespace System.Windows.Forms
{
	// Token: 0x02000670 RID: 1648
	internal class MouseHoverTimer : IDisposable
	{
		// Token: 0x06005691 RID: 22161 RVA: 0x0013B05C File Offset: 0x0013A05C
		public MouseHoverTimer()
		{
			int num = SystemInformation.MouseHoverTime;
			if (num == 0)
			{
				num = 400;
			}
			this.mouseHoverTimer.Interval = num;
			this.mouseHoverTimer.Tick += this.OnTick;
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x0013B0AC File Offset: 0x0013A0AC
		public void Start(ToolStripItem item)
		{
			if (item != this.currentItem)
			{
				this.Cancel(this.currentItem);
			}
			this.currentItem = item;
			if (this.currentItem != null)
			{
				this.mouseHoverTimer.Enabled = true;
			}
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x0013B0DE File Offset: 0x0013A0DE
		public void Cancel()
		{
			this.mouseHoverTimer.Enabled = false;
			this.currentItem = null;
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x0013B0F3 File Offset: 0x0013A0F3
		public void Cancel(ToolStripItem item)
		{
			if (item == this.currentItem)
			{
				this.Cancel();
			}
		}

		// Token: 0x06005695 RID: 22165 RVA: 0x0013B104 File Offset: 0x0013A104
		public void Dispose()
		{
			if (this.mouseHoverTimer != null)
			{
				this.Cancel();
				this.mouseHoverTimer.Dispose();
				this.mouseHoverTimer = null;
			}
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x0013B126 File Offset: 0x0013A126
		private void OnTick(object sender, EventArgs e)
		{
			this.mouseHoverTimer.Enabled = false;
			if (this.currentItem != null && !this.currentItem.IsDisposed)
			{
				this.currentItem.FireEvent(EventArgs.Empty, ToolStripItemEventType.MouseHover);
			}
		}

		// Token: 0x0400376F RID: 14191
		private const int SPI_GETMOUSEHOVERTIME_WIN9X = 400;

		// Token: 0x04003770 RID: 14192
		private Timer mouseHoverTimer = new Timer();

		// Token: 0x04003771 RID: 14193
		private ToolStripItem currentItem;
	}
}
