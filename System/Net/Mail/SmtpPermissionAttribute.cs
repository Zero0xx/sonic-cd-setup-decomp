using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006D1 RID: 1745
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SmtpPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060035DC RID: 13788 RVA: 0x000E5D41 File Offset: 0x000E4D41
		public SmtpPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x000E5D4A File Offset: 0x000E4D4A
		// (set) Token: 0x060035DE RID: 13790 RVA: 0x000E5D52 File Offset: 0x000E4D52
		public string Access
		{
			get
			{
				return this.access;
			}
			set
			{
				this.access = value;
			}
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x000E5D5C File Offset: 0x000E4D5C
		public override IPermission CreatePermission()
		{
			SmtpPermission smtpPermission;
			if (base.Unrestricted)
			{
				smtpPermission = new SmtpPermission(PermissionState.Unrestricted);
			}
			else
			{
				smtpPermission = new SmtpPermission(PermissionState.None);
				if (this.access != null)
				{
					if (string.Compare(this.access, "Connect", StringComparison.OrdinalIgnoreCase) == 0)
					{
						smtpPermission.AddPermission(SmtpAccess.Connect);
					}
					else if (string.Compare(this.access, "ConnectToUnrestrictedPort", StringComparison.OrdinalIgnoreCase) == 0)
					{
						smtpPermission.AddPermission(SmtpAccess.ConnectToUnrestrictedPort);
					}
					else
					{
						if (string.Compare(this.access, "None", StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[]
							{
								"Access",
								this.access
							}));
						}
						smtpPermission.AddPermission(SmtpAccess.None);
					}
				}
			}
			return smtpPermission;
		}

		// Token: 0x0400310D RID: 12557
		private const string strAccess = "Access";

		// Token: 0x0400310E RID: 12558
		private string access;
	}
}
