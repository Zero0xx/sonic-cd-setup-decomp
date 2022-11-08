using System;
using System.Net.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042C RID: 1068
	internal class SslStreamContext : TransportContext
	{
		// Token: 0x0600215E RID: 8542 RVA: 0x00084065 File Offset: 0x00083065
		internal SslStreamContext(SslStream sslStream)
		{
			this.sslStream = sslStream;
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x00084074 File Offset: 0x00083074
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			return this.sslStream.GetChannelBinding(kind);
		}

		// Token: 0x04002183 RID: 8579
		private SslStream sslStream;
	}
}
