using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000AE RID: 174
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CancelEventArgs : EventArgs
	{
		// Token: 0x06000649 RID: 1609 RVA: 0x00018515 File Offset: 0x00017515
		public CancelEventArgs() : this(false)
		{
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001851E File Offset: 0x0001751E
		public CancelEventArgs(bool cancel)
		{
			this.cancel = cancel;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001852D File Offset: 0x0001752D
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x00018535 File Offset: 0x00017535
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		// Token: 0x0400090B RID: 2315
		private bool cancel;
	}
}
