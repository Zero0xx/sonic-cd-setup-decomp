using System;

namespace System.Net
{
	// Token: 0x020003F4 RID: 1012
	internal enum ContextAttribute
	{
		// Token: 0x0400200F RID: 8207
		Sizes,
		// Token: 0x04002010 RID: 8208
		Names,
		// Token: 0x04002011 RID: 8209
		Lifespan,
		// Token: 0x04002012 RID: 8210
		DceInfo,
		// Token: 0x04002013 RID: 8211
		StreamSizes,
		// Token: 0x04002014 RID: 8212
		Authority = 6,
		// Token: 0x04002015 RID: 8213
		PackageInfo = 10,
		// Token: 0x04002016 RID: 8214
		NegotiationInfo = 12,
		// Token: 0x04002017 RID: 8215
		UniqueBindings = 25,
		// Token: 0x04002018 RID: 8216
		EndpointBindings,
		// Token: 0x04002019 RID: 8217
		ClientSpecifiedSpn,
		// Token: 0x0400201A RID: 8218
		RemoteCertificate = 83,
		// Token: 0x0400201B RID: 8219
		LocalCertificate,
		// Token: 0x0400201C RID: 8220
		RootStore,
		// Token: 0x0400201D RID: 8221
		IssuerListInfoEx = 89,
		// Token: 0x0400201E RID: 8222
		ConnectionInfo
	}
}
