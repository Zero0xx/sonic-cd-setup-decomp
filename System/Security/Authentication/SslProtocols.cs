using System;

namespace System.Security.Authentication
{
	// Token: 0x0200058C RID: 1420
	[Flags]
	public enum SslProtocols
	{
		// Token: 0x040029D4 RID: 10708
		None = 0,
		// Token: 0x040029D5 RID: 10709
		Ssl2 = 12,
		// Token: 0x040029D6 RID: 10710
		Ssl3 = 48,
		// Token: 0x040029D7 RID: 10711
		Tls = 192,
		// Token: 0x040029D8 RID: 10712
		Default = 240
	}
}
