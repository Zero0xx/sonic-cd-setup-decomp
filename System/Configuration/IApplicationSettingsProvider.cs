using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020006F6 RID: 1782
	public interface IApplicationSettingsProvider
	{
		// Token: 0x060036FC RID: 14076
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property);

		// Token: 0x060036FD RID: 14077
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Reset(SettingsContext context);

		// Token: 0x060036FE RID: 14078
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Upgrade(SettingsContext context, SettingsPropertyCollection properties);
	}
}
