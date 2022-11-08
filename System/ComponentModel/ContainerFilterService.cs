using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000BF RID: 191
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class ContainerFilterService
	{
		// Token: 0x0600069C RID: 1692 RVA: 0x0001936F File Offset: 0x0001836F
		public virtual ComponentCollection FilterComponents(ComponentCollection components)
		{
			return components;
		}
	}
}
