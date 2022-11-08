using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x02000730 RID: 1840
	[ComVisible(true)]
	public interface IRemotingTypeInfo
	{
		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060041F1 RID: 16881
		// (set) Token: 0x060041F2 RID: 16882
		string TypeName { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }

		// Token: 0x060041F3 RID: 16883
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool CanCastTo(Type fromType, object o);
	}
}
