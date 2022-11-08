using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F8 RID: 1528
	internal struct IpAdapterUnicastAddress
	{
		// Token: 0x04002D0F RID: 11535
		internal uint length;

		// Token: 0x04002D10 RID: 11536
		internal AdapterAddressFlags flags;

		// Token: 0x04002D11 RID: 11537
		internal IntPtr next;

		// Token: 0x04002D12 RID: 11538
		internal IpSocketAddress address;

		// Token: 0x04002D13 RID: 11539
		internal PrefixOrigin prefixOrigin;

		// Token: 0x04002D14 RID: 11540
		internal SuffixOrigin suffixOrigin;

		// Token: 0x04002D15 RID: 11541
		internal DuplicateAddressDetectionState dadState;

		// Token: 0x04002D16 RID: 11542
		internal uint validLifetime;

		// Token: 0x04002D17 RID: 11543
		internal uint preferredLifetime;

		// Token: 0x04002D18 RID: 11544
		internal uint leaseLifetime;
	}
}
