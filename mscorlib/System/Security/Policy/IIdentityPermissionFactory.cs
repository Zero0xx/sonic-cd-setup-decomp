using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x020004A5 RID: 1189
	[ComVisible(true)]
	public interface IIdentityPermissionFactory
	{
		// Token: 0x06002F31 RID: 12081
		IPermission CreateIdentityPermission(Evidence evidence);
	}
}
