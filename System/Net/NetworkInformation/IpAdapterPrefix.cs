using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F9 RID: 1529
	internal struct IpAdapterPrefix
	{
		// Token: 0x04002D19 RID: 11545
		internal uint length;

		// Token: 0x04002D1A RID: 11546
		internal uint ifIndex;

		// Token: 0x04002D1B RID: 11547
		internal IntPtr next;

		// Token: 0x04002D1C RID: 11548
		internal IpSocketAddress address;

		// Token: 0x04002D1D RID: 11549
		internal uint prefixLength;
	}
}
