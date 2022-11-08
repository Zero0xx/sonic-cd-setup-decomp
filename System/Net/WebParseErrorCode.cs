using System;

namespace System.Net
{
	// Token: 0x020004C2 RID: 1218
	internal enum WebParseErrorCode
	{
		// Token: 0x04002562 RID: 9570
		Generic,
		// Token: 0x04002563 RID: 9571
		InvalidHeaderName,
		// Token: 0x04002564 RID: 9572
		InvalidContentLength,
		// Token: 0x04002565 RID: 9573
		IncompleteHeaderLine,
		// Token: 0x04002566 RID: 9574
		CrLfError,
		// Token: 0x04002567 RID: 9575
		InvalidChunkFormat,
		// Token: 0x04002568 RID: 9576
		UnexpectedServerResponse
	}
}
