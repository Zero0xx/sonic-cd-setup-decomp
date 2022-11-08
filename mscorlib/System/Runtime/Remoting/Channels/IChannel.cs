using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006CC RID: 1740
	[ComVisible(true)]
	public interface IChannel
	{
		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003ECC RID: 16076
		int ChannelPriority { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06003ECD RID: 16077
		string ChannelName { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x06003ECE RID: 16078
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string Parse(string url, out string objectURI);
	}
}
