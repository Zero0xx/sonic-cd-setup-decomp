using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B9 RID: 1721
	[ComVisible(true)]
	public interface IServerChannelSinkStack : IServerResponseChannelSinkStack
	{
		// Token: 0x06003DFF RID: 15871
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Push(IServerChannelSink sink, object state);

		// Token: 0x06003E00 RID: 15872
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object Pop(IServerChannelSink sink);

		// Token: 0x06003E01 RID: 15873
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Store(IServerChannelSink sink, object state);

		// Token: 0x06003E02 RID: 15874
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void StoreAndDispatch(IServerChannelSink sink, object state);

		// Token: 0x06003E03 RID: 15875
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void ServerCallback(IAsyncResult ar);
	}
}
