using System;

namespace System.Net
{
	// Token: 0x020003BC RID: 956
	[Flags]
	internal enum FtpMethodFlags
	{
		// Token: 0x04001DF6 RID: 7670
		None = 0,
		// Token: 0x04001DF7 RID: 7671
		IsDownload = 1,
		// Token: 0x04001DF8 RID: 7672
		IsUpload = 2,
		// Token: 0x04001DF9 RID: 7673
		TakesParameter = 4,
		// Token: 0x04001DFA RID: 7674
		MayTakeParameter = 8,
		// Token: 0x04001DFB RID: 7675
		DoesNotTakeParameter = 16,
		// Token: 0x04001DFC RID: 7676
		ParameterIsDirectory = 32,
		// Token: 0x04001DFD RID: 7677
		ShouldParseForResponseUri = 64,
		// Token: 0x04001DFE RID: 7678
		HasHttpCommand = 128
	}
}
