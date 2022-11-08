using System;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000701 RID: 1793
	internal interface IInternalMessage
	{
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06003FD5 RID: 16341
		// (set) Token: 0x06003FD6 RID: 16342
		ServerIdentity ServerIdentityObject { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06003FD7 RID: 16343
		// (set) Token: 0x06003FD8 RID: 16344
		Identity IdentityObject { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }

		// Token: 0x06003FD9 RID: 16345
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void SetURI(string uri);

		// Token: 0x06003FDA RID: 16346
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void SetCallContext(LogicalCallContext callContext);

		// Token: 0x06003FDB RID: 16347
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool HasProperties();
	}
}
