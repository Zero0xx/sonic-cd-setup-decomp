using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000644 RID: 1604
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class ZoneIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039E3 RID: 14819 RVA: 0x000C289B File Offset: 0x000C189B
		public ZoneIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x000C28AB File Offset: 0x000C18AB
		// (set) Token: 0x060039E5 RID: 14821 RVA: 0x000C28B3 File Offset: 0x000C18B3
		public SecurityZone Zone
		{
			get
			{
				return this.m_flag;
			}
			set
			{
				this.m_flag = value;
			}
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x000C28BC File Offset: 0x000C18BC
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new ZoneIdentityPermission(PermissionState.Unrestricted);
			}
			return new ZoneIdentityPermission(this.m_flag);
		}

		// Token: 0x04001E15 RID: 7701
		private SecurityZone m_flag = SecurityZone.NoZone;
	}
}
