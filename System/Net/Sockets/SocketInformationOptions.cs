using System;

namespace System.Net.Sockets
{
	// Token: 0x020005AF RID: 1455
	[Flags]
	public enum SocketInformationOptions
	{
		// Token: 0x04002B08 RID: 11016
		NonBlocking = 1,
		// Token: 0x04002B09 RID: 11017
		Connected = 2,
		// Token: 0x04002B0A RID: 11018
		Listening = 4,
		// Token: 0x04002B0B RID: 11019
		UseOnlyOverlappedIO = 8
	}
}
