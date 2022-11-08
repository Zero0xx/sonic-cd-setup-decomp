using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006E5 RID: 1765
	[ComVisible(true)]
	public interface IChannelReceiverHook
	{
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003F43 RID: 16195
		string ChannelScheme { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06003F44 RID: 16196
		bool WantsToListen { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06003F45 RID: 16197
		IServerChannelSink ChannelSinkChain { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x06003F46 RID: 16198
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void AddHookChannelUri(string channelUri);
	}
}
