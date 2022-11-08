using System;

namespace System.Net
{
	// Token: 0x02000542 RID: 1346
	internal enum SchProtocols
	{
		// Token: 0x040027DE RID: 10206
		Zero,
		// Token: 0x040027DF RID: 10207
		PctClient = 2,
		// Token: 0x040027E0 RID: 10208
		PctServer = 1,
		// Token: 0x040027E1 RID: 10209
		Pct = 3,
		// Token: 0x040027E2 RID: 10210
		Ssl2Client = 8,
		// Token: 0x040027E3 RID: 10211
		Ssl2Server = 4,
		// Token: 0x040027E4 RID: 10212
		Ssl2 = 12,
		// Token: 0x040027E5 RID: 10213
		Ssl3Client = 32,
		// Token: 0x040027E6 RID: 10214
		Ssl3Server = 16,
		// Token: 0x040027E7 RID: 10215
		Ssl3 = 48,
		// Token: 0x040027E8 RID: 10216
		TlsClient = 128,
		// Token: 0x040027E9 RID: 10217
		TlsServer = 64,
		// Token: 0x040027EA RID: 10218
		Tls = 192,
		// Token: 0x040027EB RID: 10219
		Tls11Client = 512,
		// Token: 0x040027EC RID: 10220
		Tls11Server = 256,
		// Token: 0x040027ED RID: 10221
		Tls11 = 768,
		// Token: 0x040027EE RID: 10222
		Tls12Client = 2048,
		// Token: 0x040027EF RID: 10223
		Tls12Server = 1024,
		// Token: 0x040027F0 RID: 10224
		Tls12 = 3072,
		// Token: 0x040027F1 RID: 10225
		Ssl3Tls = 240,
		// Token: 0x040027F2 RID: 10226
		UniClient = -2147483648,
		// Token: 0x040027F3 RID: 10227
		UniServer = 1073741824,
		// Token: 0x040027F4 RID: 10228
		Unified = -1073741824,
		// Token: 0x040027F5 RID: 10229
		ClientMask = -2147480918,
		// Token: 0x040027F6 RID: 10230
		ServerMask = 1073743189
	}
}
