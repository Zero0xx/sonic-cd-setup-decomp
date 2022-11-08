using System;

namespace System.Diagnostics
{
	// Token: 0x02000776 RID: 1910
	internal class ThreadInfo
	{
		// Token: 0x040033B9 RID: 13241
		public int threadId;

		// Token: 0x040033BA RID: 13242
		public int processId;

		// Token: 0x040033BB RID: 13243
		public int basePriority;

		// Token: 0x040033BC RID: 13244
		public int currentPriority;

		// Token: 0x040033BD RID: 13245
		public IntPtr startAddress;

		// Token: 0x040033BE RID: 13246
		public ThreadState threadState;

		// Token: 0x040033BF RID: 13247
		public ThreadWaitReason threadWaitReason;
	}
}
