using System;

namespace System.Reflection.Cache
{
	// Token: 0x02000859 RID: 2137
	[Serializable]
	internal enum CacheAction
	{
		// Token: 0x04002869 RID: 10345
		AllocateCache = 1,
		// Token: 0x0400286A RID: 10346
		AddItem,
		// Token: 0x0400286B RID: 10347
		ClearCache,
		// Token: 0x0400286C RID: 10348
		LookupItemHit,
		// Token: 0x0400286D RID: 10349
		LookupItemMiss,
		// Token: 0x0400286E RID: 10350
		GrowCache,
		// Token: 0x0400286F RID: 10351
		SetItemReplace,
		// Token: 0x04002870 RID: 10352
		ReplaceFailed
	}
}
