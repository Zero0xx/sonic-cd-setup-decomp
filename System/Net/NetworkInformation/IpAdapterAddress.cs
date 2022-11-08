using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F7 RID: 1527
	internal struct IpAdapterAddress
	{
		// Token: 0x04002D0B RID: 11531
		internal uint length;

		// Token: 0x04002D0C RID: 11532
		internal AdapterAddressFlags flags;

		// Token: 0x04002D0D RID: 11533
		internal IntPtr next;

		// Token: 0x04002D0E RID: 11534
		internal IpSocketAddress address;
	}
}
