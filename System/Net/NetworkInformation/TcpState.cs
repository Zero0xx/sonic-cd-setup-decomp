using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063E RID: 1598
	public enum TcpState
	{
		// Token: 0x04002E7F RID: 11903
		Unknown,
		// Token: 0x04002E80 RID: 11904
		Closed,
		// Token: 0x04002E81 RID: 11905
		Listen,
		// Token: 0x04002E82 RID: 11906
		SynSent,
		// Token: 0x04002E83 RID: 11907
		SynReceived,
		// Token: 0x04002E84 RID: 11908
		Established,
		// Token: 0x04002E85 RID: 11909
		FinWait1,
		// Token: 0x04002E86 RID: 11910
		FinWait2,
		// Token: 0x04002E87 RID: 11911
		CloseWait,
		// Token: 0x04002E88 RID: 11912
		Closing,
		// Token: 0x04002E89 RID: 11913
		LastAck,
		// Token: 0x04002E8A RID: 11914
		TimeWait,
		// Token: 0x04002E8B RID: 11915
		DeleteTcb
	}
}
