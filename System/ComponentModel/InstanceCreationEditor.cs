using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000F9 RID: 249
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class InstanceCreationEditor
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x0001BFF4 File Offset: 0x0001AFF4
		public virtual string Text
		{
			get
			{
				return SR.GetString("InstanceCreationEditorDefaultText");
			}
		}

		// Token: 0x06000809 RID: 2057
		public abstract object CreateInstance(ITypeDescriptorContext context, Type instanceType);
	}
}
