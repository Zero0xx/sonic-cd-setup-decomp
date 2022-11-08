using System;

namespace System.Net
{
	// Token: 0x020004BF RID: 1215
	internal enum DataParseStatus
	{
		// Token: 0x04002552 RID: 9554
		NeedMoreData,
		// Token: 0x04002553 RID: 9555
		ContinueParsing,
		// Token: 0x04002554 RID: 9556
		Done,
		// Token: 0x04002555 RID: 9557
		Invalid,
		// Token: 0x04002556 RID: 9558
		DataTooBig
	}
}
