using System;

namespace System.Net.Sockets
{
	// Token: 0x020005C6 RID: 1478
	public enum SocketType
	{
		// Token: 0x04002C14 RID: 11284
		Stream = 1,
		// Token: 0x04002C15 RID: 11285
		Dgram,
		// Token: 0x04002C16 RID: 11286
		Raw,
		// Token: 0x04002C17 RID: 11287
		Rdm,
		// Token: 0x04002C18 RID: 11288
		Seqpacket,
		// Token: 0x04002C19 RID: 11289
		Unknown = -1
	}
}
