using System;

namespace System.Net.Cache
{
	// Token: 0x0200056B RID: 1387
	public enum HttpRequestCacheLevel
	{
		// Token: 0x04002910 RID: 10512
		Default,
		// Token: 0x04002911 RID: 10513
		BypassCache,
		// Token: 0x04002912 RID: 10514
		CacheOnly,
		// Token: 0x04002913 RID: 10515
		CacheIfAvailable,
		// Token: 0x04002914 RID: 10516
		Revalidate,
		// Token: 0x04002915 RID: 10517
		Reload,
		// Token: 0x04002916 RID: 10518
		NoCacheNoStore,
		// Token: 0x04002917 RID: 10519
		CacheOrNextCacheOnly,
		// Token: 0x04002918 RID: 10520
		Refresh
	}
}
