using System;

namespace System.Net
{
	// Token: 0x020004EB RID: 1259
	internal static class GlobalSSPI
	{
		// Token: 0x040026AF RID: 9903
		internal static SSPIInterface SSPIAuth = new SSPIAuthType();

		// Token: 0x040026B0 RID: 9904
		internal static SSPIInterface SSPISecureChannel = new SSPISecureChannelType();
	}
}
