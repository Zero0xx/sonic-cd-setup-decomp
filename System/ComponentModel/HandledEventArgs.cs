using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000E8 RID: 232
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class HandledEventArgs : EventArgs
	{
		// Token: 0x060007CE RID: 1998 RVA: 0x0001BEC9 File Offset: 0x0001AEC9
		public HandledEventArgs() : this(false)
		{
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0001BED2 File Offset: 0x0001AED2
		public HandledEventArgs(bool defaultHandledValue)
		{
			this.handled = defaultHandledValue;
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001BEE1 File Offset: 0x0001AEE1
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x0001BEE9 File Offset: 0x0001AEE9
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		// Token: 0x0400097B RID: 2427
		private bool handled;
	}
}
