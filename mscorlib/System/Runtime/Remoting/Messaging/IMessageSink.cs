using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006A5 RID: 1701
	[ComVisible(true)]
	public interface IMessageSink
	{
		// Token: 0x06003D6E RID: 15726
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessage SyncProcessMessage(IMessage msg);

		// Token: 0x06003D6F RID: 15727
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink);

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06003D70 RID: 15728
		IMessageSink NextSink { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }
	}
}
