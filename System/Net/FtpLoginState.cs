using System;

namespace System.Net
{
	// Token: 0x020004D9 RID: 1241
	internal enum FtpLoginState : byte
	{
		// Token: 0x0400262E RID: 9774
		NotLoggedIn,
		// Token: 0x0400262F RID: 9775
		LoggedIn,
		// Token: 0x04002630 RID: 9776
		LoggedInButNeedsRelogin,
		// Token: 0x04002631 RID: 9777
		ReloginFailed
	}
}
