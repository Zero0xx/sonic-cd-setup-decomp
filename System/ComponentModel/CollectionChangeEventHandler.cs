using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000B3 RID: 179
	// (Invoke) Token: 0x06000659 RID: 1625
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void CollectionChangeEventHandler(object sender, CollectionChangeEventArgs e);
}
