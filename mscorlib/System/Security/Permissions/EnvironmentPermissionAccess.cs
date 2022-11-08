using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000622 RID: 1570
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum EnvironmentPermissionAccess
	{
		// Token: 0x04001D7E RID: 7550
		NoAccess = 0,
		// Token: 0x04001D7F RID: 7551
		Read = 1,
		// Token: 0x04001D80 RID: 7552
		Write = 2,
		// Token: 0x04001D81 RID: 7553
		AllAccess = 3
	}
}
