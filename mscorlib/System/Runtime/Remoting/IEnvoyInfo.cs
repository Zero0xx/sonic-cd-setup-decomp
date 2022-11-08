using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x02000732 RID: 1842
	[ComVisible(true)]
	public interface IEnvoyInfo
	{
		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060041F6 RID: 16886
		// (set) Token: 0x060041F7 RID: 16887
		IMessageSink EnvoySinks { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }
	}
}
