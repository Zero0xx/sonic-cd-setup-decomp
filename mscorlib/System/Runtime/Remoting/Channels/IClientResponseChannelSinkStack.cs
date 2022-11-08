using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B4 RID: 1716
	[ComVisible(true)]
	public interface IClientResponseChannelSinkStack
	{
		// Token: 0x06003DF0 RID: 15856
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void AsyncProcessResponse(ITransportHeaders headers, Stream stream);

		// Token: 0x06003DF1 RID: 15857
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void DispatchReplyMessage(IMessage msg);

		// Token: 0x06003DF2 RID: 15858
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void DispatchException(Exception e);
	}
}
