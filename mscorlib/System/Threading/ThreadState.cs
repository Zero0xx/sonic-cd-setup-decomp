using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x0200017B RID: 379
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum ThreadState
	{
		// Token: 0x040006CF RID: 1743
		Running = 0,
		// Token: 0x040006D0 RID: 1744
		StopRequested = 1,
		// Token: 0x040006D1 RID: 1745
		SuspendRequested = 2,
		// Token: 0x040006D2 RID: 1746
		Background = 4,
		// Token: 0x040006D3 RID: 1747
		Unstarted = 8,
		// Token: 0x040006D4 RID: 1748
		Stopped = 16,
		// Token: 0x040006D5 RID: 1749
		WaitSleepJoin = 32,
		// Token: 0x040006D6 RID: 1750
		Suspended = 64,
		// Token: 0x040006D7 RID: 1751
		AbortRequested = 128,
		// Token: 0x040006D8 RID: 1752
		Aborted = 256
	}
}
