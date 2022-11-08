using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000107 RID: 263
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class LicenseContext : IServiceProvider
	{
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0001C220 File Offset: 0x0001B220
		public virtual LicenseUsageMode UsageMode
		{
			get
			{
				return LicenseUsageMode.Runtime;
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001C223 File Offset: 0x0001B223
		public virtual string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
		{
			return null;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001C226 File Offset: 0x0001B226
		public virtual object GetService(Type type)
		{
			return null;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001C229 File Offset: 0x0001B229
		public virtual void SetSavedLicenseKey(Type type, string key)
		{
		}
	}
}
