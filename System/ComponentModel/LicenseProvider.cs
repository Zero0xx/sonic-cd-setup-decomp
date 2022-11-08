using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200010C RID: 268
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class LicenseProvider
	{
		// Token: 0x06000867 RID: 2151
		public abstract License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions);
	}
}
