using System;

namespace System.Net
{
	// Token: 0x020004E9 RID: 1257
	[Flags]
	internal enum ThreadKinds
	{
		// Token: 0x0400269F RID: 9887
		Unknown = 0,
		// Token: 0x040026A0 RID: 9888
		User = 1,
		// Token: 0x040026A1 RID: 9889
		System = 2,
		// Token: 0x040026A2 RID: 9890
		Sync = 4,
		// Token: 0x040026A3 RID: 9891
		Async = 8,
		// Token: 0x040026A4 RID: 9892
		Timer = 16,
		// Token: 0x040026A5 RID: 9893
		CompletionPort = 32,
		// Token: 0x040026A6 RID: 9894
		Worker = 64,
		// Token: 0x040026A7 RID: 9895
		Finalization = 128,
		// Token: 0x040026A8 RID: 9896
		Other = 256,
		// Token: 0x040026A9 RID: 9897
		OwnerMask = 3,
		// Token: 0x040026AA RID: 9898
		SyncMask = 12,
		// Token: 0x040026AB RID: 9899
		SourceMask = 496,
		// Token: 0x040026AC RID: 9900
		SafeSources = 352,
		// Token: 0x040026AD RID: 9901
		ThreadPool = 96
	}
}
