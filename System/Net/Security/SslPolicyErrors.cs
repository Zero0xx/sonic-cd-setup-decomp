using System;

namespace System.Net.Security
{
	// Token: 0x02000591 RID: 1425
	[Flags]
	public enum SslPolicyErrors
	{
		// Token: 0x040029EF RID: 10735
		None = 0,
		// Token: 0x040029F0 RID: 10736
		RemoteCertificateNotAvailable = 1,
		// Token: 0x040029F1 RID: 10737
		RemoteCertificateNameMismatch = 2,
		// Token: 0x040029F2 RID: 10738
		RemoteCertificateChainErrors = 4
	}
}
