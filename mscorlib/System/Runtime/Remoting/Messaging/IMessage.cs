using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006DF RID: 1759
	[ComVisible(true)]
	public interface IMessage
	{
		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003F27 RID: 16167
		IDictionary Properties { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }
	}
}
