using System;

namespace System.Net.Cache
{
	// Token: 0x02000569 RID: 1385
	public enum RequestCacheLevel
	{
		// Token: 0x04002907 RID: 10503
		Default,
		// Token: 0x04002908 RID: 10504
		BypassCache,
		// Token: 0x04002909 RID: 10505
		CacheOnly,
		// Token: 0x0400290A RID: 10506
		CacheIfAvailable,
		// Token: 0x0400290B RID: 10507
		Revalidate,
		// Token: 0x0400290C RID: 10508
		Reload,
		// Token: 0x0400290D RID: 10509
		NoCacheNoStore
	}
}
