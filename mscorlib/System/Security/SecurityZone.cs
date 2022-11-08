using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x02000696 RID: 1686
	[ComVisible(true)]
	[Serializable]
	public enum SecurityZone
	{
		// Token: 0x04001F56 RID: 8022
		MyComputer,
		// Token: 0x04001F57 RID: 8023
		Intranet,
		// Token: 0x04001F58 RID: 8024
		Trusted,
		// Token: 0x04001F59 RID: 8025
		Internet,
		// Token: 0x04001F5A RID: 8026
		Untrusted,
		// Token: 0x04001F5B RID: 8027
		NoZone = -1
	}
}
