using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000106 RID: 262
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class License : IDisposable
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000839 RID: 2105
		public abstract string LicenseKey { get; }

		// Token: 0x0600083A RID: 2106
		public abstract void Dispose();
	}
}
