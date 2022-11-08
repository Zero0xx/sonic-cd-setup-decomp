using System;

namespace System.Net.Sockets
{
	// Token: 0x020005A4 RID: 1444
	[Flags]
	internal enum AsyncEventBits
	{
		// Token: 0x04002A83 RID: 10883
		FdNone = 0,
		// Token: 0x04002A84 RID: 10884
		FdRead = 1,
		// Token: 0x04002A85 RID: 10885
		FdWrite = 2,
		// Token: 0x04002A86 RID: 10886
		FdOob = 4,
		// Token: 0x04002A87 RID: 10887
		FdAccept = 8,
		// Token: 0x04002A88 RID: 10888
		FdConnect = 16,
		// Token: 0x04002A89 RID: 10889
		FdClose = 32,
		// Token: 0x04002A8A RID: 10890
		FdQos = 64,
		// Token: 0x04002A8B RID: 10891
		FdGroupQos = 128,
		// Token: 0x04002A8C RID: 10892
		FdRoutingInterfaceChange = 256,
		// Token: 0x04002A8D RID: 10893
		FdAddressListChange = 512,
		// Token: 0x04002A8E RID: 10894
		FdAllEvents = 1023
	}
}
