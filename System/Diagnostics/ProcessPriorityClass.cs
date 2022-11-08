using System;

namespace System.Diagnostics
{
	// Token: 0x02000786 RID: 1926
	public enum ProcessPriorityClass
	{
		// Token: 0x0400341D RID: 13341
		Normal = 32,
		// Token: 0x0400341E RID: 13342
		Idle = 64,
		// Token: 0x0400341F RID: 13343
		High = 128,
		// Token: 0x04003420 RID: 13344
		RealTime = 256,
		// Token: 0x04003421 RID: 13345
		BelowNormal = 16384,
		// Token: 0x04003422 RID: 13346
		AboveNormal = 32768
	}
}
