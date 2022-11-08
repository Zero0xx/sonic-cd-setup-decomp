using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006FA RID: 1786
	[ComVisible(true)]
	public interface IContributeServerContextSink
	{
		// Token: 0x06003F95 RID: 16277
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessageSink GetServerContextSink(IMessageSink nextSink);
	}
}
