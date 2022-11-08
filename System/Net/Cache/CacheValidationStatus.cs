using System;

namespace System.Net.Cache
{
	// Token: 0x0200056F RID: 1391
	internal enum CacheValidationStatus
	{
		// Token: 0x0400292B RID: 10539
		DoNotUseCache,
		// Token: 0x0400292C RID: 10540
		Fail,
		// Token: 0x0400292D RID: 10541
		DoNotTakeFromCache,
		// Token: 0x0400292E RID: 10542
		RetryResponseFromCache,
		// Token: 0x0400292F RID: 10543
		RetryResponseFromServer,
		// Token: 0x04002930 RID: 10544
		ReturnCachedResponse,
		// Token: 0x04002931 RID: 10545
		CombineCachedAndServerResponse,
		// Token: 0x04002932 RID: 10546
		CacheResponse,
		// Token: 0x04002933 RID: 10547
		UpdateResponseInformation,
		// Token: 0x04002934 RID: 10548
		RemoveFromCache,
		// Token: 0x04002935 RID: 10549
		DoNotUpdateCache,
		// Token: 0x04002936 RID: 10550
		Continue
	}
}
