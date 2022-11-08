using System;

namespace System.Net.Mail
{
	// Token: 0x0200068A RID: 1674
	internal enum ServerState
	{
		// Token: 0x04002FC0 RID: 12224
		Starting = 1,
		// Token: 0x04002FC1 RID: 12225
		Started,
		// Token: 0x04002FC2 RID: 12226
		Stopping,
		// Token: 0x04002FC3 RID: 12227
		Stopped,
		// Token: 0x04002FC4 RID: 12228
		Pausing,
		// Token: 0x04002FC5 RID: 12229
		Paused,
		// Token: 0x04002FC6 RID: 12230
		Continuing
	}
}
