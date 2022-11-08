using System;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x02000695 RID: 1685
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	public abstract class SecurityState
	{
		// Token: 0x06003D13 RID: 15635 RVA: 0x000D10E0 File Offset: 0x000D00E0
		public bool IsStateAvailable()
		{
			AppDomainManager currentAppDomainManager = AppDomainManager.CurrentAppDomainManager;
			return currentAppDomainManager != null && currentAppDomainManager.CheckSecuritySettings(this);
		}

		// Token: 0x06003D14 RID: 15636
		public abstract void EnsureState();
	}
}
