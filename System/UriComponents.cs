using System;

namespace System
{
	// Token: 0x02000367 RID: 871
	[Flags]
	public enum UriComponents
	{
		// Token: 0x04001C53 RID: 7251
		Scheme = 1,
		// Token: 0x04001C54 RID: 7252
		UserInfo = 2,
		// Token: 0x04001C55 RID: 7253
		Host = 4,
		// Token: 0x04001C56 RID: 7254
		Port = 8,
		// Token: 0x04001C57 RID: 7255
		Path = 16,
		// Token: 0x04001C58 RID: 7256
		Query = 32,
		// Token: 0x04001C59 RID: 7257
		Fragment = 64,
		// Token: 0x04001C5A RID: 7258
		StrongPort = 128,
		// Token: 0x04001C5B RID: 7259
		KeepDelimiter = 1073741824,
		// Token: 0x04001C5C RID: 7260
		SerializationInfoString = -2147483648,
		// Token: 0x04001C5D RID: 7261
		AbsoluteUri = 127,
		// Token: 0x04001C5E RID: 7262
		HostAndPort = 132,
		// Token: 0x04001C5F RID: 7263
		StrongAuthority = 134,
		// Token: 0x04001C60 RID: 7264
		SchemeAndServer = 13,
		// Token: 0x04001C61 RID: 7265
		HttpRequestUrl = 61,
		// Token: 0x04001C62 RID: 7266
		PathAndQuery = 48
	}
}
