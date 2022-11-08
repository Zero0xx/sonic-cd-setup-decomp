using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000AF RID: 175
	// (Invoke) Token: 0x0600064E RID: 1614
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void CancelEventHandler(object sender, CancelEventArgs e);
}
