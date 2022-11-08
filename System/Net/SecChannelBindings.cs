using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200040B RID: 1035
	[StructLayout(LayoutKind.Sequential)]
	internal class SecChannelBindings
	{
		// Token: 0x040020BA RID: 8378
		internal int dwInitiatorAddrType;

		// Token: 0x040020BB RID: 8379
		internal int cbInitiatorLength;

		// Token: 0x040020BC RID: 8380
		internal int dwInitiatorOffset;

		// Token: 0x040020BD RID: 8381
		internal int dwAcceptorAddrType;

		// Token: 0x040020BE RID: 8382
		internal int cbAcceptorLength;

		// Token: 0x040020BF RID: 8383
		internal int dwAcceptorOffset;

		// Token: 0x040020C0 RID: 8384
		internal int cbApplicationDataLength;

		// Token: 0x040020C1 RID: 8385
		internal int dwApplicationDataOffset;
	}
}
