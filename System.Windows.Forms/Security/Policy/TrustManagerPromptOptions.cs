using System;

namespace System.Security.Policy
{
	// Token: 0x02000714 RID: 1812
	[Flags]
	internal enum TrustManagerPromptOptions
	{
		// Token: 0x04003A55 RID: 14933
		None = 0,
		// Token: 0x04003A56 RID: 14934
		StopApp = 1,
		// Token: 0x04003A57 RID: 14935
		RequiresPermissions = 2,
		// Token: 0x04003A58 RID: 14936
		WillHaveFullTrust = 4,
		// Token: 0x04003A59 RID: 14937
		AddsShortcut = 8,
		// Token: 0x04003A5A RID: 14938
		LocalNetworkSource = 16,
		// Token: 0x04003A5B RID: 14939
		LocalComputerSource = 32,
		// Token: 0x04003A5C RID: 14940
		InternetSource = 64,
		// Token: 0x04003A5D RID: 14941
		TrustedSitesSource = 128,
		// Token: 0x04003A5E RID: 14942
		UntrustedSitesSource = 256
	}
}
