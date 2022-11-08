using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006CD RID: 1741
	[ComVisible(true)]
	public interface IChannelSender : IChannel
	{
		// Token: 0x06003ECF RID: 16079
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessageSink CreateMessageSink(string url, object remoteChannelData, out string objectURI);
	}
}
