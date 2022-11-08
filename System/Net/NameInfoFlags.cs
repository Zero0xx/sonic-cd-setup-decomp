using System;

namespace System.Net
{
	// Token: 0x02000500 RID: 1280
	[Flags]
	internal enum NameInfoFlags
	{
		// Token: 0x04002728 RID: 10024
		NI_NOFQDN = 1,
		// Token: 0x04002729 RID: 10025
		NI_NUMERICHOST = 2,
		// Token: 0x0400272A RID: 10026
		NI_NAMEREQD = 4,
		// Token: 0x0400272B RID: 10027
		NI_NUMERICSERV = 8,
		// Token: 0x0400272C RID: 10028
		NI_DGRAM = 16
	}
}
