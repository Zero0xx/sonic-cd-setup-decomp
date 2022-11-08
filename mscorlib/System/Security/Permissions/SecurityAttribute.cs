using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000630 RID: 1584
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public abstract class SecurityAttribute : Attribute
	{
		// Token: 0x06003925 RID: 14629 RVA: 0x000C12A7 File Offset: 0x000C02A7
		protected SecurityAttribute(SecurityAction action)
		{
			this.m_action = action;
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06003926 RID: 14630 RVA: 0x000C12B6 File Offset: 0x000C02B6
		// (set) Token: 0x06003927 RID: 14631 RVA: 0x000C12BE File Offset: 0x000C02BE
		public SecurityAction Action
		{
			get
			{
				return this.m_action;
			}
			set
			{
				this.m_action = value;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06003928 RID: 14632 RVA: 0x000C12C7 File Offset: 0x000C02C7
		// (set) Token: 0x06003929 RID: 14633 RVA: 0x000C12CF File Offset: 0x000C02CF
		public bool Unrestricted
		{
			get
			{
				return this.m_unrestricted;
			}
			set
			{
				this.m_unrestricted = value;
			}
		}

		// Token: 0x0600392A RID: 14634
		public abstract IPermission CreatePermission();

		// Token: 0x0600392B RID: 14635 RVA: 0x000C12D8 File Offset: 0x000C02D8
		internal static IntPtr FindSecurityAttributeTypeHandle(string typeName)
		{
			PermissionSet.s_fullTrust.Assert();
			Type type = Type.GetType(typeName, false, false);
			if (type == null)
			{
				return IntPtr.Zero;
			}
			return type.TypeHandle.Value;
		}

		// Token: 0x04001DAD RID: 7597
		internal SecurityAction m_action;

		// Token: 0x04001DAE RID: 7598
		internal bool m_unrestricted;
	}
}
