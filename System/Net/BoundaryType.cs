using System;

namespace System.Net
{
	// Token: 0x020003A9 RID: 937
	internal enum BoundaryType
	{
		// Token: 0x04001D77 RID: 7543
		ContentLength,
		// Token: 0x04001D78 RID: 7544
		Chunked,
		// Token: 0x04001D79 RID: 7545
		Multipart = 3,
		// Token: 0x04001D7A RID: 7546
		None,
		// Token: 0x04001D7B RID: 7547
		Invalid
	}
}
