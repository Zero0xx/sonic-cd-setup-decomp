using System;

namespace System.Net.Sockets
{
	// Token: 0x020005BD RID: 1469
	public enum SocketAsyncOperation
	{
		// Token: 0x04002B48 RID: 11080
		None,
		// Token: 0x04002B49 RID: 11081
		Accept,
		// Token: 0x04002B4A RID: 11082
		Connect,
		// Token: 0x04002B4B RID: 11083
		Disconnect,
		// Token: 0x04002B4C RID: 11084
		Receive,
		// Token: 0x04002B4D RID: 11085
		ReceiveFrom,
		// Token: 0x04002B4E RID: 11086
		ReceiveMessageFrom,
		// Token: 0x04002B4F RID: 11087
		Send,
		// Token: 0x04002B50 RID: 11088
		SendPackets,
		// Token: 0x04002B51 RID: 11089
		SendTo
	}
}
