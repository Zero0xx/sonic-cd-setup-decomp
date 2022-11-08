using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200069A RID: 1690
	[ComVisible(true)]
	public interface IContextAttribute
	{
		// Token: 0x06003D36 RID: 15670
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool IsContextOK(Context ctx, IConstructionCallMessage msg);

		// Token: 0x06003D37 RID: 15671
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void GetPropertiesForNewContext(IConstructionCallMessage msg);
	}
}
