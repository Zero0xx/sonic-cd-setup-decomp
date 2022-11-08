using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000703 RID: 1795
	[ComVisible(true)]
	public interface IMessageCtrl
	{
		// Token: 0x06003FE8 RID: 16360
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Cancel(int msToCancel);
	}
}
