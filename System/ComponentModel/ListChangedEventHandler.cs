using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000113 RID: 275
	// (Invoke) Token: 0x06000888 RID: 2184
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs e);
}
