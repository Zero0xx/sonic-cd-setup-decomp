using System;

namespace System.Runtime.Versioning
{
	// Token: 0x0200091C RID: 2332
	[Flags]
	public enum ResourceScope
	{
		// Token: 0x04002C06 RID: 11270
		None = 0,
		// Token: 0x04002C07 RID: 11271
		Machine = 1,
		// Token: 0x04002C08 RID: 11272
		Process = 2,
		// Token: 0x04002C09 RID: 11273
		AppDomain = 4,
		// Token: 0x04002C0A RID: 11274
		Library = 8,
		// Token: 0x04002C0B RID: 11275
		Private = 16,
		// Token: 0x04002C0C RID: 11276
		Assembly = 32
	}
}
