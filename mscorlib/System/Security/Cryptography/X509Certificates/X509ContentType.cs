using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008C0 RID: 2240
	[ComVisible(true)]
	public enum X509ContentType
	{
		// Token: 0x04002A0A RID: 10762
		Unknown,
		// Token: 0x04002A0B RID: 10763
		Cert,
		// Token: 0x04002A0C RID: 10764
		SerializedCert,
		// Token: 0x04002A0D RID: 10765
		Pfx,
		// Token: 0x04002A0E RID: 10766
		Pkcs12 = 3,
		// Token: 0x04002A0F RID: 10767
		SerializedStore,
		// Token: 0x04002A10 RID: 10768
		Pkcs7,
		// Token: 0x04002A11 RID: 10769
		Authenticode
	}
}
