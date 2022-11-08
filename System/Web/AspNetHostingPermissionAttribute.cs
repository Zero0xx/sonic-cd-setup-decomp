using System;
using System.Security;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000738 RID: 1848
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class AspNetHostingPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600384B RID: 14411 RVA: 0x000ED7E1 File Offset: 0x000EC7E1
		public AspNetHostingPermissionAttribute(SecurityAction action) : base(action)
		{
			this._level = AspNetHostingPermissionLevel.None;
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x0600384C RID: 14412 RVA: 0x000ED7F2 File Offset: 0x000EC7F2
		// (set) Token: 0x0600384D RID: 14413 RVA: 0x000ED7FA File Offset: 0x000EC7FA
		public AspNetHostingPermissionLevel Level
		{
			get
			{
				return this._level;
			}
			set
			{
				AspNetHostingPermission.VerifyAspNetHostingPermissionLevel(value, "Level");
				this._level = value;
			}
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000ED80E File Offset: 0x000EC80E
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new AspNetHostingPermission(PermissionState.Unrestricted);
			}
			return new AspNetHostingPermission(this._level);
		}

		// Token: 0x0400323A RID: 12858
		private AspNetHostingPermissionLevel _level;
	}
}
