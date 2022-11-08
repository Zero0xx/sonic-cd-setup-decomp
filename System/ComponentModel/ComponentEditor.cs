using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000A2 RID: 162
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class ComponentEditor
	{
		// Token: 0x060005CF RID: 1487 RVA: 0x0001797C File Offset: 0x0001697C
		public bool EditComponent(object component)
		{
			return this.EditComponent(null, component);
		}

		// Token: 0x060005D0 RID: 1488
		public abstract bool EditComponent(ITypeDescriptorContext context, object component);
	}
}
