using System;

namespace System.Diagnostics
{
	// Token: 0x02000798 RID: 1944
	public enum ThreadPriorityLevel
	{
		// Token: 0x04003495 RID: 13461
		Idle = -15,
		// Token: 0x04003496 RID: 13462
		Lowest = -2,
		// Token: 0x04003497 RID: 13463
		BelowNormal,
		// Token: 0x04003498 RID: 13464
		Normal,
		// Token: 0x04003499 RID: 13465
		AboveNormal,
		// Token: 0x0400349A RID: 13466
		Highest,
		// Token: 0x0400349B RID: 13467
		TimeCritical = 15
	}
}
