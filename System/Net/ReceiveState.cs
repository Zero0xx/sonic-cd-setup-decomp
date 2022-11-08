using System;

namespace System.Net
{
	// Token: 0x020004BD RID: 1213
	internal class ReceiveState
	{
		// Token: 0x060025A8 RID: 9640 RVA: 0x0009605B File Offset: 0x0009505B
		internal ReceiveState(CommandStream connection)
		{
			this.Connection = connection;
			this.Resp = new ResponseDescription();
			this.Buffer = new byte[1024];
			this.ValidThrough = 0;
		}

		// Token: 0x04002547 RID: 9543
		private const int bufferSize = 1024;

		// Token: 0x04002548 RID: 9544
		internal ResponseDescription Resp;

		// Token: 0x04002549 RID: 9545
		internal int ValidThrough;

		// Token: 0x0400254A RID: 9546
		internal byte[] Buffer;

		// Token: 0x0400254B RID: 9547
		internal CommandStream Connection;
	}
}
