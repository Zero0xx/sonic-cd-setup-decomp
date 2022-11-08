using System;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005EB RID: 1515
	internal class IpHelperErrors
	{
		// Token: 0x06002FC0 RID: 12224 RVA: 0x000CEF2A File Offset: 0x000CDF2A
		internal static void CheckFamilyUnspecified(AddressFamily family)
		{
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6 && family != AddressFamily.Unspecified)
			{
				throw new ArgumentException(SR.GetString("net_invalidversion"), "family");
			}
		}

		// Token: 0x04002CB5 RID: 11445
		internal const uint Success = 0U;

		// Token: 0x04002CB6 RID: 11446
		internal const uint ErrorInvalidFunction = 1U;

		// Token: 0x04002CB7 RID: 11447
		internal const uint ErrorNoSuchDevice = 2U;

		// Token: 0x04002CB8 RID: 11448
		internal const uint ErrorInvalidData = 13U;

		// Token: 0x04002CB9 RID: 11449
		internal const uint ErrorInvalidParameter = 87U;

		// Token: 0x04002CBA RID: 11450
		internal const uint ErrorBufferOverflow = 111U;

		// Token: 0x04002CBB RID: 11451
		internal const uint ErrorInsufficientBuffer = 122U;

		// Token: 0x04002CBC RID: 11452
		internal const uint ErrorNoData = 232U;

		// Token: 0x04002CBD RID: 11453
		internal const uint Pending = 997U;

		// Token: 0x04002CBE RID: 11454
		internal const uint ErrorNotFound = 1168U;
	}
}
