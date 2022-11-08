using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000605 RID: 1541
	internal struct MibTcpRow
	{
		// Token: 0x04002D92 RID: 11666
		internal TcpState state;

		// Token: 0x04002D93 RID: 11667
		internal uint localAddr;

		// Token: 0x04002D94 RID: 11668
		internal byte localPort1;

		// Token: 0x04002D95 RID: 11669
		internal byte localPort2;

		// Token: 0x04002D96 RID: 11670
		internal byte localPort3;

		// Token: 0x04002D97 RID: 11671
		internal byte localPort4;

		// Token: 0x04002D98 RID: 11672
		internal uint remoteAddr;

		// Token: 0x04002D99 RID: 11673
		internal byte remotePort1;

		// Token: 0x04002D9A RID: 11674
		internal byte remotePort2;

		// Token: 0x04002D9B RID: 11675
		internal byte remotePort3;

		// Token: 0x04002D9C RID: 11676
		internal byte remotePort4;
	}
}
