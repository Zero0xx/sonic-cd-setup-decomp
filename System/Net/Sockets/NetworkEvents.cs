using System;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x020005A6 RID: 1446
	internal struct NetworkEvents
	{
		// Token: 0x04002A9B RID: 10907
		public AsyncEventBits Events;

		// Token: 0x04002A9C RID: 10908
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] ErrorCodes;
	}
}
