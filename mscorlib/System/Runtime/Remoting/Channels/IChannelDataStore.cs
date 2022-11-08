using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006EC RID: 1772
	[ComVisible(true)]
	public interface IChannelDataStore
	{
		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003F4F RID: 16207
		string[] ChannelUris { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000AA6 RID: 2726
		object this[object key]
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}
	}
}
