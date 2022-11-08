using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006F6 RID: 1782
	[ComVisible(true)]
	public interface IContributeClientContextSink
	{
		// Token: 0x06003F91 RID: 16273
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessageSink GetClientContextSink(IMessageSink nextSink);
	}
}
