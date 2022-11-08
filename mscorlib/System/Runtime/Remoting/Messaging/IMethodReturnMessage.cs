using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006E3 RID: 1763
	[ComVisible(true)]
	public interface IMethodReturnMessage : IMethodMessage, IMessage
	{
		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003F3D RID: 16189
		int OutArgCount { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x06003F3E RID: 16190
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string GetOutArgName(int index);

		// Token: 0x06003F3F RID: 16191
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object GetOutArg(int argNum);

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003F40 RID: 16192
		object[] OutArgs { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003F41 RID: 16193
		Exception Exception { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003F42 RID: 16194
		object ReturnValue { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }
	}
}
