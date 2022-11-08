using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200069B RID: 1691
	[ComVisible(true)]
	public interface IContextProperty
	{
		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06003D38 RID: 15672
		string Name { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x06003D39 RID: 15673
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool IsNewContextOK(Context newCtx);

		// Token: 0x06003D3A RID: 15674
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Freeze(Context newContext);
	}
}
