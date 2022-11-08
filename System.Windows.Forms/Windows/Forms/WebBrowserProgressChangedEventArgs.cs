using System;

namespace System.Windows.Forms
{
	// Token: 0x02000739 RID: 1849
	public class WebBrowserProgressChangedEventArgs : EventArgs
	{
		// Token: 0x06006296 RID: 25238 RVA: 0x0016684F File Offset: 0x0016584F
		public WebBrowserProgressChangedEventArgs(long currentProgress, long maximumProgress)
		{
			this.currentProgress = currentProgress;
			this.maximumProgress = maximumProgress;
		}

		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x06006297 RID: 25239 RVA: 0x00166865 File Offset: 0x00165865
		public long CurrentProgress
		{
			get
			{
				return this.currentProgress;
			}
		}

		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x06006298 RID: 25240 RVA: 0x0016686D File Offset: 0x0016586D
		public long MaximumProgress
		{
			get
			{
				return this.maximumProgress;
			}
		}

		// Token: 0x04003B20 RID: 15136
		private long currentProgress;

		// Token: 0x04003B21 RID: 15137
		private long maximumProgress;
	}
}
