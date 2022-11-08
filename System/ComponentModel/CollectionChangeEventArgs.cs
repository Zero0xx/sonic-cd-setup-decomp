using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000B2 RID: 178
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CollectionChangeEventArgs : EventArgs
	{
		// Token: 0x06000655 RID: 1621 RVA: 0x00018612 File Offset: 0x00017612
		public CollectionChangeEventArgs(CollectionChangeAction action, object element)
		{
			this.action = action;
			this.element = element;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00018628 File Offset: 0x00017628
		public virtual CollectionChangeAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00018630 File Offset: 0x00017630
		public virtual object Element
		{
			get
			{
				return this.element;
			}
		}

		// Token: 0x04000910 RID: 2320
		private CollectionChangeAction action;

		// Token: 0x04000911 RID: 2321
		private object element;
	}
}
