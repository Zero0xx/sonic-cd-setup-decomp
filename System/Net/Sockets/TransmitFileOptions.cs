using System;

namespace System.Net.Sockets
{
	// Token: 0x020005C9 RID: 1481
	[Flags]
	public enum TransmitFileOptions
	{
		// Token: 0x04002C24 RID: 11300
		UseDefaultWorkerThread = 0,
		// Token: 0x04002C25 RID: 11301
		Disconnect = 1,
		// Token: 0x04002C26 RID: 11302
		ReuseSocket = 2,
		// Token: 0x04002C27 RID: 11303
		WriteBehind = 4,
		// Token: 0x04002C28 RID: 11304
		UseSystemThread = 16,
		// Token: 0x04002C29 RID: 11305
		UseKernelApc = 32
	}
}
