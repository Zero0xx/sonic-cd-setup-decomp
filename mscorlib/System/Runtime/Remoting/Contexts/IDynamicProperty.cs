using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006FB RID: 1787
	[ComVisible(true)]
	public interface IDynamicProperty
	{
		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003F96 RID: 16278
		string Name { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }
	}
}
