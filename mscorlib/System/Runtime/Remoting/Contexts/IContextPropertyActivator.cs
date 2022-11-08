using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006C8 RID: 1736
	[ComVisible(true)]
	public interface IContextPropertyActivator
	{
		// Token: 0x06003EAD RID: 16045
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool IsOKToActivate(IConstructionCallMessage msg);

		// Token: 0x06003EAE RID: 16046
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void CollectFromClientContext(IConstructionCallMessage msg);

		// Token: 0x06003EAF RID: 16047
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool DeliverClientContextToServerContext(IConstructionCallMessage msg);

		// Token: 0x06003EB0 RID: 16048
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void CollectFromServerContext(IConstructionReturnMessage msg);

		// Token: 0x06003EB1 RID: 16049
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool DeliverServerContextToClientContext(IConstructionReturnMessage msg);
	}
}
