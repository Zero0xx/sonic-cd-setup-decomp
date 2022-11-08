using System;

namespace System.Security.Policy
{
	// Token: 0x020004A6 RID: 1190
	internal interface IDelayEvaluatedEvidence
	{
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002F32 RID: 12082
		bool IsVerified { get; }

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002F33 RID: 12083
		bool WasUsed { get; }

		// Token: 0x06002F34 RID: 12084
		void MarkUsed();
	}
}
