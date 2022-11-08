using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005F1 RID: 1521
	internal struct IPExtendedAddress
	{
		// Token: 0x06002FC2 RID: 12226 RVA: 0x000CEF55 File Offset: 0x000CDF55
		internal IPExtendedAddress(IPAddress address, IPAddress mask)
		{
			this.address = address;
			this.mask = mask;
		}

		// Token: 0x04002CDF RID: 11487
		internal IPAddress mask;

		// Token: 0x04002CE0 RID: 11488
		internal IPAddress address;
	}
}
