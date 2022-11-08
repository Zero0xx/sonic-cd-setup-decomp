using System;

namespace System.Diagnostics
{
	// Token: 0x0200079A RID: 1946
	public enum ThreadWaitReason
	{
		// Token: 0x040034A6 RID: 13478
		Executive,
		// Token: 0x040034A7 RID: 13479
		FreePage,
		// Token: 0x040034A8 RID: 13480
		PageIn,
		// Token: 0x040034A9 RID: 13481
		SystemAllocation,
		// Token: 0x040034AA RID: 13482
		ExecutionDelay,
		// Token: 0x040034AB RID: 13483
		Suspended,
		// Token: 0x040034AC RID: 13484
		UserRequest,
		// Token: 0x040034AD RID: 13485
		EventPairHigh,
		// Token: 0x040034AE RID: 13486
		EventPairLow,
		// Token: 0x040034AF RID: 13487
		LpcReceive,
		// Token: 0x040034B0 RID: 13488
		LpcReply,
		// Token: 0x040034B1 RID: 13489
		VirtualMemory,
		// Token: 0x040034B2 RID: 13490
		PageOut,
		// Token: 0x040034B3 RID: 13491
		Unknown
	}
}
