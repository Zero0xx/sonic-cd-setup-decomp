using System;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x020004FE RID: 1278
	internal struct AddressInfo
	{
		// Token: 0x0400271B RID: 10011
		internal AddressInfoHints ai_flags;

		// Token: 0x0400271C RID: 10012
		internal AddressFamily ai_family;

		// Token: 0x0400271D RID: 10013
		internal SocketType ai_socktype;

		// Token: 0x0400271E RID: 10014
		internal ProtocolFamily ai_protocol;

		// Token: 0x0400271F RID: 10015
		internal int ai_addrlen;

		// Token: 0x04002720 RID: 10016
		internal unsafe sbyte* ai_canonname;

		// Token: 0x04002721 RID: 10017
		internal unsafe byte* ai_addr;

		// Token: 0x04002722 RID: 10018
		internal unsafe AddressInfo* ai_next;
	}
}
