using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000663 RID: 1635
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum RegistryPermissionAccess
	{
		// Token: 0x04001E88 RID: 7816
		NoAccess = 0,
		// Token: 0x04001E89 RID: 7817
		Read = 1,
		// Token: 0x04001E8A RID: 7818
		Write = 2,
		// Token: 0x04001E8B RID: 7819
		Create = 4,
		// Token: 0x04001E8C RID: 7820
		AllAccess = 7
	}
}
