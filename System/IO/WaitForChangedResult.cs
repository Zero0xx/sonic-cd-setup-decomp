using System;

namespace System.IO
{
	// Token: 0x02000730 RID: 1840
	public struct WaitForChangedResult
	{
		// Token: 0x06003821 RID: 14369 RVA: 0x000ED2A5 File Offset: 0x000EC2A5
		internal WaitForChangedResult(WatcherChangeTypes changeType, string name, bool timedOut)
		{
			this = new WaitForChangedResult(changeType, name, null, timedOut);
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000ED2B1 File Offset: 0x000EC2B1
		internal WaitForChangedResult(WatcherChangeTypes changeType, string name, string oldName, bool timedOut)
		{
			this.changeType = changeType;
			this.name = name;
			this.oldName = oldName;
			this.timedOut = timedOut;
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003823 RID: 14371 RVA: 0x000ED2D0 File Offset: 0x000EC2D0
		// (set) Token: 0x06003824 RID: 14372 RVA: 0x000ED2D8 File Offset: 0x000EC2D8
		public WatcherChangeTypes ChangeType
		{
			get
			{
				return this.changeType;
			}
			set
			{
				this.changeType = value;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003825 RID: 14373 RVA: 0x000ED2E1 File Offset: 0x000EC2E1
		// (set) Token: 0x06003826 RID: 14374 RVA: 0x000ED2E9 File Offset: 0x000EC2E9
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003827 RID: 14375 RVA: 0x000ED2F2 File Offset: 0x000EC2F2
		// (set) Token: 0x06003828 RID: 14376 RVA: 0x000ED2FA File Offset: 0x000EC2FA
		public string OldName
		{
			get
			{
				return this.oldName;
			}
			set
			{
				this.oldName = value;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003829 RID: 14377 RVA: 0x000ED303 File Offset: 0x000EC303
		// (set) Token: 0x0600382A RID: 14378 RVA: 0x000ED30B File Offset: 0x000EC30B
		public bool TimedOut
		{
			get
			{
				return this.timedOut;
			}
			set
			{
				this.timedOut = value;
			}
		}

		// Token: 0x04003219 RID: 12825
		private WatcherChangeTypes changeType;

		// Token: 0x0400321A RID: 12826
		private string name;

		// Token: 0x0400321B RID: 12827
		private string oldName;

		// Token: 0x0400321C RID: 12828
		private bool timedOut;

		// Token: 0x0400321D RID: 12829
		internal static readonly WaitForChangedResult TimedOutResult = new WaitForChangedResult((WatcherChangeTypes)0, null, true);
	}
}
