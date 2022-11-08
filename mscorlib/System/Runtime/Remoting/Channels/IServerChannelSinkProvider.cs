using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006D4 RID: 1748
	[ComVisible(true)]
	public interface IServerChannelSinkProvider
	{
		// Token: 0x06003EFE RID: 16126
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void GetChannelData(IChannelDataStore channelData);

		// Token: 0x06003EFF RID: 16127
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IServerChannelSink CreateSink(IChannelReceiver channel);

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003F00 RID: 16128
		// (set) Token: 0x06003F01 RID: 16129
		IServerChannelSinkProvider Next { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }
	}
}
