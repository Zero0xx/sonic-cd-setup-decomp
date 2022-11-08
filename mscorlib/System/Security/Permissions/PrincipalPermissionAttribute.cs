using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200063F RID: 1599
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class PrincipalPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003999 RID: 14745 RVA: 0x000C22AC File Offset: 0x000C12AC
		public PrincipalPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x0600399A RID: 14746 RVA: 0x000C22BC File Offset: 0x000C12BC
		// (set) Token: 0x0600399B RID: 14747 RVA: 0x000C22C4 File Offset: 0x000C12C4
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x0600399C RID: 14748 RVA: 0x000C22CD File Offset: 0x000C12CD
		// (set) Token: 0x0600399D RID: 14749 RVA: 0x000C22D5 File Offset: 0x000C12D5
		public string Role
		{
			get
			{
				return this.m_role;
			}
			set
			{
				this.m_role = value;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x0600399E RID: 14750 RVA: 0x000C22DE File Offset: 0x000C12DE
		// (set) Token: 0x0600399F RID: 14751 RVA: 0x000C22E6 File Offset: 0x000C12E6
		public bool Authenticated
		{
			get
			{
				return this.m_authenticated;
			}
			set
			{
				this.m_authenticated = value;
			}
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x000C22EF File Offset: 0x000C12EF
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new PrincipalPermission(PermissionState.Unrestricted);
			}
			return new PrincipalPermission(this.m_name, this.m_role, this.m_authenticated);
		}

		// Token: 0x04001E09 RID: 7689
		private string m_name;

		// Token: 0x04001E0A RID: 7690
		private string m_role;

		// Token: 0x04001E0B RID: 7691
		private bool m_authenticated = true;
	}
}
