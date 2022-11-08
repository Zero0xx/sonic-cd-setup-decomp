using System;

namespace System.Threading
{
	// Token: 0x0200016C RID: 364
	[Serializable]
	internal enum StackCrawlMark
	{
		// Token: 0x040006AB RID: 1707
		LookForMe,
		// Token: 0x040006AC RID: 1708
		LookForMyCaller,
		// Token: 0x040006AD RID: 1709
		LookForMyCallersCaller,
		// Token: 0x040006AE RID: 1710
		LookForThread
	}
}
