using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042B RID: 1067
	internal class ConnectStreamContext : TransportContext
	{
		// Token: 0x0600215C RID: 8540 RVA: 0x00084048 File Offset: 0x00083048
		internal ConnectStreamContext(ConnectStream connectStream)
		{
			this.connectStream = connectStream;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x00084057 File Offset: 0x00083057
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			return this.connectStream.GetChannelBinding(kind);
		}

		// Token: 0x04002182 RID: 8578
		private ConnectStream connectStream;
	}
}
