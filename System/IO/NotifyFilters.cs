using System;

namespace System.IO
{
	// Token: 0x02000723 RID: 1827
	[Flags]
	public enum NotifyFilters
	{
		// Token: 0x040031DF RID: 12767
		FileName = 1,
		// Token: 0x040031E0 RID: 12768
		DirectoryName = 2,
		// Token: 0x040031E1 RID: 12769
		Attributes = 4,
		// Token: 0x040031E2 RID: 12770
		Size = 8,
		// Token: 0x040031E3 RID: 12771
		LastWrite = 16,
		// Token: 0x040031E4 RID: 12772
		LastAccess = 32,
		// Token: 0x040031E5 RID: 12773
		CreationTime = 64,
		// Token: 0x040031E6 RID: 12774
		Security = 256
	}
}
