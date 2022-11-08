using System;

namespace System.Net
{
	// Token: 0x02000409 RID: 1033
	internal enum CertificateEncoding
	{
		// Token: 0x04002094 RID: 8340
		Zero,
		// Token: 0x04002095 RID: 8341
		X509AsnEncoding,
		// Token: 0x04002096 RID: 8342
		X509NdrEncoding,
		// Token: 0x04002097 RID: 8343
		Pkcs7AsnEncoding = 65536,
		// Token: 0x04002098 RID: 8344
		Pkcs7NdrEncoding = 131072,
		// Token: 0x04002099 RID: 8345
		AnyAsnEncoding = 65537
	}
}
