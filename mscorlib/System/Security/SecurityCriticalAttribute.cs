using System;

namespace System.Security
{
	// Token: 0x0200066A RID: 1642
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class SecurityCriticalAttribute : Attribute
	{
		// Token: 0x06003B1F RID: 15135 RVA: 0x000C86C7 File Offset: 0x000C76C7
		public SecurityCriticalAttribute()
		{
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x000C86CF File Offset: 0x000C76CF
		public SecurityCriticalAttribute(SecurityCriticalScope scope)
		{
			this._val = scope;
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x000C86DE File Offset: 0x000C76DE
		public SecurityCriticalScope Scope
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001E96 RID: 7830
		internal SecurityCriticalScope _val;
	}
}
