using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000618 RID: 1560
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class NetworkInformationPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003007 RID: 12295 RVA: 0x000CF821 File Offset: 0x000CE821
		public NetworkInformationPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x000CF82A File Offset: 0x000CE82A
		// (set) Token: 0x06003009 RID: 12297 RVA: 0x000CF832 File Offset: 0x000CE832
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

		// Token: 0x0600300A RID: 12298 RVA: 0x000CF83C File Offset: 0x000CE83C
		public override IPermission CreatePermission()
		{
			NetworkInformationPermission networkInformationPermission;
			if (base.Unrestricted)
			{
				networkInformationPermission = new NetworkInformationPermission(PermissionState.Unrestricted);
			}
			else
			{
				networkInformationPermission = new NetworkInformationPermission(PermissionState.None);
				if (this.access != null)
				{
					if (string.Compare(this.access, "Read", StringComparison.OrdinalIgnoreCase) == 0)
					{
						networkInformationPermission.AddPermission(NetworkInformationAccess.Read);
					}
					else if (string.Compare(this.access, "Ping", StringComparison.OrdinalIgnoreCase) == 0)
					{
						networkInformationPermission.AddPermission(NetworkInformationAccess.Ping);
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
						networkInformationPermission.AddPermission(NetworkInformationAccess.None);
					}
				}
			}
			return networkInformationPermission;
		}

		// Token: 0x04002DD1 RID: 11729
		private const string strAccess = "Access";

		// Token: 0x04002DD2 RID: 11730
		private string access;
	}
}
