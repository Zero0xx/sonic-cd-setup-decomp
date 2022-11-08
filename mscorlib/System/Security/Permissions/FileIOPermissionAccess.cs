using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200062C RID: 1580
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum FileIOPermissionAccess
	{
		// Token: 0x04001D8C RID: 7564
		NoAccess = 0,
		// Token: 0x04001D8D RID: 7565
		Read = 1,
		// Token: 0x04001D8E RID: 7566
		Write = 2,
		// Token: 0x04001D8F RID: 7567
		Append = 4,
		// Token: 0x04001D90 RID: 7568
		PathDiscovery = 8,
		// Token: 0x04001D91 RID: 7569
		AllAccess = 15
	}
}
