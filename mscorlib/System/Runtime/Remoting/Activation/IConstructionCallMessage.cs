using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006E2 RID: 1762
	[ComVisible(true)]
	public interface IConstructionCallMessage : IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003F37 RID: 16183
		// (set) Token: 0x06003F38 RID: 16184
		IActivator Activator { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003F39 RID: 16185
		object[] CallSiteActivationAttributes { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003F3A RID: 16186
		string ActivationTypeName { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003F3B RID: 16187
		Type ActivationType { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003F3C RID: 16188
		IList ContextProperties { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }
	}
}
