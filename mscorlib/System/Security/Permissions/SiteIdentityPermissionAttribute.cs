using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000646 RID: 1606
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SiteIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039EF RID: 14831 RVA: 0x000C29B2 File Offset: 0x000C19B2
		public SiteIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060039F0 RID: 14832 RVA: 0x000C29BB File Offset: 0x000C19BB
		// (set) Token: 0x060039F1 RID: 14833 RVA: 0x000C29C3 File Offset: 0x000C19C3
		public string Site
		{
			get
			{
				return this.m_site;
			}
			set
			{
				this.m_site = value;
			}
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x000C29CC File Offset: 0x000C19CC
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new SiteIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.m_site == null)
			{
				return new SiteIdentityPermission(PermissionState.None);
			}
			return new SiteIdentityPermission(this.m_site);
		}

		// Token: 0x04001E19 RID: 7705
		private string m_site;
	}
}
