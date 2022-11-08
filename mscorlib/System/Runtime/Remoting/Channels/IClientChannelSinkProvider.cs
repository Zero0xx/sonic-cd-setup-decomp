using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006E6 RID: 1766
	[ComVisible(true)]
	public interface IClientChannelSinkProvider
	{
		// Token: 0x06003F47 RID: 16199
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData);

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06003F48 RID: 16200
		// (set) Token: 0x06003F49 RID: 16201
		IClientChannelSinkProvider Next { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }
	}
}
