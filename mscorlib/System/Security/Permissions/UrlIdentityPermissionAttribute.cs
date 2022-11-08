using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000647 RID: 1607
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039F3 RID: 14835 RVA: 0x000C29F7 File Offset: 0x000C19F7
		public UrlIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x000C2A00 File Offset: 0x000C1A00
		// (set) Token: 0x060039F5 RID: 14837 RVA: 0x000C2A08 File Offset: 0x000C1A08
		public string Url
		{
			get
			{
				return this.m_url;
			}
			set
			{
				this.m_url = value;
			}
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x000C2A11 File Offset: 0x000C1A11
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new UrlIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.m_url == null)
			{
				return new UrlIdentityPermission(PermissionState.None);
			}
			return new UrlIdentityPermission(this.m_url);
		}

		// Token: 0x04001E1A RID: 7706
		private string m_url;
	}
}
