using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042E RID: 1070
	internal class CachedTransportContext : TransportContext
	{
		// Token: 0x06002162 RID: 8546 RVA: 0x000840D7 File Offset: 0x000830D7
		internal CachedTransportContext(ChannelBinding binding)
		{
			this.binding = binding;
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000840E6 File Offset: 0x000830E6
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			if (kind != ChannelBindingKind.Endpoint)
			{
				return null;
			}
			return this.binding;
		}

		// Token: 0x04002185 RID: 8581
		private ChannelBinding binding;
	}
}
