using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000609 RID: 1545
	internal struct IcmpEchoReply
	{
		// Token: 0x04002DA8 RID: 11688
		internal uint address;

		// Token: 0x04002DA9 RID: 11689
		internal uint status;

		// Token: 0x04002DAA RID: 11690
		internal uint roundTripTime;

		// Token: 0x04002DAB RID: 11691
		internal ushort dataSize;

		// Token: 0x04002DAC RID: 11692
		internal ushort reserved;

		// Token: 0x04002DAD RID: 11693
		internal IntPtr data;

		// Token: 0x04002DAE RID: 11694
		internal IPOptions options;
	}
}
