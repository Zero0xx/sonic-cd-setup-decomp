using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x020007A7 RID: 1959
	[ComVisible(true)]
	public interface ITrackingHandler
	{
		// Token: 0x060045AB RID: 17835
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void MarshaledObject(object obj, ObjRef or);

		// Token: 0x060045AC RID: 17836
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void UnmarshaledObject(object obj, ObjRef or);

		// Token: 0x060045AD RID: 17837
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void DisconnectedObject(object obj);
	}
}
