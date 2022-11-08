using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200042A RID: 1066
	public abstract class TransportContext
	{
		// Token: 0x0600215A RID: 8538
		public abstract ChannelBinding GetChannelBinding(ChannelBindingKind kind);
	}
}
