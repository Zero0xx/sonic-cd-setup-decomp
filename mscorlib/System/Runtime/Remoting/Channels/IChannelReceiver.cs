using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006CE RID: 1742
	[ComVisible(true)]
	public interface IChannelReceiver : IChannel
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003ED0 RID: 16080
		object ChannelData { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x06003ED1 RID: 16081
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string[] GetUrlsForUri(string objectURI);

		// Token: 0x06003ED2 RID: 16082
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void StartListening(object data);

		// Token: 0x06003ED3 RID: 16083
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void StopListening(object data);
	}
}
