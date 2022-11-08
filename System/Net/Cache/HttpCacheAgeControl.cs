using System;

namespace System.Net.Cache
{
	// Token: 0x0200056C RID: 1388
	public enum HttpCacheAgeControl
	{
		// Token: 0x0400291A RID: 10522
		None,
		// Token: 0x0400291B RID: 10523
		MinFresh,
		// Token: 0x0400291C RID: 10524
		MaxAge,
		// Token: 0x0400291D RID: 10525
		MaxStale = 4,
		// Token: 0x0400291E RID: 10526
		MaxAgeAndMinFresh = 3,
		// Token: 0x0400291F RID: 10527
		MaxAgeAndMaxStale = 6
	}
}
