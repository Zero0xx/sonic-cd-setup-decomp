using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000E0 RID: 224
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class EventDescriptor : MemberDescriptor
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x0001B364 File Offset: 0x0001A364
		protected EventDescriptor(string name, Attribute[] attrs) : base(name, attrs)
		{
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001B36E File Offset: 0x0001A36E
		protected EventDescriptor(MemberDescriptor descr) : base(descr)
		{
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001B377 File Offset: 0x0001A377
		protected EventDescriptor(MemberDescriptor descr, Attribute[] attrs) : base(descr, attrs)
		{
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600077C RID: 1916
		public abstract Type ComponentType { get; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600077D RID: 1917
		public abstract Type EventType { get; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600077E RID: 1918
		public abstract bool IsMulticast { get; }

		// Token: 0x0600077F RID: 1919
		public abstract void AddEventHandler(object component, Delegate value);

		// Token: 0x06000780 RID: 1920
		public abstract void RemoveEventHandler(object component, Delegate value);
	}
}
