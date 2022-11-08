using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000DB RID: 219
	// (Invoke) Token: 0x0600075B RID: 1883
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void DoWorkEventHandler(object sender, DoWorkEventArgs e);
}
