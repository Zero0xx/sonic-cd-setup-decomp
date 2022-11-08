using System;

namespace System
{
	// Token: 0x0200036A RID: 874
	[Flags]
	public enum GenericUriParserOptions
	{
		// Token: 0x04001C6C RID: 7276
		Default = 0,
		// Token: 0x04001C6D RID: 7277
		GenericAuthority = 1,
		// Token: 0x04001C6E RID: 7278
		AllowEmptyAuthority = 2,
		// Token: 0x04001C6F RID: 7279
		NoUserInfo = 4,
		// Token: 0x04001C70 RID: 7280
		NoPort = 8,
		// Token: 0x04001C71 RID: 7281
		NoQuery = 16,
		// Token: 0x04001C72 RID: 7282
		NoFragment = 32,
		// Token: 0x04001C73 RID: 7283
		DontConvertPathBackslashes = 64,
		// Token: 0x04001C74 RID: 7284
		DontCompressPath = 128,
		// Token: 0x04001C75 RID: 7285
		DontUnescapePathDotsAndSlashes = 256,
		// Token: 0x04001C76 RID: 7286
		Idn = 512,
		// Token: 0x04001C77 RID: 7287
		IriParsing = 1024
	}
}
