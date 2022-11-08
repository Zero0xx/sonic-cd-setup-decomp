using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006C7 RID: 1735
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ContextProperty
	{
		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06003EAA RID: 16042 RVA: 0x000D6F18 File Offset: 0x000D5F18
		public virtual string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06003EAB RID: 16043 RVA: 0x000D6F20 File Offset: 0x000D5F20
		public virtual object Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x000D6F28 File Offset: 0x000D5F28
		internal ContextProperty(string name, object prop)
		{
			this._name = name;
			this._property = prop;
		}

		// Token: 0x04001FE6 RID: 8166
		internal string _name;

		// Token: 0x04001FE7 RID: 8167
		internal object _property;
	}
}
