using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006F7 RID: 1783
	[ComVisible(true)]
	public interface IContributeDynamicSink
	{
		// Token: 0x06003F92 RID: 16274
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IDynamicMessageSink GetDynamicSink();
	}
}
