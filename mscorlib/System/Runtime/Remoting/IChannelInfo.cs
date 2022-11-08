using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x02000731 RID: 1841
	[ComVisible(true)]
	public interface IChannelInfo
	{
		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060041F4 RID: 16884
		// (set) Token: 0x060041F5 RID: 16885
		object[] ChannelData { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }
	}
}
