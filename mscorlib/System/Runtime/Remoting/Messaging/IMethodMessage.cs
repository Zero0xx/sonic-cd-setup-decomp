using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006E0 RID: 1760
	[ComVisible(true)]
	public interface IMethodMessage : IMessage
	{
		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003F28 RID: 16168
		string Uri { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003F29 RID: 16169
		string MethodName { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003F2A RID: 16170
		string TypeName { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003F2B RID: 16171
		object MethodSignature { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003F2C RID: 16172
		int ArgCount { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x06003F2D RID: 16173
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string GetArgName(int index);

		// Token: 0x06003F2E RID: 16174
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object GetArg(int argNum);

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06003F2F RID: 16175
		object[] Args { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003F30 RID: 16176
		bool HasVarArgs { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003F31 RID: 16177
		LogicalCallContext LogicalCallContext { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003F32 RID: 16178
		MethodBase MethodBase { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }
	}
}
