using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000E9 RID: 233
	// (Invoke) Token: 0x060007D3 RID: 2003
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void HandledEventHandler(object sender, HandledEventArgs e);
}
