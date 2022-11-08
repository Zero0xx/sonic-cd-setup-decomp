using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B2 RID: 1714
	internal class ChannelServicesData
	{
		// Token: 0x04001F94 RID: 8084
		internal long remoteCalls;

		// Token: 0x04001F95 RID: 8085
		internal CrossContextChannel xctxmessageSink;

		// Token: 0x04001F96 RID: 8086
		internal CrossAppDomainChannel xadmessageSink;

		// Token: 0x04001F97 RID: 8087
		internal bool fRegisterWellKnownChannels;
	}
}
