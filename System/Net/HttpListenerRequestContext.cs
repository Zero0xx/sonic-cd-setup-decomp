using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042D RID: 1069
	internal class HttpListenerRequestContext : TransportContext
	{
		// Token: 0x06002160 RID: 8544 RVA: 0x00084082 File Offset: 0x00083082
		internal HttpListenerRequestContext(HttpListenerRequest request)
		{
			this.request = request;
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x00084094 File Offset: 0x00083094
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			if (kind != ChannelBindingKind.Endpoint)
			{
				throw new NotSupportedException(SR.GetString("net_listener_invalid_cbt_type", new object[]
				{
					kind.ToString()
				}));
			}
			return this.request.GetChannelBinding();
		}

		// Token: 0x04002184 RID: 8580
		private HttpListenerRequest request;
	}
}
