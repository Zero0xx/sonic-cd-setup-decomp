using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200069D RID: 1693
	[ComVisible(true)]
	public interface IActivator
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06003D43 RID: 15683
		// (set) Token: 0x06003D44 RID: 15684
		IActivator NextActivator { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }

		// Token: 0x06003D45 RID: 15685
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IConstructionReturnMessage Activate(IConstructionCallMessage msg);

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06003D46 RID: 15686
		ActivatorLevel Level { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }
	}
}
